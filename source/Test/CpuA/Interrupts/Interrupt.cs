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
        public static readonly Interrupt Timer = new Interrupt(0x04, 0x50);
    }
}