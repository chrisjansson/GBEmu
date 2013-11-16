using Core;
using Xunit;

namespace Test
{
    public class LD_HLI_A
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public LD_HLI_A()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void Stores_contents_of_a_at_hl_and_increments_hl()
        {
            _cpu.A = 0xAC;
            _cpu.H = 0x9A;
            _cpu.L = 0x45;

            _cpu.Execute(0x22);

            Assert.Equal(0xAC, _fakeMmu.GetByte(0x9A45));
            Assert.Equal(0x9A46, _cpu.H << 8 | _cpu.L);
        }

        [Fact]
        public void Increments_program_counter_and_clock()
        {
            _cpu.ProgramCounter = 9381;
            _cpu.Cycles = 1823;

            _cpu.Execute(0x22);

            Assert.Equal(9382, _cpu.ProgramCounter);
            Assert.Equal(1825, _cpu.Cycles);
        }
    }
}