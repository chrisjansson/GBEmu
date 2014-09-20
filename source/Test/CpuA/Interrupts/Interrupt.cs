namespace Test.CpuA.Interrupts
{
    public class Interrupt
    {
        private Interrupt(byte interruptMask, ushort interruptVector)
        {
            InterruptMask = interruptMask;
            InterruptVector = interruptVector;
        }

        public byte InterruptMask { get; private set; }
        public ushort InterruptVector { get; private set; }

        public static readonly Interrupt VBlank = new Interrupt(0x01, 0x40);
        public static readonly Interrupt LCDStat = new Interrupt(0x02, 0x48);
        public static readonly Interrupt Timer = new Interrupt(0x04, 0x50);
        public static readonly Interrupt Serial = new Interrupt(0x08, 0x58);
        public static readonly Interrupt JoyPad = new Interrupt(0x10, 0x60);
    }
}