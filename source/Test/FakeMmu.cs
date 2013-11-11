using System;
using Core;

namespace Test
{
    public class FakeMmu : IMmu
    {
        public readonly byte[] Memory = new byte[ushort.MaxValue + 1];

        public byte GetByte(ushort address)
        {
            return Memory[address];
        }

        public void SetByte(ushort address, byte value)
        {
            if (address == 0xFF01 || address == 0xFF02)
            {
                Console.WriteLine(value);
            }
            Memory[address] = value;
        }
    }
}