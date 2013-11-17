using Core;
using Xunit;

namespace Test
{
    public class XOR_HL
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public XOR_HL()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Fact]
        public void XORs_A_and_memory_at_HL()
        {
            _cpu.A = 0x96;
            _cpu.Z = 1;
            _cpu.ProgramCounter = 931;
            _cpu.Cycles = 9123;
            RegisterPair.HL.Set(_cpu, 0xBC, 0x04);
            _fakeMmu.SetByte(0xBC04, 0x5D);

            _cpu.Execute(0xAE);

            Assert.Equal(0xCB, _cpu.A);
            Assert.Equal(0, _cpu.Z);
            Assert.Equal(932, _cpu.ProgramCounter);
            Assert.Equal(9125, _cpu.Cycles);
        }

        [Fact]
        public void Resets_n_c_h()
        {
            _cpu.N = 1;
            _cpu.HC = 1;
            _cpu.Carry = 1;

            _cpu.Execute(0xAE);

            Assert.Equal(0, _cpu.N);
            Assert.Equal(0, _cpu.HC);
            Assert.Equal(0, _cpu.Carry);
        }

        [Fact]
        public void Sets_z_if_result_is_zero()
        {
            _cpu.Z = 0;

            _cpu.Execute(0xAE);

            Assert.Equal(1, _cpu.Z);
        }
    }
}