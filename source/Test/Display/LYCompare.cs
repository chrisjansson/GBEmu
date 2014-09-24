using Core;
using Xunit;

namespace Test.Display
{
    public class LYCompare : CpuTestBase
    {
        private readonly Core.Display _sut;
        private FakeMmu _fakeMmu;

        public LYCompare()
        {
            _fakeMmu = new FakeMmu();
            _sut = new Core.Display(_fakeMmu, new FakeDisplayDataTransferService());
        }

        [Fact]
        public void Coincidence_flag_is_zero_when_LYC_and_LY_are_different()
        {
            _sut.LYC = 100;
            for (var i = 0; i < 99; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.False(CoincidenceFlag);
        }

        [Fact]
        public void Has_not_raised_LYC_interrupt_before_LY_and_LYC_are_same()
        {
            _sut.LCDC |= 0x40;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0xFD;
            _sut.LYC = 102;
            for (var i = 0; i < 100; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(0xFD, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Coincidence_flag_is_one_when_LYC_and_LY_are_same()
        {
            _sut.LYC = 123;
            for (var i = 0; i < 123; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.True(CoincidenceFlag);
        }

        [Fact]
        public void Raises_coincidence_interrupt_when_LYC_and_LY_are_same()
        {
            _sut.LCDC |= 0x40;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x18;
            _sut.LYC = 123;
            for (var i = 0; i < 123; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(0x1A, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Raises_coincidence_interrupt_for_vertical_blanking_lines()
        {
            _sut.LCDC |= 0x40;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x18;
            _sut.LYC = 150;
            for (var i = 0; i < 152; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(0x1B, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        [Fact]
        public void Does_not_raise_interrupt_when_coincidence_interrupt_is_disabled()
        {
            _sut.LCDC = 0;
            _fakeMmu.Memory[RegisterAddresses.IF] = 0x18;
            _sut.LYC = 150;
            for (var i = 0; i < 152; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(0x19, _fakeMmu.Memory[RegisterAddresses.IF]);
        }

        private bool CoincidenceFlag
        {
            get { return ((_sut.LCDC >> 2) & 0x1) == 0x1; }
        }
    }
}