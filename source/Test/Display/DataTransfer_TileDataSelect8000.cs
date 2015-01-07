using Core;

namespace Test.Display
{
    public class DataTransfer_TileDataSelect8000 : BackgroundTestBase
    {
        public DataTransfer_TileDataSelect8000()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] |= 0x10;
        }

        protected override void InsertTile(byte tileNumber, byte[] tileData)
        {
            InsertTileAt((ushort)(0x8000 + tileNumber * 16), tileData);
        }

        protected override void SetBlockTile(int block, byte tile)
        {
            _fakeMmu.Memory[0x9800 + block] = tile;
        }
    }
}