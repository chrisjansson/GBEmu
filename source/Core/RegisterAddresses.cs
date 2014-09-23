namespace Core
{
    public class RegisterAddresses
    {
        public const ushort TAC = 0xFF07; //Timer control
        public const ushort TMA = 0xFF06; //Timer modulo
        public const ushort TIMA = 0xFF05; //Timer counter
        public const ushort LCDC = 0xFF40;
        public const ushort ScrollX = 0xFF43;
        public const ushort ScrollY = 0xFF42;
        public const ushort LY = 0xFF44;
        public const ushort BGP = 0xFF47;
        public const ushort IE = 0xFFFF; //Interrupt enable
        public const ushort IF = 0xFF0F; //Interrupt request
    }
}