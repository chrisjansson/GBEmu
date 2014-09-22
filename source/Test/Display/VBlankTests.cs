using Core;
using Xunit;

namespace Test.Display
{
    public class VBlankTests
    {
        private const byte Vblank = 0x01;
        private readonly Core.Display _sut;
        private readonly FakeMmu _mmu;

        public VBlankTests()
        {
            _mmu = new FakeMmu();
            _sut = new Core.Display(_mmu, new FakeDisplayDataTransferService());
        }

        [Fact]
        public void Does_not_request_interrupt_before_vblank_strt()
        {
            _mmu.Memory[RegisterAddresses.IF] = 0xFE;
            for (var i = 0; i < 143; i++)
            {
                _sut.AdvanceScanLine();
            }
            _sut.Tick(50);

            var actualInterruptRequest = _mmu.Memory[RegisterAddresses.IF] & Vblank;
            Assert.Equal(0x00, actualInterruptRequest);
        }

        [Fact]
        public void Requests_interrupt_at_the_start_of_vblank()
        {
            for (var i = 0; i < 144; i++)
            {
                _sut.AdvanceScanLine();
            }

            var actualInterruptRequest = _mmu.Memory[RegisterAddresses.IF] & Vblank;
            Assert.Equal(1, actualInterruptRequest);
        }
    }
}