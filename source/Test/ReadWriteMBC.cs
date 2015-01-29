using Core;

namespace Test
{
    public class ReadWriteMBC : IMBC
    {
        private readonly byte[] _memory;

        public ReadWriteMBC(byte [] memory)
        {
            _memory = memory;
        }

        public byte GetByte(ushort address)
        {
            return _memory[address];
        }

        public void SetByte(ushort address, byte value)
        {
            _memory[address] = value;
        }
    }
}