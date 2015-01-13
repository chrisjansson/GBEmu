using System;
using Core;
using Xunit;

namespace Test.MMU
{
    public class MMUTests
    {
        [Fact]
        public void Reads_0000_to_7FFF_from_MBC() //ROM
        {
            AssertReadsRangeFromMBC(startAddress: 0, endAddress: 0x7FFF);
        }

        [Fact]
        public void Writes_0000_to_7FFF_to_MBC() //ROM
        {
            AssertWritesRangeToMBC(startAddress: 0, endAddress: 0x7FFF);
        }

        [Fact]
        public void Reads_A000_to_BFFF_from_MBC() //External RAM
        {
            AssertReadsRangeFromMBC(startAddress: 0xA000, endAddress: 0xBFFF);
        }

        [Fact]
        public void Writes_A000_to_BFFF_to_MBC()
        {
            AssertWritesRangeToMBC(startAddress: 0xA000, endAddress: 0xBFFF);
        }

        private void AssertReadsRangeFromMBC(ushort startAddress, ushort endAddress)
        {
            var mbc = CreateFakeMBC();
            var mmu = new Core.MMU(mbc);

            for (int i = startAddress; i < endAddress + 1; i++)
            {
                var expected = mbc.Memory[i];
                var actual = mmu.GetByte((ushort) i);
                Assert.Equal(expected, actual);
            }
        }

        private static void AssertWritesRangeToMBC(int startAddress, int endAddress)
        {
            var mbc = CreateFakeMBC();
            var mmu = new Core.MMU(mbc);
            var random = new Random();
            for (var i = startAddress; i < endAddress + 1; i++)
            {
                var expected = (byte) random.Next(0, 255);
                mmu.SetByte((ushort) i, expected);
                var actual = mbc.Memory[i];
                Assert.Equal(expected, actual);
            }
        }

        private static FakeMBC CreateFakeMBC()
        {
            var mbc = new FakeMBC();
            var random = new Random();
            random.NextBytes(mbc.Memory);
            return mbc;
        }
    }

    public class FakeMBC : IMBC
    {
        public byte[] Memory = new byte[ushort.MaxValue + 1];

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

//General Memory Map
//  0000-3FFF   16KB ROM Bank 00     (in cartridge, fixed at bank 00)
//  4000 - 7FFF   16KB ROM Bank 01..NN(in cartridge, switchable bank number)
//  8000 - 9FFF   8KB Video RAM(VRAM)(switchable bank 0 - 1 in CGB Mode)
//  A000 - BFFF   8KB External RAM(in cartridge, switchable bank, if any)
//  C000 - CFFF   4KB Work RAM Bank 0(WRAM)
//  D000 - DFFF   4KB Work RAM Bank 1(WRAM)(switchable bank 1 - 7 in CGB Mode)
//  E000 - FDFF   Same as C000 - DDFF(ECHO)(typically not used)
//  FE00 - FE9F   Sprite Attribute Table(OAM)
//  FEA0 - FEFF   Not Usable
//  FF00 - FF7F   I / O Ports
//  FF80 - FFFE   High RAM(HRAM)
//  FFFF        Interrupt Enable Register