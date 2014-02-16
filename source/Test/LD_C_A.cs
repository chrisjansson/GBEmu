using Xunit;

namespace Test
{
    public class LD_C_A : CpuTestBase
    {
        [Fact]
        public void Advances_clocks()
        {
            Execute(CreateOpcode());

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Sets_memory_at_FF00_plus_C_to_the_contents_of_A()
        {
            Cpu.A = 0xAB;
            Cpu.C = 0x9F;

            Execute(CreateOpcode());

            Assert.Equal(0xAB, FakeMmu.Memory[0xFF9F]);
        }

        private byte CreateOpcode()
        {
            return 0xE2;
        }
    }
}