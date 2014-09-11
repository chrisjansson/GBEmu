using Xunit;

namespace Test.TimerA
{
    public class Timer_4096Tests
    {
        private Core.Timer _timer;
        private FakeMmu _fakeMmu;

        public Timer_4096Tests()
        {
            _fakeMmu = new FakeMmu();
            _timer = new Core.Timer(_fakeMmu);
        }

        private const int TicksPerTimerCycle = 256;

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
    }
}