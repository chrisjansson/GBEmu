using System;
using System.IO;
using System.Linq;
using Core;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class AcceptanceTest
    {
        protected Cpu Sut;
        protected Fixture Fixture;
        protected MMuSpy Mmu;

        public AcceptanceTest()
        {
            Fixture = new Fixture();
            Mmu = new MMuSpy(new Core.MMU
            {
                Display = new NullDisplay()
            });
            Sut = new Cpu(Mmu);
        }

        private void LoadTest(string rom)
        {
            var ldAcceptanceTestRom = File.ReadAllBytes(rom);

            for (var i = 0; i < ldAcceptanceTestRom.Length; i++)
            {
                var n = ldAcceptanceTestRom[i];
                Mmu.SetByte((ushort)i, n);
            }
        }

        [Theory]
        [InlineData("01-special.gb")]
        [InlineData("02-interrupts.gb")]
        [InlineData("03-op sp,hl.gb")]
        [InlineData("04-op r,imm.gb")]
        [InlineData("05-op rp.gb")]
        [InlineData("06-ld r,r.gb")]
        [InlineData("07-jr,jp,call,ret,rst.gb")]
        [InlineData("08-misc instrs.gb")]
        [InlineData("09-op r,r.gb")]
        [InlineData("10-bit ops.gb")]
        [InlineData("11-op a,(hl).gb")]
        public void Passes_cpu_instructions_tests(string rom)
        {
            LoadTest(rom);
            Sut.ProgramCounter = 0x00;

            var previewsProgramCounter = 0x00;
            var previousProgramCounterCount = 0;
            for (var i = 0; i < 100000000 && previousProgramCounterCount < 1000; i++)
            {
                var instruction = Mmu.GetByte(Sut.ProgramCounter);
                Sut.Execute(instruction);

                if ((Mmu.GetByte(0xFF02) & 0x80) == 0x80)
                {
                    Mmu.SetByte(0xFF02, 0);
                }

                if (Sut.ProgramCounter == previewsProgramCounter)
                {
                    previousProgramCounterCount++;
                }
                else
                {
                    previewsProgramCounter = Sut.ProgramCounter;
                    previousProgramCounterCount = 0;
                }
            }

            var output = Mmu.Output.Aggregate("", (x, y) => x + y);
            Console.WriteLine(output);
            Assert.Contains("passed", output.ToLower());
        }
    }
}
