using Core;

namespace Test
{
    public class NullDisplay : IDisplay
    {
        public byte BackgroundPaletteData { get; set; }
        public byte Line { get; private set; }
        public byte LCDC { get; set; }
    }
}