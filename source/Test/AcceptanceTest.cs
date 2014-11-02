using System.IO;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class AcceptanceTest
    {
        protected Cpu Sut;
        protected MMuSpy Mmu;
        protected Timer Timer;
        private long _previousCycleCount;
        private ushort _previousProgramCounter;

        public AcceptanceTest()
        {
            var mmu = new Core.MMU
            {
                Display = new NullDisplay()
            };
            Mmu = new MMuSpy(mmu);
            Sut = new Cpu(Mmu);
            Timer = new Timer(Mmu);
            mmu.Cpu = Sut;
            mmu.Timer = Timer;
            mmu.Joypad = new Joypad();
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
            LoadTestRomIntoMmu(rom);

            Run();

            AssertTestRomPassed();
        }

        private void Run()
        {
            var programCounterRepeatCount = 0;
            for (var i = 0; i < 100000000 && programCounterRepeatCount < 10000; i++)
            {
                var nextInstruction = Mmu.GetByte(Sut.ProgramCounter);
                Sut.Execute(nextInstruction);
                RunOtherComponents();
                SendSerialData();

                if (Sut.ProgramCounter == _previousProgramCounter)
                {
                    programCounterRepeatCount++;
                }
                else
                {
                    _previousProgramCounter = Sut.ProgramCounter;
                    programCounterRepeatCount = 0;
                }
                _previousCycleCount = Sut.Cycles;
            }
        }

        private void RunOtherComponents()
        {
            for (var j = 0; j < Sut.Cycles - _previousCycleCount; j++)
            {
                Timer.Tick();
            }
        }

        private void SendSerialData()
        {
            if ((Mmu.GetByte(0xFF02) & 0x80) == 0x80)
            {
                Mmu.SetByte(0xFF02, 0);
            }
        }

        private void AssertTestRomPassed()
        {
            var output = Mmu.Output.Aggregate("", (x, y) => x + y);
            Assert.Contains("passed", output.ToLower());
        }

        private void LoadTestRomIntoMmu(string rom)
        {
            var ldAcceptanceTestRom = File.ReadAllBytes(rom);

            for (var i = 0; i < ldAcceptanceTestRom.Length; i++)
            {
                var n = ldAcceptanceTestRom[i];
                Mmu.SetByte((ushort)i, n);
            }
        }
    }
}
