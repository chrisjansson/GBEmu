using System.Linq;
using Core;
using Xunit;

namespace Test.Display
{
    public abstract class WindowDataTransferTestBase : DataTransferTestBase
    {
        protected WindowDataTransferTestBase()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] = 0x20;
            _fakeMmu.Memory[RegisterAddresses.WX] = 0x07;
        }

        [Fact]
        public void Does_not_draw_window_when_window_render_is_disabled()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] = 0x00;

            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            AssertLine(line, Enumerable.Repeat((byte)0, 16).ToArray());
        }

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
        public void Copies_pixels_from_second_row_of_first_tile_to_second_row_of_display()
        {
            _sut.FinishFrame();
            _sut.TransferScanLine(1);

            var line = GetLine(1);
            AssertLine(line, FirstTileSecondRow);
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
        public void WX_moves_window_position_horizontally()
        {
            _fakeMmu.Memory[RegisterAddresses.WX] = 8;

            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            var expectedLine = new byte[] { 0 }.Concat(FirstTileFirstRow).ToArray();
            AssertLine(line, expectedLine);
        }

        [Fact]
        public void WY_moves_window_position_vertically()
        {
            _fakeMmu.Memory[RegisterAddresses.WY] = 1;

            _sut.FinishFrame();
            _sut.TransferScanLine(1);

            var line = GetLine(1);
            AssertLine(line, FirstTileFirstRow);
        }

        [Fact]
        public void Does_not_underdraw_window_vertically() //Dont draw window on lines above it's start line
        {
            _fakeMmu.Memory[RegisterAddresses.WY] = 1;

            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            AssertLine(line, Enumerable.Repeat((byte)0, 16).ToArray());
        }

        [Fact]
        public void Do_not_overdraw_window_horizontally_into_the_next_line()
        {
            _fakeMmu.Memory[RegisterAddresses.WX] = 7 + 158;

            _sut.FinishFrame();
            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);

            var firstLine = GetLine(0);
            var secondLine = GetLine(1);
            AssertLine(firstLine, Enumerable.Repeat((byte)0, 158).Concat(FirstTileFirstRow.Take(2)).ToArray());
            AssertLine(secondLine, Enumerable.Repeat((byte)0, 16).ToArray());
        }

        [Fact]
        public void Renders_on_top_of_background()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] |= 0x01;
            _fakeMmu.Memory[RegisterAddresses.WX] = 7 + 2;

            _sut.FinishFrame();
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            var expectedLine = FirstTileFirstRow.Take(2).Concat(FirstTileFirstRow).ToArray();
            AssertLine(line, expectedLine);
        }
    }
}