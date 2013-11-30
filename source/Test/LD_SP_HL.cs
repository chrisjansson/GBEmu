using Xunit;

namespace Test
{
    public class LD_SP_HL : CpuTestBase
    {
        [Fact]
        public void Advances_counters()
        {
            Execute(0xF9);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Fact]
        public void Loads_hl_into_sp()
        {
            Cpu.H = 0x44;
            Cpu.L = 0x2E;
            Cpu.SP = 0;

            Execute(0xF9);

            Assert.Equal(0x442E, Cpu.SP);
        }
    }
}