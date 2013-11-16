using Core;
using Xunit;

namespace Test
{
    public class LD_A_nn
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public LD_A_nn()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void Loads_memory_at_nn_into_a()
        {
            _cpu.ProgramCounter = 9182;
            _cpu.Cycles = 1234;
            _fakeMmu.SetByte(9183, 0x44);
            _fakeMmu.SetByte(9184, 0xFF);
            _fakeMmu.SetByte(0xFF44, 0xAB);

            _cpu.Execute(0xFA);

            Assert.Equal(0xAB, _cpu.A);
            Assert.Equal(9185, _cpu.ProgramCounter);
            Assert.Equal(1238, _cpu.Cycles);
        }
    }
}