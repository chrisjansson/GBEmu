﻿namespace Core
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

            for (var sprite = 0; sprite < 40; sprite++)
            {
                var spriteAddress = (ushort)(0xFE00 + sprite * 4);
                var spriteY = _mmu.GetByte(spriteAddress);
                var displayY = spriteY - 16;
                var spriteYCoord = line - displayY;
                if (spriteYCoord >= 0 && spriteYCoord <= 7)
                {
                    var spriteX = _mmu.GetByte((ushort)(spriteAddress + 1));
                    var startX = spriteX - 8;

                    var tileNumber = _mmu.GetByte((ushort)(spriteAddress + 2));
                    var tile = tiles[tileNumber];

                    for (var x = 0; x < 8; x++)
                    {
                        var displayX = startX + x;
                        if (displayX >= 0 && displayX < 160)
                        {
                            var color = tile.Pixels[x + spriteYCoord * 8];
                            frameBuffer[line * DisplayDataTransferService.WindowWidth + displayX] = color;
                        }
                    }
                }
            }
        }
    }
}