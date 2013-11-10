using Core;
using Xunit;

namespace Test
{
    public class RET : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Loads_program_counter_with_memory_pointed_to_by_SP()
        {
            _cpu.Cycles = 12384;
            _cpu.SP = 9481;
            _fakeMmu.Memory[9481] = 0x03;
            _fakeMmu.Memory[9482] = 0x80;

            _cpu.Execute(0xC9);

            Assert.Equal(0x8003, _cpu.ProgramCounter);
            Assert.Equal(12388, _cpu.Cycles);
        }

        [Fact]
        public void Increases_sp()
        {
            _cpu.SP = 9481;

            _cpu.Execute(0xC9);

            Assert.Equal(9483, _cpu.SP);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}