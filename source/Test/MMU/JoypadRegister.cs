using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Test.MMU
{
    public class JoypadRegister
    {
        private readonly Core.MMU _sut;
        private FakeJoypad _fakeJoypad;

        public JoypadRegister()
        {
            _fakeJoypad = new FakeJoypad();

            _sut = new Core.MMU
            {
                Joypad = _fakeJoypad
            };
        }

        [Theory, AutoData]
        public void Joypad_P1_is_read_from_joypad_through_FF00(byte p1)
        {
            _fakeJoypad.P1 = p1;

            var actual = _sut.GetByte(0xFF00);

            Assert.Equal(p1, actual);
        }

        [Theory, AutoData]
        public void Joypad_P1_is_written_to_joypad_through_FF00(byte p1)
        {
            _sut.SetByte(0xFF00, p1);

            Assert.Equal(p1, _fakeJoypad.P1);
        }
    }
}