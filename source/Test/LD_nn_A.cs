using Core;
using Xunit;

namespace Test
{
    public class LD_nn_A : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Loads_a_into_nn()
        {
            _cpu.A = 0xAB;
            _cpu.ProgramCounter = 10189;
            _cpu.Cycles = 92181;
            _fakeMmu.SetByte(10190, 0xAC);
            _fakeMmu.SetByte(10191, 0x18);

            _cpu.Execute(0xEA);

            Assert.Equal(0xAB, _fakeMmu.GetByte(0x18AC));
            Assert.Equal(10192, _cpu.ProgramCounter);
            Assert.Equal(92185, _cpu.Cycles);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}