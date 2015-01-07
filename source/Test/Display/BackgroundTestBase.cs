using Xunit;

namespace Test.Display
{
    public abstract class BackgroundTestBase : DataTransferTestBase
    {

        [Fact]
        public void Copies_pixels_from_first_and_second_tile_on_first_row()
        {
            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            AssertLine(line,
                FirstTileFirstRow,
                SecondTileFirstRow);
        }

        [Fact]
        public void Copies_pixels_from_second_tile_on_8th_row()
        {
            _sut.FinishFrame();
            _sut.TransferScanLine(8);

            var line = GetLine(8);
            AssertLine(line, SecondTileFirstRow);
        }

        [Fact]
        public void Scrolls_x_and_wraps_around_tile_map()
        {
            _fakeMmu.ScrollX(250);

            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            AssertLine(line, 3, 3, 3, 3, 0, 0, 0, 3);
        }

        [Fact]
        public void Scrolls_y_and_wraps_around_tile_map()
        {
            _fakeMmu.ScrollY(254);

            _sut.FinishFrame();
            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);
            _sut.TransferScanLine(2);

            AssertLine(GetLine(0), 0, 0, 3, 3, 3, 3, 0, 0); //2nd last line of tile 1, tile map block 992
            AssertLine(GetLine(1), 0, 0, 0, 0, 0, 0, 0, 0); //last line of tile 1, tile map block 992
            AssertLine(GetLine(2), 0, 3, 3, 3, 3, 3, 0, 0); //wrapped around, 1st line of tile 0, tile map block 0
        }
    }
}