using System;
using Core;
using Xunit;

namespace Test.Display
{
    public class DataTransfer_tests
    {
        private readonly DisplayDataTransferService _sut;
        private readonly FakeMmu _fakeMmu;

        public DataTransfer_tests()
        {
            _fakeMmu = new FakeMmu();
            _sut = new DisplayDataTransferService(_fakeMmu);

            InsertTileAt(0x8000, new byte[]
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

            InsertTileAt(0x8010, new byte[]
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

            _fakeMmu.Memory[0x9800] = 0;
            _fakeMmu.Memory[0x9801] = 1;
            _fakeMmu.Memory[0x981F] = 1;
            _fakeMmu.Memory[0x9820] = 1;
            _fakeMmu.Memory[0x9BE0] = 1;
        }

        [Fact]
        public void Copies_pixels_from_first_and_second_tile_on_first_row()
        {
            _sut.TransferScanLine(0);

            var line = GetLine(0);
            AssertLine(line,
                0, 3, 3, 3, 3, 3, 0, 0, //First tile row 0
                0, 0, 3, 3, 3, 3, 0, 0); //Second tile row 0
        }

        [Fact]
        public void Copies_pixels_from_first_tile_on_8th_row()
        {
            _sut.TransferScanLine(8);

            var line = GetLine(8);
            AssertLine(line, 0, 0, 3, 3, 3, 3, 0, 0);
        }

        [Fact]
        public void Scrolls_x_and_wraps_around_tile_map()
        {
            _fakeMmu.ScrollX(250);

            _sut.TransferScanLine(0);
            
            var line = GetLine(0);
            AssertLine(line, 3, 3, 3, 3, 0, 0, 0, 3);
        }

        [Fact]
        public void Scrolls_y_and_wraps_around_tile_map()
        {
            _fakeMmu.ScrollY(254);

            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);
            _sut.TransferScanLine(2);

            AssertLine(GetLine(0), 0, 0, 3, 3, 3, 3, 0, 0); //2nd last line of tile 1, tile map block 992
            AssertLine(GetLine(1), 0, 0, 0, 0, 0, 0, 0, 0); //last line of tile 1, tile map block 992
            AssertLine(GetLine(2), 0, 3, 3, 3, 3, 3, 0, 0); //wrapped around, 1st line of tile 0, tile map block 0
        }

        private void AssertLine(byte[] line, params byte[] colors)
        {
            for (var i = 0; i < colors.Length; i++)
            {
                Assert.Equal(colors[i], line[i]);
            }
        }

        private byte[] GetLine(int i)
        {
            var line = new Byte[160];
            for (var x = 0; x < 160; x++)
            {
                line[x] = _sut.FrameBuffer[i * 160 + x];
            }

            return line;
        }

        private void InsertTileAt(ushort address, byte[] tile)
        {
            Assert.Equal(16, tile.Length);
            for (int i = 0; i < tile.Length; i++)
            {
                _fakeMmu.Memory[address + i] = tile[i];
            }
        }
    }

    public static class MMUExtensions
    {
        public static void ScrollX(this IMmu mmu, byte scrollY)
        {
            mmu.SetByte(RegisterAddresses.ScrollX, scrollY);
        }

        public static void ScrollY(this IMmu mmu, byte scrollY)
        {
            mmu.SetByte(RegisterAddresses.ScrollY, scrollY);
        }
    }
}