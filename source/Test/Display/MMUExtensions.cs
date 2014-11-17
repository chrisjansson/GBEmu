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
    }
}