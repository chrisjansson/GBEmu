using Core;
using Xunit;

namespace Test.Display
{
    public class OAM_interrupt_tests
    {
        private readonly FakeMmu _fakeMmu;
        private readonly Core.Display _sut;

        public OAM_interrupt_tests()
        {
            _fakeMmu = new FakeMmu();
            _sut = new Core.Display(_fakeMmu, new FakeDisplayRenderer());
        }

        [Fact]
        public void Does_not_raise_interrupt_before_oam()
        {
            _sut.STAT = 0x20;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;

            _sut.Tick(20);
            _sut.Tick(43);
            _sut.Tick(50);

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Does_not_raise_interrupt_when_oam_interrupt_is_disabled()
        {
            _sut.STAT = 0;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;

            _sut.Tick(20);
            _sut.Tick(43);
            _sut.Tick(51);

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Raises_interrupt_when_entering_oam()
        {
            _sut.STAT = 0x20;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x18;

            _sut.Tick(20);
            _sut.Tick(43);
            _sut.Tick(51);

            Assert.Equal(0x1A, _fakeMmu.Memory[RegisterAddresses.IF]);
        }
    }
}