using System;
using Core;

namespace Gui
{
    public class MMUWithBootRom : IMmu
    {
        private readonly IMmu _mmu;
        private readonly byte[] _rom;

        public MMUWithBootRom(byte[] rom, IMmu mmu)
        {
            if (rom.Length != 256)
            {
                throw new NotSupportedException();
            }

            _rom = rom;
            _mmu = mmu;
        }

        public byte GetByte(ushort address)
        {
            if (address < 256)
            {
                return _rom[address];
            }

            return _mmu.GetByte(address);
        }

        public void SetByte(ushort address, byte value)
        {
            _mmu.SetByte(address, value);
        }
    }
}