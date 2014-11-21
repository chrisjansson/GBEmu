using System;
using Core;
using Xunit;

namespace Test.Display
{
    public class SpriteRendererTests
    {
        private DisplayDataTransferService.Tile _firstTile;
        private DisplayDataTransferService.Tile _secondTile;
        private readonly SpriteRenderer _sut;
        private readonly FakeMmu _mmu;
        private readonly byte[] _firstTileFirstRow;
        private byte[] _secondTileFirstRow;
        private readonly byte[] _framebuffer;
        private readonly DisplayDataTransferService.Tile[] _tiles;

        public SpriteRendererTests()
        {
            _mmu = new FakeMmu();
            _sut = new SpriteRenderer(_mmu);
            _firstTile = new DisplayDataTransferService.Tile();
            _firstTile.Initialize();
            _firstTile.Update(new byte[]
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
            _secondTile = new DisplayDataTransferService.Tile();
            _secondTile.Initialize();
            _secondTile.Update(new byte[]
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

            _mmu.Memory[RegisterAddresses.LCDC] = 0x10;

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

            _firstTileFirstRow = new byte[]
            {
                0, 3, 3, 3, 3, 3, 0, 0
            };

            _secondTileFirstRow = new byte[]
            {
                0, 0, 3, 3, 3, 3, 0, 0
            };

            _framebuffer = new byte[160 * 144];
            _tiles = new DisplayDataTransferService.Tile[256];
            _tiles[0] = _firstTile;
            _tiles[1] = _secondTile;
        }

        [Fact]
        public void Draws_sprite_at_origin()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x10, //y = 16, displaycoordinate + 16
                0x08, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });
            _sut.Render(0, _tiles, _framebuffer);
            _sut.Render(1, _tiles, _framebuffer);
            _sut.Render(7, _tiles, _framebuffer);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, _firstTileFirstRow);
            AssertLine(second, 2, 2, 0, 0, 0, 2, 2, 0);
        }

        [Fact]
        public void Draws_sprite_offet_by_y()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x11, //y = 17, displaycoordinate + 16
                0x08, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });
            _sut.Render(0, _tiles, _framebuffer);
            _sut.Render(1, _tiles, _framebuffer);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            AssertLine(second, _firstTileFirstRow);
        }

        [Fact]
        public void Draws_sprite_offet_by_x()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x10, //y = 17, displaycoordinate + 16
                0x09, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });

            _sut.Render(0, _tiles, _framebuffer);
            _sut.Render(1, _tiles, _framebuffer);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, 0, 0, 3, 3, 3, 3, 3, 0, 0);
            AssertLine(second, 0, 2, 2, 0, 0, 0, 2, 2, 0);
        }

        protected void AssertLine(byte[] line, params byte[] colors)
        {
            for (var i = 0; i < colors.Length; i++)
            {
                Assert.Equal(colors[i], line[i]);
            }
        }

        protected void AssertLine(byte[] line, params byte[][] colors)
        {
            for (var i = 0; i < colors.Length; i++)
            {
                for (var j = 0; j < colors[i].Length; j++)
                {
                    Assert.Equal(colors[i][j], line[j + i * 8]);
                }
            }
        }

        protected byte[] GetLine(int i)
        {
            var line = new Byte[160];
            for (var x = 0; x < 160; x++)
            {
                line[x] = _framebuffer[i * 160 + x];
            }

            return line;
        }


        private void InsertSpriteAttribute(int number, byte[] o)
        {
            const int spriteAttributeStartAddress = 0xFE00;
            for (var i = 0; i < o.Length; i++)
            {
                _mmu.Memory[spriteAttributeStartAddress + number * 4 + i] = o[i];
            }
        }
    }
}