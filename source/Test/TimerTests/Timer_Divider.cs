using Core;
using Xunit;

namespace Test.TimerTests
{
    public class Timer_Divider
    {
        private readonly Timer _sut;

        public Timer_Divider()
        {
            _sut = new Timer(new FakeMmu());
        }

        [Fact]
        public void Increments_divider_after_64_cycles()
        {
            _sut.Tick(64);

            Assert.Equal(1, _sut.DIV);
        }

        [Fact]
        public void Has_not_incremented_after_64_cycles()
        {
            _sut.Tick(63);

            Assert.Equal(0, _sut.DIV);
        }

        [Fact]
        public void Writing_to_div_resets_it()
        {
            _sut.Tick(128);

            _sut.DIV = 30;

            Assert.Equal(0, _sut.DIV);
        }
    }
}