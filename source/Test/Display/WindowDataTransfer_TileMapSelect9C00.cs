using Core;

namespace Test.Display
{
    public class WindowDataTransfer_TileMapSelect9C00 : WindowDataTransferTestBase
    {
        public WindowDataTransfer_TileMapSelect9C00()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] |= 0x50;
        }
        protected override void InsertTile(byte tileNumber, byte[] tileData)
        {
            InsertTileAt((ushort)(0x8000 + tileNumber * 16), tileData);
            //Bit 4 - BG & Window Tile Data Select(0 = 8800 - 97FF, 1 = 8000 - 8FFF)
        }

        protected override void SetBlockTile(int block, byte tile)
        {
            //Bit 6 - Window Tile Map Display Select (0 = 9800 - 9BFF, 1 = 9C00 - 9FFF)
            _fakeMmu.Memory[0x9C00 + block] = tile;
        }
    }
}