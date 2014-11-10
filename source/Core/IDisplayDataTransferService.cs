using System;

namespace Core
{
    public interface IDisplayDataTransferService
    {
        void TransferScanLine(int line);
        void FinishFrame();
    }

    public class DisplayDataTransferService : IDisplayDataTransferService
    {
        private const int WindowWidth = 160;
        private const int WindowHeight = 144;
        private const int TileHeight = 8;
        private const int TileWidth = 8;

        private readonly IMmu _mmu;
        public byte[] FrameBuffer = new Byte[WindowWidth * WindowHeight];

        public DisplayDataTransferService(IMmu mmu)
        {
            _mmu = mmu;
        }


        public void TransferScanLine(int line)
        {
            //var scrollX = _mmu.GetByte(RegisterAddresses.ScrollX);
            //var scrollY = _mmu.GetByte(RegisterAddresses.ScrollY);

            //var backgroundY = (line + scrollY) & 0xFF;
            //for (var i = 0; i < WindowWidth; i++)
            //{
            //    var backgroundX = (scrollX + i) & 0xFF;
            //    var block = backgroundX / 8 + 32 * (backgroundY / 8);
            //    var tileNumber = _mmu.GetByte((ushort)(0x9800 + block));

            //    var y = backgroundY % TileHeight;
            //    var lower = _mmu.GetByte((ushort)(0x8000 + (tileNumber * 16) + y * 2));
            //    var higher = _mmu.GetByte((ushort)(0x8000 + (tileNumber * 16) + 1 + y * 2));

            //    var x = backgroundX % TileWidth;
            //    var lowerPixel = (lower >> (7 - x)) & 0x01;
            //    var higherPixel = (higher >> (6 - x)) & 0x02;

            //    var color = higherPixel | lowerPixel;
            //    FrameBuffer[line * WindowWidth + i] = (byte)color;
            //}
        }

        private Tile[] _internalScreen = new Tile[1024];

        public void FinishFrame()
        {
            var tiles = GetTiles();
            var scrollX = _mmu.GetByte(RegisterAddresses.ScrollX);
            var scrollY = _mmu.GetByte(RegisterAddresses.ScrollY);

            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    var charData = _mmu.GetByte((ushort)(0x9800 + x + y * 32));
                    var tile = tiles[charData];
                    _internalScreen[x + y * 32] = tile;
                }
            }

            for (int y = 0; y < WindowHeight; y++)
            {
                for (int x = 0; x < WindowWidth; x++)
                {
                    var pixelX = (scrollX + x) % 256;
                    var pixelY = (scrollY + y) % 256;

                    var tileX = pixelX/TileWidth;
                    var tileY = pixelY/TileHeight;

                    var tile = tiles[tileX + tileY*32];

                    var color = tile.Pixels[(pixelX%TileWidth) + (pixelY%TileHeight)*8];
                    FrameBuffer[x + y*160] = color;
                }
            }
        }

        private Tile[] GetTiles()
        {
            var data = new byte[0x1000];
            for (var i = 0; i < 0x1000; i++)
            {
                data[i] = _mmu.GetByte((ushort)(0x8000 + i));
            }

            var tiles = new Tile[256];
            for (var i = 0; i < 0x1000; i += 16)
            {
                var tileData = new byte[16];
                Array.Copy(data, i, tileData, 0, 16);
                tiles[i / 16] = new Tile(tileData);
            }
            return tiles;
        }

        private struct Tile
        {
            public byte[] Pixels;

            public Tile(byte[] data)
            {
                Pixels = new byte[64];
                for (int i = 0; i < 8; i += 2)
                {
                    var low = data[i];
                    var high = data[i + 1];
                    for (int j = 0; j < 8; j++)
                    {
                        var mask = 1 << j;
                        var pixel = (high & mask) << 1 | (low & mask);
                        Pixels[j + i * 8] = (byte)pixel;
                    }
                }
            }
        }
    }
}