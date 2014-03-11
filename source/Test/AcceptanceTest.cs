using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class AcceptanceTest : TestBase
    {
        private void LoadTest(string rom)
        {
            var ldAcceptanceTestRom = File.ReadAllBytes(rom);

            for (var i = 0; i < ldAcceptanceTestRom.Length; i++)
            {
                var n = ldAcceptanceTestRom[i];
                FakeMmu.SetByte((ushort) i, n);
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

            var previousInstruction = 0x00;
            var previousInstructionCount = 0;
            for (var i = 0; i < 100000000 && previousInstructionCount < 1000; i++)
            {
                var instruction = FakeMmu.GetByte(Sut.ProgramCounter);
                Sut.Execute(instruction);

                if ((FakeMmu.Memory[0xFF02] & 0x80) == 0x80)
                {
                    FakeMmu.Memory[0xFF02] = 0;
                }

                if (instruction == previousInstruction)
                {
                    previousInstructionCount++;
                }
                else
                {
                    previousInstruction = instruction;
                    previousInstructionCount = 0;
                }
            }

            var output = FakeMmu.Output.Aggregate("", (x, y) => x + y);
            Console.WriteLine(output);
            Assert.Contains("passed", output.ToLower());
        }
    }
}
