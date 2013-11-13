using Core;
using Xunit;

namespace Test
{
    public class LD_A_n
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public LD_A_n()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void Loads_memory_at_FFnn_into_A()
        {
            _cpu.ProgramCounter = 7894;
            _cpu.Cycles = 1295;
            _fakeMmu.SetByte(7895, 0xBE);
            _fakeMmu.SetByte(0xFFBE, 0x5E);

            _cpu.Execute(0xF0);

            Assert.Equal(0x5E, _cpu.A);
            Assert.Equal(7896, _cpu.ProgramCounter);
            Assert.Equal(1298, _cpu.Cycles);
        }
    }
}