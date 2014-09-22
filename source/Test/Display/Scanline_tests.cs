using Xunit;

namespace Test.Display
{
    public class Scanline_tests
    {
        private readonly FakeDisplayDataTransferService _fakeDisplayDataTransferService;
        private readonly Core.Display _sut;

        public Scanline_tests()
        {
            _fakeDisplayDataTransferService = new FakeDisplayDataTransferService();
            _sut = new Core.Display(new FakeMmu(), _fakeDisplayDataTransferService);
        }

        [Fact]
        public void Starts_at_line_0()
        {
            Assert.Equal(0, _sut.Line);
        }

        [Fact]
        public void Increments_line_after_completing_a_scan_line()
        {
            _sut.AdvanceScanLine();

            Assert.Equal(1, _sut.Line);
        }

        [Fact]
        public void Increments_line_to_153()
        {
            for (var i = 0; i < 153; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(153, _sut.Line);
        }

        [Fact]
        public void Resets_line_count_to_zero_after_153()
        {
            for (var i = 0; i < 154; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(0, _sut.Line);
        }

        [Fact]
        public void Transfers_a_scan_line_after_a_scan_line_cycle()
        {
            _sut.AdvanceScanLine();

            Assert.Equal(1, _fakeDisplayDataTransferService.TransferedScanLines.Count);
        }

        [Fact]
        public void Transfers_a_scan_line_for_every_scan_line_cycle()
        {
            _sut.AdvanceScanLine();
            _sut.AdvanceScanLine();

            Assert.Equal(2, _fakeDisplayDataTransferService.TransferedScanLines.Count);
        }

        [Fact]
        public void Transfers_the_current_scan_line()
        {
            _sut.AdvanceScanLine();
            _sut.AdvanceScanLine();

            Assert.Equal(1, _fakeDisplayDataTransferService.LastTransferedScanLine);
        }

        [Fact]
        public void Transfers_current_line_when_transitioning_from_mode_3_to_0()
        {
            _sut.AdvanceScanLine();
            _sut.AdvanceScanLine();
            _sut.AdvanceScanLine();
            _sut.AdvanceScanLine();
            //Advance to the last tick of mode 3 of line 4
            for (var i = 0; i < 62; i++)
            {
                _sut.Tick();
            }
            Assert.Equal(3, _sut.Mode);

            _sut.Tick();

            Assert.Equal(4, _fakeDisplayDataTransferService.LastTransferedScanLine);
        }

        [Fact]
        public void Finishes_a_frame_every_154_lines()
        {
            for (int i = 0; i < 5; i++)
            {
                _sut.AdvanceFrame();
            }

            Assert.Equal(5, _fakeDisplayDataTransferService.FinishedFrames);
        }

        [Fact]
        public void Finishes_frame_at_the_end_of_scan_line_144()
        {
            for (int i = 0; i < 144; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(1, _fakeDisplayDataTransferService.FinishedFrames);
        }
    }
}