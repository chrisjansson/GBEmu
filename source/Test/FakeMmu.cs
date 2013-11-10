﻿using System;
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
            Memory[address] = value;
        }
    }
}