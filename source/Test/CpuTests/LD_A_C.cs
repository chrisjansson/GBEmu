using Xunit;

namespace Test.CpuTests
{
    public class LD_A_C : CpuTestBase
    {
        private const byte OpCode = 0xF2;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Fact]
        public void Loads_FF00_plus_C_into_a()
        {
            FakeMmu.Memory[0xFFAC] = 0xCE;
            Cpu.C = 0xAC;

            Execute(OpCode);

            Assert.Equal(0xCE, Cpu.A);
        }
    }
}