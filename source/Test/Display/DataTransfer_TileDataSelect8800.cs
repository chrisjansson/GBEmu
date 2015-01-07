using Core;

namespace Test.Display
{
    public class DataTransfer_TileDataSelect8800 : BackgroundTestBase
    {
        public DataTransfer_TileDataSelect8800()
        {
            _fakeMmu.Memory[RegisterAddresses.LCDC] = 0x01; 
        }

        protected override void InsertTile(byte tileNumber, byte[] tileData)
        {
            var tileStartAddress = 0x9000 + ((sbyte)tileNumber) * 16;
            InsertTileAt((ushort)tileStartAddress, tileData);
        }

        protected override void SetBlockTile(int block, byte tile)
        {
            _fakeMmu.Memory[0x9800 + block] = tile;
        }
    }
}