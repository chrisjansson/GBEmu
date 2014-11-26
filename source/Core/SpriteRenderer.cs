using System;

namespace Core
{
    public interface ISpriteRenderer
    {
        void Render(int line, DisplayDataTransferService.Tile[] tiles, byte[] frameBuffer);
    }

    public class SpriteRenderer : ISpriteRenderer
    {
        private readonly IMmu _mmu;

        public SpriteRenderer(IMmu mmu)
        {
            _mmu = mmu;
        }

        public void Render(int line, DisplayDataTransferService.Tile[] tiles, byte[] frameBuffer)
        {
            var lcdc = _mmu.GetByte(RegisterAddresses.LCDC);
            var spriteEnable = (lcdc & 0x02) == 0x02;
            if (!spriteEnable)
                return;

            var largeSprites = (lcdc & 0x04) == 0x04;
            var spriteSize = largeSprites ? 16 : 8;
            for (var sprite = 0; sprite < 40; sprite++)
            {
                var spriteAddress = (ushort)(0xFE00 + sprite * 4);
                var yPos = _mmu.GetByte(spriteAddress);
                var displayY = yPos - 16;
                var spriteY = line - displayY;
                if (spriteY >= 0 && spriteY < spriteSize)
                {
                    var xPos = _mmu.GetByte((ushort)(spriteAddress + 1));
                    var tileNumber = _mmu.GetByte((ushort)(spriteAddress + 2));
                    var attributes = _mmu.GetByte((ushort)(spriteAddress + 3));

                    var tile = GetTile(lcdc, tileNumber, spriteY, tiles);
                    DrawSprite(xPos, spriteY % 8, attributes, tile, line * DisplayDataTransferService.WindowWidth, frameBuffer);
                }
            }
        }

        private DisplayDataTransferService.Tile GetTile(byte lcdc, byte tileNumber, int spriteYCoord, DisplayDataTransferService.Tile[] tiles)
        {
            var largeSprites = (lcdc & 0x04) == 0x04;
            if (largeSprites)
            {
                var firstTile = spriteYCoord <= 7;
                if (firstTile)
                {
                    return tiles[tileNumber & 0xFE];
                }

                return tiles[tileNumber | 0x01];
            }

            return tiles[tileNumber];
        }

        private static void DrawSprite(byte xPos, int spriteY, byte attributes, DisplayDataTransferService.Tile tile, int framebufferOffset, byte[] frameBuffer)
        {
            var displayXstart = xPos - 8;

            var flipX = (attributes & 0x20) == 0x20;
            var flipY = (attributes & 0x40) == 0x40;

            for (var spriteX = 0; spriteX < 8; spriteX++)
            {
                var displayX = displayXstart + spriteX;
                if (displayX >= 0 && displayX < DisplayDataTransferService.WindowWidth)
                {
                    var sourceX = flipX ? (7 - spriteX) : spriteX;
                    var sourceY = flipY ? (7 - spriteY) : spriteY;
                    var color = tile.Pixels[sourceX + sourceY * 8];
                    if (color > 0)
                        frameBuffer[framebufferOffset + displayX] = color;
                }
            }
        }
    }
}