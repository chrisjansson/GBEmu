using System;
using Core;
using Gui;
using Xunit;

namespace Test
{
    public class NoMBCTests
    {
        private int _romSize = 0x8000;

        [Fact]
        public void Should_not_allow_oversized_roms()
        {
            var rom = new byte[_romSize + 1];

            Assert.Throws<NotSupportedException>(() => new NoMBC(rom));
        }

        [Fact]
        public void Should_not_allow_undersized_roms()
        {
            var rom = new byte[_romSize - 1];

            Assert.Throws<NotSupportedException>(() => new NoMBC(rom));
        }

        [Fact]
        public void Should_expose_rom_from_0000_to_7FFF()
        {
            var rom = CreateFakeRom();

            var mc = new NoMBC(rom);

            for (var i = 0; i < _romSize; i++)
            {
                var data = mc.GetByte((ushort)i);
                var expected = rom[i];
                Assert.Equal(expected, data);
            }
        }

        [Fact]
        public void Should_not_be_able_to_write_to_rom()
        {
            var rom = CreateFakeRom();
            var mc = new NoMBC(rom);
            var random = new Random();

            for (var i = 0; i < _romSize; i++)
            {
                mc.SetByte((ushort) i, (byte) random.Next());
            }

            for (var i = 0; i < _romSize; i++)
            {
                var data = mc.GetByte((ushort)i);
                var expected = rom[i];
                Assert.Equal(expected, data);
            }
        }

        [Fact]
        public void Writing_to_RAM_is_always_0()
        {
            var rom = CreateFakeRom();
            var mc = new NoMBC(rom);
            var random = new Random();

            for (var i = 0xA000; i < 0xC000; i++)
            {
                mc.SetByte((ushort) i, (byte) random.Next());
                var actual = mc.GetByte((ushort) i);
                Assert.Equal(0, actual);
            }
        }

        private byte[] CreateFakeRom()
        {
            var random = new Random();
            var rom = new byte[_romSize];
            random.NextBytes(rom);
            return rom;
        }
    }
}