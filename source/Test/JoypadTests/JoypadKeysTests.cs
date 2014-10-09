using Core;
using Xunit;

namespace Test.JoypadTests
{
    public class JoypadKeysTests
    {
        private readonly Joypad _sut;

        public JoypadKeysTests()
        {
            _sut = new Joypad();
        }

        [Fact]
        public void Selecting_both_button_and_direction_keys_joins_both()
        {
            _sut.P1 = 0x00;
            _sut.A = true;
            _sut.Left = true;

            Assert.Equal(0x0C, _sut.P1);
        }
    }
}