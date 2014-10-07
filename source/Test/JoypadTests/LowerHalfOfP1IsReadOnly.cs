using Core;
using Ploeh.AutoFixture.Xunit;
using Xunit;

namespace Test.JoypadTests
{
    public class LowerHalfOfP1IsReadOnly
    {
        private readonly Joypad _sut;

        public LowerHalfOfP1IsReadOnly()
        {
            _sut = new Joypad();
        }

        [Fact]
        public void Lower_half_of_JOYP_is_read_only()
        {
            _sut.P1 = 0xFF;

            Assert.Equal(_sut.P1, 0xF0);
        }
    }
}