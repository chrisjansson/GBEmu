using System;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class MBC1Tests
    {

        //        0148 - ROM Size
        //Specifies the ROM Size of the cartridge.Typically calculated as "32KB shl N".
        //  00h -  32KByte(no ROM banking)
        //  01h -  64KByte(4 banks)
        //  02h - 128KByte(8 banks)
        //  03h - 256KByte(16 banks)
        //  04h - 512KByte(32 banks)
        //  05h -   1MByte(64 banks)  - only 63 banks used by MBC1
        //  06h -   2MByte(128 banks) - only 125 banks used by MBC1
        //  07h -   4MByte(256 banks)
        //  52h - 1.1MByte(72 banks)
        //  53h - 1.2MByte(80 banks)
        //  54h - 1.5MByte(96 banks)
        [Theory]
        [InlineData(32, CartridgeHeader.RomSizeEnum._32KB)]
        [InlineData(64, CartridgeHeader.RomSizeEnum._64KB)]
        [InlineData(128, CartridgeHeader.RomSizeEnum._128KB)]
        [InlineData(256, CartridgeHeader.RomSizeEnum._256KB)]
        [InlineData(512, CartridgeHeader.RomSizeEnum._512KB)]
        [InlineData(1008, CartridgeHeader.RomSizeEnum._1MB)]
        [InlineData(2000, CartridgeHeader.RomSizeEnum._2MB)]
        public void Should_accept_rom_size_that_matches_the_configured_rom_size(int kB, CartridgeHeader.RomSizeEnum romSize)
        {
            var rom = new byte[kB * 1024];

            Assert.DoesNotThrow(() => new MBC1(rom, romSize));
        }

        [Theory]
        [InlineData(32, CartridgeHeader.RomSizeEnum._32KB)]
        [InlineData(64, CartridgeHeader.RomSizeEnum._64KB)]
        [InlineData(128, CartridgeHeader.RomSizeEnum._128KB)]
        [InlineData(256, CartridgeHeader.RomSizeEnum._256KB)]
        [InlineData(512, CartridgeHeader.RomSizeEnum._512KB)]
        [InlineData(1008, CartridgeHeader.RomSizeEnum._1MB)]
        [InlineData(2000, CartridgeHeader.RomSizeEnum._2MB)]
        public void Should_reject_rom_size_that_does_not_match_the_configured_rom_size(int kB, CartridgeHeader.RomSizeEnum romSize)
        {
            var rom = new byte[kB * 1024 + 1];

            Assert.Throws<InvalidOperationException>(() => new MBC1(rom, romSize));
        }

        [Fact]
        public void Should_read_0000_to_3FFF_from_rom()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsMapped(0, 0x3FFF);
        }

        [Fact]
        public void Should_not_persist_writes_to_ROM_bank_0()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsReadOnly(0, 0x3FFF);
        }

        [Fact]
        public void Should_always_return_zero_when_ram_is_disabled()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsNotMapped(0xA000, 0xBFFF);
        }

        [Fact]
        public void Should_map_selected_rom_bank_to_4000_7FFF()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var memoryBank = 15;
            mbc.SetByte(0x2000, (byte)memoryBank);

            var offset = 0x4000 * memoryBank;
            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
        }

        [Fact]
        public void Should_only_use_lower_5bits_from_lower_bank_select()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var memoryBank = 15 + 224;
            mbc.SetByte(0x2000, (byte)memoryBank);

            var offset = 0x4000 * 15; //lower 4 bits
            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
        }

        [Fact]
        public void Should_use_upper_2_bits_from_upper_bank_select()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var memoryBank = 3 + 252;
            mbc.SetByte(0x4000, (byte)memoryBank);

            var offset = 0x4000 * 96; // two lowest bits are bit 5-6 in bank select
            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
        }

        [Fact]
        public void Should_combine_upper_and_lower_bank_select()
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            var memoryBank = 122;
            SelectMemoryBank(mbc, memoryBank);

            var offset = 0x4000 * memoryBank; // two lowest bits are bit 5-6 in bank select
            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
        }

        [Fact]
        public void Should_be_able_to_map_all_rom_banks_to_4000_7FFF()
        {
            var memoryBanks = Enumerable.Range(1, 127)
                .Except(new[] { 20, 40, 60 })
                .ToList();
            Assert.Equal(124, memoryBanks.Count);

            foreach (var memoryBank in memoryBanks)
            {
                var rom = CreateFakeRom();
                var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

                SelectMemoryBank(mbc, memoryBank);

                var offset = 0x4000 * memoryBank;
                var assertion = new MBCAssertion(mbc, rom);
                assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0x20)]
        [InlineData(0x40)]
        [InlineData(0x60)]
        public void Should_automatically_select_next_bank_for_special_bank_number(int bankNumber)
        {
            var rom = CreateFakeRom();
            var mbc = new MBC1(rom, CartridgeHeader.RomSizeEnum._2MB);

            SelectMemoryBank(mbc, bankNumber);

            var offset = 0x4000 * (bankNumber + 1);
            var assertion = new MBCAssertion(mbc, rom);
            assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
        }

        private static void SelectMemoryBank(MBC1 mbc, int memoryBank)
        {
            mbc.SetByte(0x2000, (byte)memoryBank);
            mbc.SetByte(0x4000, (byte)(memoryBank >> 5));
        }

        private byte[] CreateFakeRom()
        {
            var rom = new byte[0x8000 * 128];
            var random = new Random();
            random.NextBytes(rom);

            return rom;
        }
    }
}