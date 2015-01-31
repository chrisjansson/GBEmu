using System.Linq;
using Core;
using Xunit;

namespace Test.Display
{
    public abstract class DataTransferTestBase
    {
        protected FakeMmu _fakeMmu;
        protected DisplayRenderer _sut;

        protected byte[] FirstTileFirstRow;
        protected byte[] FirstTileSecondRow;
        protected byte[] SecondTileFirstRow;

        protected DataTransferTestBase()
        {
            _fakeMmu = new FakeMmu();
            _sut = new DisplayRenderer(_fakeMmu, new SpriteRenderer(_fakeMmu));

            InsertTile(0, new byte[]
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

            InsertTile(128, new byte[]
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

            SetBlockTile(0, 0);
            SetBlockTile(1, 128);
            SetBlockTile(0x1F, 128);
            SetBlockTile(0x20, 128);
            SetBlockTile(0x3E0, 128);

            FirstTileFirstRow = new byte[]
            {
                0, 3, 3, 3, 3, 3, 0, 0
            };

            FirstTileSecondRow = new byte[]
            {
                2, 2, 0, 0, 0, 2, 2, 0
            };

            SecondTileFirstRow = new byte[]
            {
                0, 0, 3, 3, 3, 3, 0, 0
            };
        }

        protected abstract void InsertTile(byte tileNumber, byte[] tileData);
        protected abstract void SetBlockTile(int block, byte tile);

        protected void AssertLine(Pixel[] actualPixels, params byte[][] colors)
        {
            var expected = colors
                .SelectMany(x => x)
                .ToArray();

            AssertLine(actualPixels, expected);
        }

        protected void AssertLine(Pixel[] actual, params byte[] colors)
        {
            var shades = ExtractShades();

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

        protected void AssertLineIsEmpty(Pixel[] actualPixels)
        {
            var expected = Enumerable.Range(0, actualPixels.Length)
                .Select(x => new Pixel(0, DisplayShades.White))
                .ToArray();

            Assert.Equal(expected, actualPixels);
        }

        private DisplayShades[] ExtractShades()
        {
            var bgp = _fakeMmu.GetByte(0xFF47);
            var shades = new[]
            {
                (DisplayShades) (bgp & 0x3),
                (DisplayShades) ((bgp >> 2) & 0x3),
                (DisplayShades) ((bgp >> 4) & 0x3),
                (DisplayShades) ((bgp >> 6) & 0x3),
            };
            return shades;
        }

        protected Pixel[] GetLine(int i)
        {
            var line = new Pixel[160];
            for (var x = 0; x < 160; x++)
            {
                line[x] = _sut.FrameBuffer[i * 160 + x];
            }

            return line;
        }

        protected void InsertTileAt(ushort address, byte[] tile)
        {
            Assert.Equal(16, tile.Length);
            for (int i = 0; i < tile.Length; i++)
            {
                _fakeMmu.Memory[address + i] = tile[i];
            }
        }
    }
}