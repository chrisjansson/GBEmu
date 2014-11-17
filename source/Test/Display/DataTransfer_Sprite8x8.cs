using Xunit;

namespace Test.Display
{
    public class DataTransfer_Sprite8x8 : DataTransferTestBase
    {
        public DataTransfer_Sprite8x8()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x16, //y = 16, displaycoordinate + 16
                0x08, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });
        }

        [Fact]
        public void Draws_sprite_at_origin()
        {
            _sut.FinishFrame();
            _sut.TransferScanLine(0);
            _sut.TransferScanLine(1);

            var line = GetLine(0);
            AssertLine(line, FirstTileFirstRow);
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