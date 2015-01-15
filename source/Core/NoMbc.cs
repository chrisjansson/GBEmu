using System;

namespace Core
{
    public class NoMBC : IMBC
    {
        private readonly byte[] _rom;
        private const int MaxRomSize = 0x8000; //32kB
        public NoMBC(byte[] rom)
        {
            if (rom.Length != MaxRomSize)
                throw new NotSupportedException();
            _rom = rom;
        }

        public byte GetByte(ushort i)
        {
            if (i < MaxRomSize)
            {
                return _rom[i];
            }

            return 0;
        }

        public void SetByte(ushort address, byte value)
        {
        }
    }
}