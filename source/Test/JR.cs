using Core;
using Xunit;

namespace Test
{
    public class JR : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void FactMethodName()
        {
            _cpu.ProgramCounter = 9281;
            _cpu.Cycles = 1819;
            _fakeMmu.SetByte(9282, 0x92);

            _cpu.Execute(0x18);

            Assert.Equal(9173, _cpu.ProgramCounter);
            Assert.Equal(1822, _cpu.Cycles);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}