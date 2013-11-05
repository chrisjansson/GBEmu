using Core;
using Xunit;

namespace Test
{
    public class LD_DE_A : IUseFixture<CpuFixture>
    {
        private Cpu _sut;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Stores_A_at_memory_DE()
        {
            _sut.ProgramCounter = 9482;
            _sut.Cycles = 82712;
            _sut.A = 123;
            _sut.D = 0x20;
            _sut.E = 0xAC;

            _sut.Execute(0x12);

            Assert.Equal(123, _fakeMmu.GetByte(0x20AC));
            Assert.Equal(9483, _sut.ProgramCounter);
            Assert.Equal(82714, _sut.Cycles);
        }

        public void SetFixture(CpuFixture data)
        {
            _sut = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}