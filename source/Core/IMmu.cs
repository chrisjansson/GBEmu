using System;

namespace Core
{
    public interface IMmu
    {
        byte GetByte(ushort address);
        void SetByte(ushort address, byte value);
    }

    public interface IDisplay
    {
        byte BackgroundPaletteData { get; set; }
        byte Line { get; set; }
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
                case RegisterAddresses.BGP:
                    return Display.BackgroundPaletteData;
                    break;
                case RegisterAddresses.LY:
                    return Display.Line;
                    break;
                default:
                    return _memory[address];
            }
        }

        public void SetByte(ushort address, byte value)
        {
            switch (address)
            {
                case RegisterAddresses.BGP:
                    Display.BackgroundPaletteData = value;
                    break;
                case RegisterAddresses.LY:
                    break;
                default:
                    _memory[address] = value;
                    break;
            }
        }

        public IDisplay Display { get; set; }
    }
}