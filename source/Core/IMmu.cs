namespace Core
{
    public interface IMmu
    {
        byte GetByte(ushort address);
        void SetByte(ushort address, byte value);
    }

    public enum WindowTileMapDisplaySelect
    {
        _9800,
        _9C00
    }

    public interface IDisplay
    {

        byte BackgroundPaletteData { get; set; }
        byte Line { get; }
        byte LCDC { get; set; }
        byte STAT { get; set; }
        byte DMA { get; set; }
        byte LYC { get; set; }
    }

    public class MMU : IMmu
    {
        private readonly byte[] _memory;

        public MMU()
        {
            _memory = new byte[ushort.MaxValue + 1];
        }

        public byte GetByte(ushort address)
        {
            switch (address)
            {
                case RegisterAddresses.P1:
                    return Joypad.P1;
                case RegisterAddresses.LCDC:
                    return Display.LCDC;
                case RegisterAddresses.LCDSTAT:
                    return Display.STAT;
                case RegisterAddresses.BGP:
                    return Display.BackgroundPaletteData;
                case RegisterAddresses.LY:
                    return Display.Line;
                case RegisterAddresses.LYC:
                    return Display.LYC;
                case RegisterAddresses.IE:
                    return Cpu.IE;
                case RegisterAddresses.IF:
                    return Cpu.IF;
                case RegisterAddresses.TIMA:
                    return Timer.TIMA;
                case RegisterAddresses.TMA:
                    return Timer.TMA;
                case RegisterAddresses.TAC:
                    return Timer.TAC;
                case RegisterAddresses.DIV:
                    return Timer.DIV;
                case RegisterAddresses.DMA:
                    return Display.DMA;
                default:
                    return _memory[address];
            }
        }

        public void SetByte(ushort address, byte value)
        {
            switch (address)
            {
                case RegisterAddresses.P1:
                    Joypad.P1 = value;
                    break;
                case RegisterAddresses.LCDC:
                    Display.LCDC = value;
                    break;
                case RegisterAddresses.LCDSTAT:
                    Display.STAT = value;
                    break;
                case RegisterAddresses.BGP:
                    Display.BackgroundPaletteData = value;
                    break;
                case RegisterAddresses.LY:
                    break;
                case RegisterAddresses.LYC:
                    Display.LYC = value;
                    break;
                case RegisterAddresses.IE:
                    Cpu.IE = value;
                    break;
                case RegisterAddresses.IF:
                    Cpu.IF = value;
                    break;
                case RegisterAddresses.TIMA:
                    Timer.TIMA = value;
                    break;
                case RegisterAddresses.TMA:
                    Timer.TMA = value;
                    break;
                case RegisterAddresses.TAC:
                    Timer.TAC = value;
                    break;
                case RegisterAddresses.DIV:
                    Timer.DIV = value;
                    break;
                case RegisterAddresses.DMA:
                    Display.DMA = value;
                    break;
                default:
                    _memory[address] = value;
                    break;
            }
        }

        public Timer Timer;
        public IDisplay Display;
        public Cpu Cpu;
        public IJoypad Joypad;
    }
}