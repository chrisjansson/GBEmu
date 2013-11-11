using Core;
using Xunit;

namespace Test
{
    public class POP_qq : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Pushes_HL_to_the_stack()
        {
            _cpu.ProgramCounter = 9381;
            _cpu.Cycles = 2912349;
            _cpu.SP = 33812;
            _fakeMmu.SetByte(33812, 0xBC);
            _fakeMmu.SetByte(33813, 0xAE);

            _cpu.Execute(0xE1);

            Assert.Equal(0xAE, _cpu.H);
            Assert.Equal(0xBC, _cpu.L);
            Assert.Equal(9382, _cpu.ProgramCounter);
            Assert.Equal(2912352, _cpu.Cycles);
        }

        [Fact]
        public void Increments_sp()
        {
            _cpu.SP = 33812;

            _cpu.Execute(0xE1);

            Assert.Equal(33814, _cpu.SP);
        }

        private byte CreateOpCode(RegisterPair registerPair)
        {
            return (byte) (0xC5 << 4 | registerPair);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        } 
    }
}