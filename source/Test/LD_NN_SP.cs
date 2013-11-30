using Xunit;

namespace Test
{
    public class LD_NN_SP : CpuTestBase
    {
        [Fact]
        public void Advances_counters()
        {
            Execute(0x08);

            AdvancedProgramCounter(3);
            AdvancedClock(5);
        }

        [Fact]
        public void Stores_contents_of_sp_at_nn()
        {
            Cpu.SP = 0xFFF8;
            
            Execute(0x08, 0x00, 0xC1);

            Assert.Equal(0xF8, FakeMmu.GetByte(0xC100));
            Assert.Equal(0xFF, FakeMmu.GetByte(0xC101));
        }
    }
}