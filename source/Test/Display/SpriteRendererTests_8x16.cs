using Core;
using Xunit;

namespace Test.Display
{
    public class SpriteRendererTests_8x16 : SpriteRendererTestsBase
    {
        public SpriteRendererTests_8x16()
        {
            Mmu.Memory[RegisterAddresses.LCDC] |= 0x04;
        }

        [Fact]
        public void Draws_sprite_at_origin()
        {
            InsertSpriteAttribute(number: 1, y: 16, x: 8, flags: 0, tile: 0);

            RenderLine(0);
            RenderLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, FirstTileFirstRow);
            AssertLine(second, 2, 2, 0, 0, 0, 2, 2, 0);
        }

        [Fact]
        public void Draws_tile_two_as_second_part_of_sprite_1()
        {
            InsertSpriteAttribute(number: 1, y: 16, x: 8, flags: 0, tile: 0);

            RenderLine(8);

            var line = GetLine(8);
            AssertLine(line, SecondTileFirstRow);
        }

        [Fact]
        public void Flips_sprite_in_y_direction()
        {
            InsertSpriteAttribute(number: 1, y: 16, x: 8, flags: 0x40, tile: 0);

            RenderLine(1);
            RenderLine(15);

            var first = GetLine(1);
            var second = GetLine(15);
            AssertLine(first, SecondTileSecondLastRow);
            AssertLine(second, FirstTileFirstRow);
        }
    }
}