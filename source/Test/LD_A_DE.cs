using Core;
using Xunit;

namespace Test
{
    public class LD_A_DE
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public LD_A_DE()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void Loads_memory_at_DE_into_A()
        {
            _cpu.ProgramCounter = 9213;
            _cpu.Cycles = 84712;
            RegisterPair.DE.Set(_cpu, 0xAB, 0xFA);
            _fakeMmu.SetByte(0xABFA, 0x32);

            _cpu.Execute(0x1A);

            Assert.Equal(0x32, _cpu.A);
            Assert.Equal(9214, _cpu.ProgramCounter);
            Assert.Equal(84714, _cpu.Cycles);
        }
    }
}