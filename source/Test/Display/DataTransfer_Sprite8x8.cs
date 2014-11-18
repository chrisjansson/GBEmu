using Core;
using Xunit;

namespace Test.Display
{
    public class DataTransfer_Sprite8x8 : DataTransferTestBase
    {
        public DataTransfer_Sprite8x8()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] = 0x10;
            for (int i = 0; i < 40; i++)
            {
                InsertSpriteAttribute(i, new byte[]
                {
                    0x00,
                    0x00,
                    0x00,
                    0x00
                });
            }
        }

        [Fact]
        public void Draws_sprite_at_origin()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x10, //y = 16, displaycoordinate + 16
                0x08, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });
            _sut.FinishFrame();
            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);
            _sut.TransferScanLine(7);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, FirstTileFirstRow);
            AssertLine(second, 2, 2, 0, 0, 0, 2, 2, 0);
        }

        [Fact]
        public void Draws_sprite_offet_by_y()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x11, //y = 17, displaycoordinate + 16
                0x08, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });
            _sut.FinishFrame();
            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            AssertLine(second, FirstTileFirstRow);
        }

        [Fact]
        public void Draws_sprite_offet_by_x()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x10, //y = 17, displaycoordinate + 16
                0x09, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });
            _sut.FinishFrame();
            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, 0, 0, 3, 3, 3, 3, 3, 0, 0);
            AssertLine(second, 0, 2, 2, 0, 0, 0, 2, 2, 0);
        }

        private void InsertSpriteAttribute(int number, byte[] o)
        {
            const int spriteAttributeStartAddress = 0xFE00;
            for (var i = 0; i < o.Length; i++)
            {
                _fakeMmu.Memory[spriteAttributeStartAddress + number * 4 + i] = o[i];
            }
        }

        protected override void InsertTile(byte tileNumber, byte[] tileData)
        {
            InsertTileAt((ushort)(0x8000 + tileNumber * 16), tileData);
        }

        protected override void SetBlockTile(int block, byte tile)
        {
        }
    }
}