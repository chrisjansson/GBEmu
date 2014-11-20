using System;
using Core;
using Xunit;

namespace Test.Display
{
    public abstract class DataTransferTestBase
    {
        protected FakeMmu _fakeMmu;
        protected DisplayDataTransferService _sut;

        protected byte[] FirstTileFirstRow;
        protected byte[] SecondTileFirstRow;

        protected DataTransferTestBase()
        {
            _fakeMmu = new FakeMmu();
            _sut = new DisplayDataTransferService(_fakeMmu, new SpriteRenderer(_fakeMmu));

            InsertTile(0, new byte[]
            {
                0x7C, 0x7C, 
                0x00, 0xC6, 
                0xC6, 0x00, 
                0x00, 0xFE,
                0xC6, 0xC6,
                0x00, 0xC6,
                0xC6, 0x00,
                0x00, 0x00
            });

            InsertTile(128, new byte[]
            {
                0x3C, 0x3C, 
                0x66, 0x66,
                0x6E, 0x6E,
                0x76, 0x76,
                0x66, 0x66,
                0x66, 0x66,
                0x3C, 0x3C,
                0x00, 0x00,
            });

            SetBlockTile(0, 0);
            SetBlockTile(1, 128);
            SetBlockTile(0x1F, 128);
            SetBlockTile(0x20, 128);
            SetBlockTile(0x3E0, 128);

            FirstTileFirstRow = new byte[]
            {
                0, 3, 3, 3, 3, 3, 0, 0
            };

            SecondTileFirstRow = new byte[]
            {
                0, 0, 3, 3, 3, 3, 0, 0
            };
        }

        protected abstract void InsertTile(byte tileNumber, byte[] tileData);
        protected abstract void SetBlockTile(int block, byte tile);

        protected void AssertLine(byte[] line, params byte[] colors)
        {
            for (var i = 0; i < colors.Length; i++)
            {
                Assert.Equal(colors[i], line[i]);
            }
        }

        protected void AssertLine(byte[] line, params byte[][] colors)
        {
            for (var i = 0; i < colors.Length; i++)
            {
                for (var j = 0; j < colors[i].Length; j++)
                {
                    Assert.Equal(colors[i][j], line[j + i * 8]);
                }
            }
        }

        protected byte[] GetLine(int i)
        {
            var line = new Byte[160];
            for (var x = 0; x < 160; x++)
            {
                line[x] = _sut.FrameBuffer[i * 160 + x];
            }

            return line;
        }

        protected void InsertTileAt(ushort address, byte[] tile)
        {
            Assert.Equal(16, tile.Length);
            for (int i = 0; i < tile.Length; i++)
            {
                _fakeMmu.Memory[address + i] = tile[i];
            }
        }
    }

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