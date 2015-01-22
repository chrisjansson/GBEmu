using System;

namespace Core
{
    public class MmuWithBootRom : IMmu
    {
        private readonly IMmu _mmu;
        private readonly byte[] _rom;

        public MmuWithBootRom(byte[] rom, IMmu mmu)
        {
            if (rom.Length != 256)
            {
                throw new NotSupportedException();
            }

            _rom = rom;
            _mmu = mmu;
        }

        public bool Switched { get; private set; }

        public byte GetByte(ushort address)
        {
            if (address < 256 && !Switched)
            {
                return _rom[address];
            }

            return _mmu.GetByte(address);
        }

        public void SetByte(ushort address, byte value)
        {
            if (address == 0xFF50 && !Switched)
            {
                Switched = true;
            }

            _mmu.SetByte(address, value);
        }
    }
}