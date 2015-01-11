using System;

namespace Core
{
    public class NoMBC
    {
        private readonly byte[] _rom;
        private const int MaxRomSize = 0x8000; //32kB
        public NoMBC(byte[] rom)
        {
            if(rom.Length != MaxRomSize)
                throw new NotSupportedException();
            _rom = rom;
        }

        public byte GetByte(ushort i)
        {
            return _rom[i];
        }
    }
}