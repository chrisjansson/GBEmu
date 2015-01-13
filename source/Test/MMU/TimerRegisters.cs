using Core;
using Ploeh.AutoFixture.Xunit;
using Test.TimerTests;
using Xunit;
using Xunit.Extensions;

namespace Test.MMU
{
    public class TimerRegisters
    {
        private readonly Core.MMU _sut;
        private readonly Timer _timer;

        public TimerRegisters()
        {
            _sut = new Core.MMU(null);
            _timer = new Timer(_sut);
            _sut.Timer = _timer;
        }

        [Theory, AutoData]
        public void TIMA_is_read_from_timer_through_0xFF05(byte expectedValue)
        {
            _timer.TIMA = expectedValue;

            var actual = _sut.GetByte(0xFF05);

            Assert.Equal(expectedValue, actual);
        }

        [Theory, AutoData]
        public void TIMA_is_set_on_timer_through_0xFF05(byte expectedValue)
        {
            _sut.SetByte(0xFF05, expectedValue);

            Assert.Equal(expectedValue, _timer.TIMA);
        }


        [Theory, AutoData]
        public void TMA_is_set_on_timer_through_0xFF06(byte expectedValue)
        {
            _sut.SetByte(0xFF06, expectedValue);

            Assert.Equal(expectedValue, _timer.TMA);
        }


        [Theory, AutoData]
        public void TMA_is_read_from_timer_through_0xFF06(byte expectedValue)
        {
            _timer.TMA = expectedValue;

            var actual = _sut.GetByte(0xFF06);

            Assert.Equal(expectedValue, actual);
        }


        [Theory, AutoData]
        public void TAC_is_set_on_timer_through_0xFF07(byte expectedValue)
        {
            _sut.SetByte(0xFF07, expectedValue);

            Assert.Equal(expectedValue, _timer.TAC);
        }


        [Theory, AutoData]
        public void TAC_is_read_from_timer_through_0xFF07(byte expectedValue)
        {
            _timer.TAC = expectedValue;

            var actual = _sut.GetByte(0xFF07);

            Assert.Equal(expectedValue, actual);
        }

        [Fact]
        public void DIV_is_read_from_timer_through_0xFF04()
        {
            _timer.Tick(1233);

            var actual = _sut.GetByte(0xFF04);

            Assert.Equal(_timer.DIV, actual);
        }

        [Fact]
        public void DIV_is_written_to_timer_through_0xFF04()
        {
            _timer.Tick(3242);

            _sut.SetByte(0xFF04, 43);

            Assert.Equal(0, _timer.DIV);
        }
    }
}