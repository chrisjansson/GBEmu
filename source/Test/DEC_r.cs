using Core;
using Xunit;

namespace Test
{
    public class DEC_r : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;

        [Fact]
        public void DEC_c()
        {
            _cpu.C = 0x05;
            _cpu.ProgramCounter = 18349;
            _cpu.Cycles = 934821;

            _cpu.Execute(0x0D);

            Assert.Equal(0x04, _cpu.C);
            Assert.Equal(18350, _cpu.ProgramCounter);
            Assert.Equal(934822, _cpu.Cycles);
        }

        [Fact]
        public void Sets_hc_when_borrow_from_bit_4()
        {
            _cpu.C = 0xA0;
            _cpu.HC = 0;

            _cpu.Execute(0x0D);

            Assert.Equal(1, _cpu.HC);
        }

        [Fact]
        public void Resets_hc_when_not_borrow_from_bit_4()
        {
            _cpu.C = 0x01;
            _cpu.HC = 1;

            _cpu.Execute(0x0D);

            Assert.Equal(0, _cpu.HC);
        }

        [Fact]
        public void Sets_N_when_dec()
        {
            _cpu.N = 0;

            _cpu.Execute(0x0D);

            Assert.Equal(1, _cpu.N);
        }

        [Fact]
        public void Unsets_z_when_result_is_non_zero()
        {
            _cpu.Z  = 1;
            _cpu.C = 0x02;

            _cpu.Execute(0x0D);

            Assert.Equal(0, _cpu.Z);
        }

        [Fact]
        public void Sets_z_when_result_is_zero()
        {
            _cpu.Z = 0;
            _cpu.C = 0x01;

            _cpu.Execute(0x0D);

            Assert.Equal(1, _cpu.Z);
        }
        
        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
        }
    }
}