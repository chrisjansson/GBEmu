using Core;
using Xunit;

namespace Test
{
    public class PUSH_qq : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Pushes_HL_to_the_stack()
        {
            _cpu.ProgramCounter = 2911;
            _cpu.Cycles = 2912349;
            _cpu.SP = 33812;
            _cpu.H = 0xAE;
            _cpu.L = 0xBC;

            _cpu.Execute(0xE5);

            Assert.Equal(0xAE, _fakeMmu.GetByte(33811));
            Assert.Equal(0xBC, _fakeMmu.GetByte(33810));
            Assert.Equal(2912, _cpu.ProgramCounter);
            Assert.Equal(2912353, _cpu.Cycles);
        }

        [Fact]
        public void Decrements_sp()
        {
            _cpu.SP = 33812;

            _cpu.Execute(0xE5);

            Assert.Equal(33810, _cpu.SP);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}