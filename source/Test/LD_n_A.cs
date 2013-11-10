using System.Runtime.InteropServices;
using Core;
using Xunit;

namespace Test
{
    public class LD_n_A : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Loads_a_into_memory_FFn()
        {
            _cpu.A = 0xBA;
            _cpu.ProgramCounter = 9183;
            _cpu.Cycles = 90281;
            _fakeMmu.SetByte(9184, 0x34);

            _cpu.Execute(0xE0);

            Assert.Equal(0xBA, _fakeMmu.GetByte(0xFF34));
            Assert.Equal(9185, _cpu.ProgramCounter);
            Assert.Equal(90284, _cpu.Cycles);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}