using Core;

namespace Test.Display
{
    public static class MMUExtensions
    {
        public static void ScrollX(this IMmu mmu, byte scrollY)
        {
            mmu.SetByte(RegisterAddresses.ScrollX, scrollY);
        }

        public static void ScrollY(this IMmu mmu, byte scrollY)
        {
            mmu.SetByte(RegisterAddresses.ScrollY, scrollY);
        }

        public static DisplayShades[] ExtractShades(this IMmu mmu, ushort address = 0xFF47)
        {
            var source = mmu.GetByte(address);
            var shades = new[]
            {
                (DisplayShades) (source & 0x3),
                (DisplayShades) ((source >> 2) & 0x3),
                (DisplayShades) ((source >> 4) & 0x3),
                (DisplayShades) ((source >> 6) & 0x3),
            };
            return shades;
        }
    }
}