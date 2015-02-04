using System.Linq;
using Core;
using Xunit;

namespace Test.Display
{
    public class SpriteRendererTests_8x8 : SpriteRendererTestsBase
    {
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
            RenderLine(0);
            RenderLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, FirstTileFirstRow);
            AssertLine(second, Line(2, 2, 0, 0, 0, 2, 2, 0));
        }

        [Fact]
        public void Sprite_color_0_does_not_overwrite_frame_buffer_color()
        {
            InsertSpriteAttribute(1, 16, 8, 0, 0);
            Framebuffer[0] = new Pixel(1, DisplayShades.White);
            Framebuffer[1] = new Pixel(2, DisplayShades.White);
            RenderLine(0);

            var first = GetLine(0);
            AssertLine(first, Line(1, 3, 3, 3, 3, 3, 0, 0));
        }

        [Fact]
        public void Draws_sprite_with_colors_from_OBP0()
        {
            Mmu.SetByte(0xFF48, 0xB1 & 0xFC); //only color 1-3 are used
            InsertSpriteAttribute(1, 16, 8, 0, 0);
            RenderLine(0);

            var first = GetLine(0);
            AssertLine(first, Line(0, 3, 3, 3, 3, 3, 0, 0), 0xFF48);
        }

        [Fact]
        public void Draws_sprite_with_colors_from_OBP1()
        {
            Mmu.SetByte(0xFF49, 0xB1 & 0xFC); //only color 1-3 are used
            InsertSpriteAttribute(1, 16, 8, 0x10, 0); //select color from obp1
            RenderLine(0);

            var first = GetLine(0);
            AssertLine(first, Line(0, 3, 3, 3, 3, 3, 0, 0), 0xFF49);
        }

        [Fact]
        public void Does_not_draw_sprite_when_obj_display_is_disabled()
        {
            Mmu.SetByte(RegisterAddresses.LCDC, (byte)(Mmu.GetByte(RegisterAddresses.LCDC) & 0xFD));
            InsertSpriteAttribute(1, new byte[]
            {
                0x10, //y = 16, displaycoordinate + 16
                0x08, //x = 8, displaycoordinate + 8
                0x00, //Tile number
                0x00, //Flags
            });

            RenderLine(0);

            var line = GetLine(0);
            AssertLine(line, Line(0, 0, 0, 0, 0, 0, 0, 0));
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
            RenderLine(0);
            RenderLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });
            AssertLine(second, FirstTileFirstRow);
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
            RenderLine(0);
            RenderLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, Line(0, 0, 3, 3, 3, 3, 3, 0, 0));
            AssertLine(second, Line(0, 2, 2, 0, 0, 0, 2, 2, 0));
        }

        [Fact]
        public void Does_not_underflow_frame_buffer_when_sprite_is_to_the_left_of_screen()
        {
            InsertSpriteAttribute(1, new byte[]
            {
                0x10, 
                0x07,
                0x00,
                0x00, 
            });
            RenderLine(0);
            RenderLine(1);

            var first = GetLine(0);
            var second = GetLine(1);
            AssertLine(first, Line(3, 3, 3, 3, 3, 0, 0, 0));
            AssertLine(second, Line(2, 0, 0, 0, 2, 2, 0, 0));
        }

        [Fact]
        public void Does_not_overflow_to_next_row_when_drawing_sprite_to_the_right()
        {
            InsertSpriteAttribute(number: 1, y: 16, x: 164, tile: 0, flags: 0);

            RenderLine(0);

            var one = GetLine(1);
            AssertLine(one, Line(0, 0, 0, 0, 0, 0, 0, 0));
        }

        [Fact]
        public void Does_not_overflow_frame_ufer_when_drawing_sprite_down_to_the_right()
        {
            InsertSpriteAttribute(number: 1, y: 159, x: 8, flags: 0, tile: 0);

            RenderLine(143);

            var lastLine = GetLine(143);
            AssertLine(lastLine, FirstTileFirstRow);
        }

        [Fact]
        public void Flips_sprite_in_x_direction()
        {
            InsertSpriteAttribute(1, 16, 8, 0x20, 0);

            RenderLine(0);

            var line = GetLine(0);
            AssertLine(line, FirstTileFirstRow.Reverse().ToArray());
        }

        [Fact]
        public void Flips_sprite_in_y_direction()
        {
            InsertSpriteAttribute(1, 16, 8, 0x40, 0);

            RenderLine(6);
            RenderLine(7);

            var first = GetLine(6);
            var second = GetLine(7);
            AssertLine(first, Line(2, 2, 0, 0, 0, 2, 2, 0));
            AssertLine(second, FirstTileFirstRow);
        }
    }
}