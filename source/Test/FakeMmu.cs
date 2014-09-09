using System;
using System.Collections.Generic;
using Core;

namespace Test
{
    public class FakeMmu : IMmu
    {
        public readonly byte[] Memory = new byte[ushort.MaxValue + 1];
        public readonly List<char> Output = new List<char>(); 
        

        public byte GetByte(ushort address)
        {
            return Memory[address];
        }

        public void SetByte(ushort address, byte value)
        {
            if (address == 0xFF01)
            {
                Output.Add((char) value);
            }

            Memory[address] = value;
        }
    }

    public class MMuSpy : IMmu
    {
        private readonly IMmu _mmu;
        public readonly List<char> Output = new List<char>();

        public MMuSpy(IMmu mmu)
        {
            _mmu = mmu;
        }


        public byte GetByte(ushort address)
        {
            return _mmu.GetByte(address);
        }

        public void SetByte(ushort address, byte value)
        {
            if (address == 0xFF01)
            {
                Output.Add((char) value);
            }

            _mmu.SetByte(address, value);
        }
    }
}