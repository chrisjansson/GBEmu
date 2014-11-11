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
        private readonly byte[] _tileData = new byte[0x1000];
        private readonly Tile[] _tiles = new Tile[256];

        public DisplayDataTransferService(IMmu mmu)
        {
            _mmu = mmu;
            for (var i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].Initialize();
            }
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

        private readonly Tile[] _internalScreen = new Tile[1024];

        public void FinishFrame()
        {
            UpdateTiles();
            var scrollX = _mmu.GetByte(RegisterAddresses.ScrollX);
            var scrollY = _mmu.GetByte(RegisterAddresses.ScrollY);

            for (var y = 0; y < 32; y++)
            {
                for (var x = 0; x < 32; x++)
                {
                    var charData = _mmu.GetByte((ushort)(0x9800 + x + y * 32));
                    var tile = _tiles[charData];
                    _internalScreen[x + y * 32] = tile;
                }
            }

            for (int y = 0; y < WindowHeight; y++)
            {
                for (int x = 0; x < WindowWidth; x++)
                {
                    var pixelX = (scrollX + x) % 256;
                    var pixelY = (scrollY + y) % 256;

                    var tileX = pixelX / TileWidth;
                    var tileY = pixelY / TileHeight;

                    var tile = _internalScreen[tileX + tileY * 32];

                    var color = tile.Pixels[(pixelX % TileWidth) + (pixelY % TileHeight) * 8];
                    FrameBuffer[x + y * 160] = color;
                }
            }
        }

        private void UpdateTiles()
        {
            for (var i = 0; i < _tileData.Length; i++)
            {
                _tileData[i] = _mmu.GetByte((ushort)(0x8000 + i));
            }

            for (var i = 0; i < 0x1000; i += 16)
            {
                var tileData = new byte[16];
                Array.Copy(_tileData, i, tileData, 0, 16);
                _tiles[i / 16].Update(tileData);
            }
        }

        public struct Tile
        {
            public byte[] Pixels;

            public void Initialize()
            {
                Pixels = new byte[64];
            }

            public void Update(byte[] data)
            {
                for (var y = 0; y < 8; y++)
                {
                    var low = data[y * 2];
                    var high = data[y * 2 + 1];

                    for (var x = 0; x < 8; x++)
                    {
                        var l = (low >> (7 - x)) & 0x01;
                        var h = (high >> (7 - x)) & 0x01;
                        Pixels[x + y * 8] = (byte)((h << 1) | l);
                    }
                }
            }
        }
    }
}