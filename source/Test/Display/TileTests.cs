using System;
using Core;
using Xunit;

namespace Test.Display
{
    public class TileTests
    {
        [Fact]
        public void Converts_tile_data_to_pixels()
        {
            var data = new Byte[]
            {
                0x7C, 0x7C,
                0x00, 0xC6,
                0xC6, 0x00,
                0x00, 0xFE,
                0xC6, 0xC6,
                0x00, 0xC6,
                0xC6, 0x00,
                0x00, 0x00
            };

            var sut = new DisplayRenderer.Tile();
            sut.Initialize();
            sut.Update(data);

            AssertRow(sut, 0, new byte[] { 0, 3, 3, 3, 3, 3, 0, 0 });
            AssertRow(sut, 1, new byte[] { 2, 2, 0, 0, 0, 2, 2, 0 });
        }

        public void AssertRow(DisplayRenderer.Tile sut, int row, byte[] pixels)
        {
            for (int i = 0; i < 8; i++)
            {
                var actual = sut.Pixels[8 * row + i];
                Assert.Equal(pixels[i], actual);
            }
        }
    }
}