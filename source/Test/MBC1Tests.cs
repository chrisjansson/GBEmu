using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class MBC1Tests
    {
        [Theory]
        [PropertyData("SupportedRomConfigurations", PropertyType = typeof(RomConfigurations))]
        public void Should_accept_rom_size_that_matches_the_configured_rom_size(int kB, CartridgeHeader.RomSizeEnum romSize)
        {
            var rom = new byte[kB * 1024];

            Assert.DoesNotThrow(() => new MBC1(rom, romSize));
        }

        [Theory]
        [PropertyData("SupportedRomConfigurations", PropertyType = typeof(RomConfigurations))]
        public void Should_reject_rom_size_that_does_not_match_the_configured_rom_size(int kB, CartridgeHeader.RomSizeEnum romSize)
        {
            var rom = new byte[kB * 1024 + 1];

            Assert.Throws<InvalidOperationException>(() => new MBC1(rom, romSize));
        }

        [Theory]
        [PropertyData("UnspportedRomConfigurations", PropertyType = typeof(RomConfigurations))]
        public void Should_reject_unsupported_rom_configurations(CartridgeHeader.RomSizeEnum romSize)
        {
            var rom = new byte[32 * 1024];

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
            mbc.SetByte(0x2000, 0xE1);
            mbc.SetByte(0x4000, (byte)memoryBank);

            var offset = 0x4000 * 97; // two lowest bits are bit 5-6 in bank select
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
                .Except(new[] { 0x20, 0x40, 0x60 })
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
            var rom = new byte[2048 * 1024];
            var random = new Random();
            random.NextBytes(rom);

            return rom;
        }

        public class RomConfigurations
        {
            public static IEnumerable<object[]> SupportedRomConfigurations
            {
                get
                {
                    return new[]
                    {
                        new object[] { 32, CartridgeHeader.RomSizeEnum._32KB},
                        new object[] { 64, CartridgeHeader.RomSizeEnum._64KB},
                        new object[] { 128, CartridgeHeader.RomSizeEnum._128KB},
                        new object[] { 256, CartridgeHeader.RomSizeEnum._256KB},
                        new object[] { 512, CartridgeHeader.RomSizeEnum._512KB},
                        new object[] { 1024, CartridgeHeader.RomSizeEnum._1MB},
                        new object[] { 2048, CartridgeHeader.RomSizeEnum._2MB},
                    };
                }
            }

            public static IEnumerable<object[]> UnspportedRomConfigurations
            {
                get
                {
                    var supportedRomSizes = SupportedRomConfigurations
                        .Select(x => (CartridgeHeader.RomSizeEnum)x[1]);
                    return typeof(CartridgeHeader.RomSizeEnum)
                        .GetEnumValues()
                        .Cast<CartridgeHeader.RomSizeEnum>()
                        .Except(supportedRomSizes)
                        .Select(x => new object[] { x });
                }
            }
        }
    }
}