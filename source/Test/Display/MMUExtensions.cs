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

        public static DisplayShades[] ExtractShades(this IMmu mmu)
        {
            var bgp = mmu.GetByte(0xFF47);
            var shades = new[]
            {
                (DisplayShades) (bgp & 0x3),
                (DisplayShades) ((bgp >> 2) & 0x3),
                (DisplayShades) ((bgp >> 4) & 0x3),
                (DisplayShades) ((bgp >> 6) & 0x3),
            };
            return shades;
        }
    }


}