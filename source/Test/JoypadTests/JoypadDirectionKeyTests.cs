using Core;
using Xunit;
using Xunit.Extensions;

namespace Test.JoypadTests
{
    public class JoypadDirectionKeyTests
    {
        private readonly Joypad _sut;

        public JoypadDirectionKeyTests()
        {
            _sut = new Joypad();
            _sut.P1 = 0x20; //Select direction keys
        }

        [Theory]
        [InlineData(false, (byte)0x1F)]
        [InlineData(true, (byte)0x1E)]
        public void Bit_0_is_right(bool isPressed, byte expectedP1)
        {
            _sut.Right = isPressed;

            Assert.Equal(expectedP1, _sut.P1);
        }

        [Theory]
        [InlineData(false, (byte)0x1F)]
        [InlineData(true, (byte)0x1D)]
        public void Bit_1_is_left(bool isPressed, byte expectedP1)
        {
            _sut.Left = isPressed;

            Assert.Equal(expectedP1, _sut.P1);
        }

        [Theory]
        [InlineData(false, (byte)0x1F)]
        [InlineData(true, (byte)0x1B)]
        public void Bit_2_is_up(bool isPressed, byte expectedP1)
        {
            _sut.Up = isPressed;

            Assert.Equal(expectedP1, _sut.P1);
        }

        [Theory]
        [InlineData(false, (byte)0x1F)]
        [InlineData(true, (byte)0x17)]
        public void Bit_3_is_up(bool isPressed, byte expectedP1)
        {
            _sut.Down = isPressed;

            Assert.Equal(expectedP1, _sut.P1);
        }

        [Fact]
        public void Combines_buttons_into_P1()
        {
            _sut.Left = true;
            _sut.Up = true;

            Assert.Equal(0x19, _sut.P1);
        }
    }
}