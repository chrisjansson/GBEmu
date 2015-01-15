using System;
using Core;
using Xunit;

namespace Test
{
    public class MBCAssertion
    {
        private readonly IMBC _mbc;
        private readonly byte[] _rom;

        public MBCAssertion(IMBC mbc, byte[] rom)
        {
            _rom = rom;
            _mbc = mbc;
        }

        public void AssertRangeIsMapped(ushort startAddress, ushort endAddress, int romOffset = 0)
        {
            for (int i = startAddress; i < endAddress + 1; i++)
            {
                var offset = romOffset - startAddress;
                var expected = _rom[i + offset];
                var actual = _mbc.GetByte((ushort)i);
                Assert.Equal(expected, actual);
            }
        }

        public void AssertRangeIsReadOnly(ushort startAddress, ushort endAddress)
        {
            var random = new Random();
            for (int i = startAddress; i < endAddress + 1; i++)
            {
                var expected = _mbc.GetByte((ushort)i);
                _mbc.SetByte((ushort) i, (byte) random.Next(0, 255));
                var actual = _mbc.GetByte((ushort)i);
                Assert.Equal(expected, actual);
            }
        }

        public void AssertRangeIsNotMapped(int startAddress, int endAddress)
        {
            for (int i = startAddress; i < endAddress + 1; i++)
            {
                var actual = _mbc.GetByte((ushort)i);
                Assert.Equal(0, actual);
            }
        }
    }
}