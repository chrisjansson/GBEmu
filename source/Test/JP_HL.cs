using Xunit;

namespace Test
{
    public class JP_HL: CpuTestBase
    {
        [Fact]
        public void Advances_clock()
        {
            Execute(0xE9);

            AdvancedClock(1);
        }

        [Fact]
        public void Sets_program_counter_to_contents_of_hl()
        {
            Cpu.ProgramCounter = 0x1000;
            Cpu.H = 0x48;
            Cpu.L = 0x01;

            Execute(0xE9);

            Assert.Equal(0x4801, Cpu.ProgramCounter);
        }
    }
}