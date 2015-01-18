namespace Core
{
    public class MBC1 : IMBC
    {
        private readonly byte[] _rom;
        private byte _lowRomSelect;
        private byte _highRomSelect;

        public MBC1(byte[] rom)
        {
            _rom = rom;
        }

        public byte GetByte(ushort address)
        {
            if (address < 0x4000)
            {
                return _rom[address];
            }

            const int romBankSize = 0x4000;
            if (address >= 0x4000 && address < 0x8000)
            {
                var lowerSelect = (_lowRomSelect & 0x1F);
                var upperSelect = (_highRomSelect & 0x03);
                var bank = (upperSelect << 5) | lowerSelect;
                if (bank == 0 || bank == 0x20 || bank == 0x40 || bank == 0x60)
                    bank++;
                var offset = romBankSize * bank;
                var baseAddress = address - 0x4000;
                return _rom[baseAddress + offset];
            }

            if (address >= 0xA000 && address < 0xC000)
            {
                return 0;
            }

            return _rom[address];
        }

        public void SetByte(ushort address, byte value)
        {
            if (address == 0x2000)
                _lowRomSelect = value;
            if (address == 0x4000)
                _highRomSelect = value;
        }
    }
}