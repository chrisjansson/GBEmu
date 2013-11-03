using Core;
using Xunit;

namespace Test
{
    public class JP_nn : IUseFixture<CpuFixture>
    {
        private Cpu _sut;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Loads_nn_into_program_counter_lower_order_byte_first()
        {
            _sut.ProgramCounter = 9876;
            const byte l = 123;
            const byte h = 234;
            _fakeMmu.SetByte(9877, l);
            _fakeMmu.SetByte(9878, h);

            _sut.Execute(0xC3);

            Assert.Equal(60027, _sut.ProgramCounter);
        }

        [Fact]
        public void Advances_clock()
        {
            _sut.Cycles = 7931;

            _sut.Execute(0xC3);

            Assert.Equal(_sut.Cycles, 7935);
        }

        public void SetFixture(CpuFixture fixture)
        {
            _sut = fixture.Cpu;
            _fakeMmu = fixture.FakeMmu;
        }
    }
}