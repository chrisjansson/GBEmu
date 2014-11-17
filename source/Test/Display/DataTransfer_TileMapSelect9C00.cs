using Core;

namespace Test.Display
{
    public class DataTransfer_TileMapSelect9C00 : DataTransferTestBase
    {
        public DataTransfer_TileMapSelect9C00()
        {
            _fakeMmu.SetByte(RegisterAddresses.LCDC, 0x18);
        }

        protected override void InsertTile(byte tileNumber, byte[] tileData)
        {
            InsertTileAt((ushort)(0x8000 + tileNumber * 16), tileData);
        }

        protected override void SetBlockTile(int block, byte tile)
        {
            _fakeMmu.Memory[0x9C00 + block] = tile;
        }
    }
}