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
            for (var i = 0; i < _tiles.Length; i++)
            {
                _tiles[i].Initialize();
            }
        }

        public void TransferScanLine(int line)
        {
            var lcdc = _mmu.GetByte(RegisterAddresses.LCDC);
            var tileDataSelect = (lcdc & 0x10) == 0x10 ? 0x8000 : 0x8800;
            var tileMapSelect = (lcdc & 0x08) == 0x08 ? 0x9C00 : 0x9800;

            var scrollX = _mmu.GetByte(RegisterAddresses.ScrollX);
            var scrollY = _mmu.GetByte(RegisterAddresses.ScrollY);

            var backgroundY = (line + scrollY) & 0xFF;

            for (var i = 0; i < WindowWidth; i++)
            {
                var backgroundX = (scrollX + i) & 0xFF;
                var block = backgroundX / 8 + 32 * (backgroundY / 8);
                var tileNumberData = _mmu.GetByte((ushort)(tileMapSelect + block));
                var tileIndex = tileDataSelect == 0x8000 ? tileNumberData : (sbyte)tileNumberData + 128;

                var tile = _tiles[tileIndex];

                var x = backgroundX % TileWidth;
                var y = backgroundY % TileHeight;

                var color = tile.Pixels[x + 8 * y];
                FrameBuffer[line * WindowWidth + i] = color;
            }

            for (var sprite = 0; sprite < 40; sprite++)
            {
                var spriteAddress = (ushort)(0xFE00 + sprite * 4);
                var spriteY = _mmu.GetByte(spriteAddress);
                var displayY = spriteY - 16;
                if (displayY != line)
                    continue;

                var spriteX = _mmu.GetByte((ushort)(spriteAddress + 1));
                var displayX = spriteX - 8;

                var tileNumber = _mmu.GetByte((ushort)(spriteAddress + 2));
                var tile = _tiles[tileNumber];

                for (var x = 0; x < 8; x++)
                {
                    var color = tile.Pixels[x + line * 8];
                    FrameBuffer[line * WindowWidth + x] = color;
                }
            }
        }

        public void FinishFrame()
        {
            var lcdc = _mmu.GetByte(RegisterAddresses.LCDC);
            var tileDataSelect = (lcdc & 0x10) == 0x10 ? 0x8000 : 0x8800;

            UpdateTileData((ushort)tileDataSelect);
        }

        private const int NumberOfTiles = 256;
        private const int TileSize = 16;
        private readonly byte[] _tileData = new byte[NumberOfTiles * TileSize];
        private readonly Tile[] _tiles = new Tile[NumberOfTiles];
        private void UpdateTileData(ushort tileDataStartAddress)
        {
            for (var i = 0; i < _tileData.Length; i++)
            {
                _tileData[i] = _mmu.GetByte((ushort)(tileDataStartAddress + i));
            }

            for (var i = 0; i < NumberOfTiles; i++)
            {
                var tileData = new byte[TileSize];
                Array.Copy(_tileData, i * TileSize, tileData, 0, TileSize);
                _tiles[i].Update(tileData);
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