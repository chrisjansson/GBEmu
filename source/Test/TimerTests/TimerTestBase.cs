using Core;
using Xunit;

namespace Test.TimerTests
{
    public abstract class TimerTestBase
    {
        private readonly Timer _timer;
        private readonly FakeMmu _fakeMmu;

        protected TimerTestBase()
        {
            _fakeMmu = new FakeMmu();
            _timer = new Timer(_fakeMmu)
            {
                TAC = (byte) (0x04 | TimerSelect)
            };
        }

        protected abstract byte TimerSelect { get; }
        protected abstract int TicksPerTimerCycle { get; }

        [Fact]
        public void Increments_TIMA_after_one_timer_cycle()
        {
            _timer.Tick(TicksPerTimerCycle);

            Assert.Equal(1, _timer.TIMA);
        }

        [Fact]
        public void Does_not_increment_TIMA_before_a_cyle_has_elapsed()
        {
            _timer.Tick(TicksPerTimerCycle - 1);

            Assert.Equal(0, _timer.TIMA);
        }

        [Fact]
        public void Increments_TIMA_once_every_timer_cycle()
        {
            _timer.Tick(TicksPerTimerCycle * 3);

            Assert.Equal(3, _timer.TIMA);
        }

        [Fact]
        public void Loads_TMA_into_TIMA_when_TIMA_overflows()
        {
            _timer.TMA = 123;

            _timer.Tick(TicksPerTimerCycle * 256);

            Assert.Equal(123, _timer.TIMA);
        }

        [Fact]
        public void Request_interrupt_when_TIMA_overflows()
        {
            _timer.TIMA = 200;

            _timer.Tick(TicksPerTimerCycle * 56);

            Assert.Equal(0x04, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Masks_interrupt_request()
        {
            _timer.TIMA = 123;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x0B;

            _timer.Tick(TicksPerTimerCycle * 133);

            Assert.Equal(0x0F, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Does_not_increment_TIMA_when_disabled_in_TAC()
        {
            _timer.TAC = 0;

            _timer.Tick(TicksPerTimerCycle * 3);

            Assert.Equal(0, _timer.TIMA);
        }
    }
}