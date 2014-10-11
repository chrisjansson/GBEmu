using Xunit;

namespace Test.CpuTests
{
    public class EI : CpuTestBase
    {
        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Sets_IME()
        {
            Cpu.IME = false;

            Execute(OpCode);

            Assert.True(Cpu.IME);
        }

        private const byte OpCode = 0xFB;
    }
}