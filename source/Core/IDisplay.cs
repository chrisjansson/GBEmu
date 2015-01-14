namespace Core
{
    public interface IDisplay
    {

        byte BackgroundPaletteData { get; set; }
        byte Line { get; }
        byte LCDC { get; set; }
        byte STAT { get; set; }
        byte DMA { get; set; }
        byte LYC { get; set; }
    }
}