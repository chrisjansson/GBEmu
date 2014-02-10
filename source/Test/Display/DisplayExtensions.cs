namespace Test.Display
{
    public static class DisplayExtensions
    {
        public static void AdvanceScanLine(this Core.Display display)
        {
            for (var i = 0; i < 114; i++)
            {
                display.Tick();
            }
        }
    }
}