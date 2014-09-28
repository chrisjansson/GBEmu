using Core;
using Xunit;

namespace Test.Display
{
    public class HBlank_interrupt_tests
    {
        private readonly Core.Display _sut;
        private readonly FakeMmu _fakeMmu;

        public HBlank_interrupt_tests()
        {
            _fakeMmu = new FakeMmu();
            _sut = new Core.Display(_fakeMmu, new FakeDisplayDataTransferService());
        }

        [Fact]
        public void Does_not_raise_interrupt_before_h_blank()
        {
            _sut.LCDC = 0x08;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;

            _sut.Tick(20);
            _sut.Tick(42);

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Does_not_raise_interrupt_when_h_blank_interrupt_is_disabled()
        {
            _sut.LCDC = 0;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;

            _sut.Tick(20);
            _sut.Tick(43);

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Raises_interrupt_when_entering_h_blank()
        {
            _sut.LCDC = 0x08;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x18;

            _sut.Tick(20);
            _sut.Tick(43);

            Assert.Equal(0x1A, _fakeMmu.Memory[RegisterAddresses.IF]);
        }
    }

    public class VBlank_lcdstat_interrupt_tests
    {
        private readonly Core.Display _sut;
        private readonly FakeMmu _fakeMmu;

        public VBlank_lcdstat_interrupt_tests()
        {
            _fakeMmu = new FakeMmu();
            _sut = new Core.Display(_fakeMmu, new FakeDisplayDataTransferService());
        }

        [Fact]
        public void Does_not_raise_interrupt_before_v_blank()
        {
            _sut.LCDC = 0x10;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;

            _sut.AdvanceToScanLine(143);
            _sut.Tick(20);
            _sut.Tick(43);
            _sut.Tick(50);

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Does_not_raise_interrupt_when_v_blank_interrupt_is_disabled()
        {
            _sut.LCDC = 0;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;

            _sut.AdvanceToScanLine(143);
            _sut.Tick(20);
            _sut.Tick(43);
            _sut.Tick(51);

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Raises_interrupt_when_entering_v_blank()
        {
            _sut.LCDC = 0x10;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x18;

            _sut.AdvanceToScanLine(143);
            _sut.Tick(20);
            _sut.Tick(43);
            _sut.Tick(51);

            Assert.Equal(0x1B, _fakeMmu.Memory[RegisterAddresses.IF]);
        }
    }
}