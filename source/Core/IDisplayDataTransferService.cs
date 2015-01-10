using System;
using System.Diagnostics;

namespace Core
{
    public interface IDisplayDataTransferService
    {
        void TransferScanLine(int line);
        void FinishFrame();
    }

    public class DisplayDataTransferService : IDisplayDataTransferService
    {
        public const int WindowWidth = 160;
        private const int WindowHeight = 144;
        private const int TileHeight = 8;
        private const int TileWidth = 8;
        private const int NumberOfTiles = 256;
        private const int TileSize = 16;

        private readonly IMmu _mmu;
        private readonly ISpriteRenderer _spriteRenderer;

        public readonly byte[] FrameBuffer = new Byte[WindowWidth * WindowHeight];
        private readonly byte[] _tileData = new byte[NumberOfTiles * TileSize];
        private readonly Tile[] _tiles8000 = new Tile[NumberOfTiles];
        private readonly Tile[] _tiles8800 = new Tile[NumberOfTiles];

        public DisplayDataTransferService(IMmu mmu, ISpriteRenderer spriteRenderer)
        {
            _mmu = mmu;
            _spriteRenderer = spriteRenderer;
            for (var i = 0; i < _tiles8000.Length; i++)
            {
                _tiles8000[i].Initialize();
                _tiles8800[i].Initialize();
            }
        }

        public void TransferScanLine(int line)
        {
            var lcdc = _mmu.GetByte(RegisterAddresses.LCDC);
            var tileDataSelect = (lcdc & 0x10) == 0x10 ? 0x8000 : 0x8800;
            var tileMapSelect = (lcdc & 0x08) == 0x08 ? 0x9C00 : 0x9800;
            var renderBackground = (lcdc & 0x01) == 0x01;
            var renderWindow = (lcdc & 0x20) == 0x20;

            //Debug.Assert((lcdc & 0x20) != 0x20, "Window display enabled is not implemented");

            if (renderBackground)
            {
                var scrollX = _mmu.GetByte(RegisterAddresses.ScrollX);
                var scrollY = _mmu.GetByte(RegisterAddresses.ScrollY);

                var backgroundY = (line + scrollY) & 0xFF;

                for (var i = 0; i < WindowWidth; i++)
                {
                    var backgroundX = (scrollX + i) & 0xFF;
                    var block = backgroundX / 8 + 32 * (backgroundY / 8);
                    var tileNumberData = _mmu.GetByte((ushort)(tileMapSelect + block));
                    var tileIndex = tileDataSelect == 0x8000 ? tileNumberData : (sbyte)tileNumberData + 128;
                    var tile = tileDataSelect == 0x8000 ? _tiles8000[tileNumberData] : _tiles8800[tileIndex];

                    var x = backgroundX % TileWidth;
                    var y = backgroundY % TileHeight;

                    var color = tile.Pixels[x + 8 * y];
                    FrameBuffer[line * WindowWidth + i] = color;
                }
            }

            if (renderWindow)
            {
                var wx = _mmu.GetByte(RegisterAddresses.WX);
                var wy = _mmu.GetByte(RegisterAddresses.WY);
                var windowTileMapSelect = (lcdc & 0x40) == 0x40 ? 0x9C00 : 0x9800;
                var windowLine = line - wy;
                if (windowLine >= 0)
                {
                    var startX = wx - 7;
                    var toDraw = WindowWidth - startX;
                    for (var i = 0; i < toDraw; i++)
                    {
                        var displayX = startX + i;
                        var block = i / 8 + (windowLine / 8) * 32;

                        var tileNumber = _mmu.GetByte((ushort)(windowTileMapSelect + block));
                        var tile = _tiles8000[tileNumber];

                        var color = tile.Pixels[i % TileWidth + (windowLine % TileHeight) * TileWidth];
                        FrameBuffer[displayX + line * WindowWidth] = color;
                    }
                }
            }

            _spriteRenderer.Render(line, _tiles8000, FrameBuffer);
        }

        public void FinishFrame()
        {
            UpdateTileData(0x8000, _tiles8000);
            UpdateTileData(0x8800, _tiles8800);
        }

        private void UpdateTileData(ushort tileDataStartAddress, Tile[] tiles)
        {
            for (var i = 0; i < _tileData.Length; i++)
            {
                _tileData[i] = _mmu.GetByte((ushort)(tileDataStartAddress + i));
            }

            for (var i = 0; i < NumberOfTiles; i++)
            {
                var tileData = new byte[TileSize];
                Array.Copy(_tileData, i * TileSize, tileData, 0, TileSize);
                tiles[i].Update(tileData);
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