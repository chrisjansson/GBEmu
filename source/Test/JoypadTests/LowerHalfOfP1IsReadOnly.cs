using Core;
using Xunit;
using Xunit.Extensions;

namespace Test.JoypadTests
{
    public class LowerHalfOfP1IsReadOnly
    {
        private readonly Joypad _sut;

        public LowerHalfOfP1IsReadOnly()
        {
            _sut = new Joypad(new FakeMmu());
        }

        [Theory]
        [InlineData(0xC0, 0x0F)]
        [InlineData(0x00, 0x0F)]
        [InlineData(0x0F, 0x0F)]
        public void Lower_half_of_JOYP_is_read_only(int set, int expected)
        {
            _sut.P1 = (byte) set;

            Assert.Equal(_sut.P1, expected);
        }
    }
}