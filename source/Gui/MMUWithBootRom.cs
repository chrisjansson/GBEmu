using System;
using Core;

namespace Gui
{
    public class MMUWithBootRom : IMmu
    {
        private readonly IMmu _mmu;
        private readonly byte[] _rom;
        private bool _switched;

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
            if (address < 256 && !_switched)
            {
                return _rom[address];
            }

            return _mmu.GetByte(address);
        }

        public void SetByte(ushort address, byte value)
        {
            if (address == 0xFF50 && !_switched)
            {
                _switched = true;
            }

            _mmu.SetByte(address, value);
        }
    }
}