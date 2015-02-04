using System;
using System.Linq;
using Core;
using Xunit;

namespace Test.Display
{
    public abstract class SpriteRendererTestsBase
    {
        private readonly SpriteRenderer _sut;
        protected readonly FakeMmu Mmu;
        protected readonly byte[] FirstTileFirstRow;
        protected readonly Pixel[] Framebuffer;
        private readonly DisplayRenderer.Tile[] _tiles;
        protected readonly byte[] SecondTileFirstRow;
        protected readonly byte[] SecondTileSecondLastRow;

        protected SpriteRendererTestsBase()
        {
            Mmu = new FakeMmu();
            _sut = new SpriteRenderer(Mmu);
            var firstTile = new DisplayRenderer.Tile();
            firstTile.Initialize();
            firstTile.Update(new byte[]
            {
                0x7C, 0x7C, 
                0x00, 0xC6, 
                0xC6, 0x00, 
                0x00, 0xFE,
                0xC6, 0xC6,
                0x00, 0xC6,
                0xC6, 0x00,
                0x00, 0x00
            });
            var secondTile = new DisplayRenderer.Tile();
            secondTile.Initialize();
            secondTile.Update(new byte[]
            {
                0x3C, 0x3C,
                0x66, 0x66,
                0x6E, 0x6E,
                0x76, 0x76,
                0x66, 0x66,
                0x66, 0x66,
                0x3C, 0x3C,
                0x00, 0x00,
            });

            Mmu.Memory[RegisterAddresses.LCDC] = 0x10;

            for (var i = 0; i < 40; i++)
            {
                InsertSpriteAttribute(i, new byte[]
                {
                    0x00,
                    0x00,
                    0x00,
                    0x00
                });
            }

            FirstTileFirstRow = new byte[]
            {
                0, 3, 3, 3, 3, 3, 0, 0
            };
            
            SecondTileFirstRow = new byte[]
            {
                0, 0, 3, 3, 3, 3, 0, 0
            };
            SecondTileSecondLastRow = SecondTileFirstRow;

            Framebuffer = new Pixel[160 * 144];
            _tiles = new DisplayRenderer.Tile[256];
            _tiles[0] = firstTile;
            _tiles[1] = secondTile;

            Mmu.SetByte(RegisterAddresses.LCDC, 0x02);
        }

        protected void RenderLine(int line)
        {
            _sut.Render(line, _tiles, Framebuffer);
        }

        protected void AssertLine(Pixel[] actualPixels, byte[][] colors)
        {
            var expected = colors
                .SelectMany(x => x)
                .ToArray();

            AssertLine(actualPixels, expected);
        }

        protected void AssertLine(Pixel[] actual, byte[] colors)
        {
            var shades = Mmu.ExtractShades();

            var expectedShades = colors
                .Select(x => shades[x])
                .ToArray();

            var actualColors = actual
                .Select(x => x.Color)
                .Take(colors.Length)
                .ToArray();

            var actualShades = actual
                .Select(x => x.Shade)
                .Take(colors.Length)
                .ToArray();

            Assert.Equal(colors, actualColors);
            Assert.Equal(expectedShades, actualShades);
        }

        protected byte[] Line(params byte[] colors)
        {
            return colors;
        }

        protected byte[][] Line(params byte[][] colors)
        {
            return colors;
        }

        protected Pixel[] GetLine(int i)
        {
            var line = new Pixel[160];
            for (var x = 0; x < 160; x++)
            {
                line[x] = Framebuffer[i * 160 + x];
            }

            return line;
        }


        protected void InsertSpriteAttribute(int number, byte[] o)
        {
            const int spriteAttributeStartAddress = 0xFE00;
            for (var i = 0; i < o.Length; i++)
            {
                Mmu.Memory[spriteAttributeStartAddress + number * 4 + i] = o[i];
            }
        }

        protected void InsertSpriteAttribute(int number, byte y, byte x, byte flags, byte tile)
        {
            InsertSpriteAttribute(number, new byte[] { y, x, tile, flags });
        }
    }
}