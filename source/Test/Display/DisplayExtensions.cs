namespace Test.Display
{
    public static class DisplayExtensions
    {
        public static void Tick(this Core.Display display, int ticks)
        {
            for (int i = 0; i < ticks; i++)
            {
                display.Tick();
            }
        }

        public static void AdvanceScanLine(this Core.Display display)
        {
            for (var i = 0; i < 114; i++)
            {
                display.Tick();
            }
        }

        public static void AdvanceFrame(this Core.Display display)
        {
            for (var i = 0; i < 154; i++)
            {
                display.AdvanceScanLine();
            }
        }
    }
}