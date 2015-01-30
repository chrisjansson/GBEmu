using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class CartridgeLoaderTests
    {
        private static IMBC LoadCartridge(byte[] rom)
        {
            var loader = new CartridgeLoader();
            return loader.Load(rom);
        }

        public class WhenLoadingAnMBC1Cartridge
        {
            public class WhenLoadingDifferentConfigurationsOfMBC1Cartridges
            {
                [Fact]
                public void Loads_MBC1_cartridge()
                {
                    AssertThatMBC1VariantIsLoadedAsMBC1(CartridgeHeader.CartridgeTypeEnum.MBC1);
                }

                [Fact]
                public void Loads_MBC1_cartridge_with_ram()
                {
                    AssertThatMBC1VariantIsLoadedAsMBC1(CartridgeHeader.CartridgeTypeEnum.MBC1RAM);
                }

                [Fact]
                public void Loads_MBC1_cartridge_with_ram_and_battery()
                {
                    AssertThatMBC1VariantIsLoadedAsMBC1(CartridgeHeader.CartridgeTypeEnum.MBC1RAMBATTERY);
                }

                private static void AssertThatMBC1VariantIsLoadedAsMBC1(CartridgeHeader.CartridgeTypeEnum cartridgeTypeEnum)
                {
                    var rom = CreateFakeRom(
                        CartridgeHeader.RomSizeEnum._2MB,
                        CartridgeHeader.RamSizeEnum.None,
                        cartridgeTypeEnum);

                    var mbc = LoadCartridge(rom);

                    Assert.IsType<MBC1>(mbc);
                }
            }

            public class WhenAccessingRam
            {
                public class WhenSwitchingRamBankInRamMode
                {
                    [Fact]
                    public void Selects_active_ram_bank_from_4000_to_5FFF()
                    {
                        var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._32KB);
                        var ramContent = CreateFakeRamContent();
                        var mbc = LoadCartridge(rom);
                        mbc.SetByte(0x0000, 0xA);
                        mbc.SetByte(0x6000, 1);

                        WriteToRamBank(mbc, 0, ramContent);
                        WriteToRamBank(mbc, 1, ramContent);
                        WriteToRamBank(mbc, 2, ramContent);
                        WriteToRamBank(mbc, 3, ramContent);

                        AssertBankContains(mbc, 0, ramContent);
                        AssertBankContains(mbc, 1, ramContent);
                        AssertBankContains(mbc, 2, ramContent);
                        AssertBankContains(mbc, 3, ramContent);
                    }

                    [Fact]
                    public void Only_uses_lower_rom_bank_selection_when_ram_mode_is_selected()
                    {
                        var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._32KB);
                        var mbc = LoadCartridge(rom);
                        mbc.SetByte(0x0000, 0xA);
                        mbc.SetByte(0x6000, 1);

                        mbc.SetByte(0x2000, 0xFF); //select bank 0x1F
                        mbc.SetByte(0x4000, 0x03); //selects ram bank in this mode

                        var assertion = new MBCAssertion(mbc, rom);
                        assertion.AssertRangeIsMapped(0x4000, 0x7FFF, 0x1F * 0x4000);
                    }
                }

                public class WhenSwitchingRamBankInRomMode
                {
                    [Fact]
                    public void Always_read_from_ram_bank_0_when_rom_mode_is_selected()
                    {
                        var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._32KB);
                        var ramContent = CreateFakeRamContent();
                        var mbc = LoadCartridge(rom);
                        mbc.SetByte(0x0000, 0xA);
                        mbc.SetByte(0x6000, 1);
                        WriteToRamBank(mbc, 0, ramContent);
                        WriteToRamBank(mbc, 1, ramContent);

                        mbc.SetByte(0x4000, 1);
                        mbc.SetByte(0x6000, 0);

                        var assertion = new MBCAssertion(mbc, ramContent[0]);
                        assertion.AssertRangeIsMapped(0xA000, 0xBFFF);
                    }

                    [Fact]
                    public void Always_write_to_ram_bank_0_when_rom_mode_is_selected()
                    {
                        var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._32KB);
                        var ramContent = CreateFakeRamContent();
                        var mbc = LoadCartridge(rom);
                        mbc.SetByte(0x0000, 0xA);
                        mbc.SetByte(0x6000, 0);
                        WriteToRamBank(mbc, 1, ramContent);

                        mbc.SetByte(0x6000, 1);

                        mbc.SetByte(0x4000, 0);
                        var assertion = new MBCAssertion(mbc, ramContent[1]);
                        assertion.AssertRangeIsMapped(0xA000, 0xBFFF);
                    }

                    [Fact]
                    public void Bank_1_is_empty_after_writing_to_bank_1_in_rom_mode()
                    {
                        var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._32KB);
                        var ramContent = CreateFakeRamContent();
                        var mbc = LoadCartridge(rom);
                        mbc.SetByte(0x0000, 0xA);
                        mbc.SetByte(0x6000, 0);
                        WriteToRamBank(mbc, 1, ramContent);

                        mbc.SetByte(0x6000, 1);

                        mbc.SetByte(0x4000, 1);
                        var assertion = new MBCAssertion(mbc, new byte[0x2000]);
                        assertion.AssertRangeIsMapped(0xA000, 0xBFFF);
                    }
                }

                [Fact]
                public void Should_read_and_write_A000_BFFF_to_RAM()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._8KB);
                    var mbc = LoadCartridge(rom);
                    mbc.SetByte(0x0000, 0xFA);

                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsReadWrite(0xA000, 0xBFFF);
                }

                [Fact]
                public void Should_always_return_zero_when_ram_is_disabled()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._8KB);
                    var mbc = LoadCartridge(rom);

                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsNotMapped(0xA000, 0xBFFF);
                }

                [Fact]
                public void Should_not_write_to_ram_when_ram_is_disabled()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum._8KB);
                    var mbc = LoadCartridge(rom);

                    mbc.SetByte(0xA000, 123);
                    mbc.SetByte(0x000, 0xFA);

                    Assert.Equal(0, mbc.GetByte(0xA000));
                }

                [Fact]
                public void Should_always_return_zero_when_there_is_no_ram()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);
                    mbc.SetByte(0x0000, 0xFA);

                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsNotMapped(0xA000, 0xBFFF);
                }

                private static void AssertBankContains(IMBC mbc, int bank, byte[][] ramContent)
                {
                    var assertion = new MBCAssertion(mbc, ramContent[bank]);
                    mbc.SetByte(0x4000, (byte)bank);
                    assertion.AssertRangeIsMapped(0xA000, 0xBFFF);
                }

                private static void WriteToRamBank(IMBC mbc, int ramBank, byte[][] ram)
                {
                    mbc.SetByte(0x4000, (byte)ramBank);

                    for (int i = 0; i < 0x2000; i++)
                    {
                        mbc.SetByte((ushort)(0xA000 + i), ram[ramBank][i]);
                    }
                }

                private static byte[][] CreateFakeRamContent()
                {
                    var fakeRom = CreateFakeRom(CartridgeHeader.RomSizeEnum._32KB, CartridgeHeader.RamSizeEnum._32KB);
                    return new[]
                    {
                        fakeRom.Take(0x2000).ToArray(),
                        fakeRom.Skip(0x2000).Take(0x2000).ToArray(),
                        fakeRom.Skip(0x4000).Take(0x6000).ToArray(),
                        fakeRom.Skip(0x6000).Take(0x8000).ToArray(),
                    };
                }
            }

            public class WhenAccessingRom
            {
                [Theory]
                [PropertyData("SupportedRomConfigurations", PropertyType = typeof(RomConfigurations))]
                public void Should_accept_rom_size_that_matches_the_configured_rom_size(int kB, CartridgeHeader.RomSizeEnum romSize)
                {
                    var rom = new byte[kB * 1024];

                    Assert.DoesNotThrow(() => new MBC1(rom, romSize, CartridgeHeader.RamSizeEnum.None));
                }

                [Theory]
                [PropertyData("SupportedRomConfigurations", PropertyType = typeof(RomConfigurations))]
                public void Should_reject_rom_size_that_does_not_match_the_configured_rom_size(int kB, CartridgeHeader.RomSizeEnum romSize)
                {
                    var rom = new byte[kB * 1024 + 1];

                    Assert.Throws<InvalidOperationException>(() => new MBC1(rom, romSize, CartridgeHeader.RamSizeEnum.None));
                }

                [Theory]
                [PropertyData("UnspportedRomConfigurations", PropertyType = typeof(RomConfigurations))]
                public void Should_reject_unsupported_rom_configurations(CartridgeHeader.RomSizeEnum romSize)
                {
                    var rom = new byte[32 * 1024];

                    Assert.Throws<InvalidOperationException>(() => new MBC1(rom, romSize, CartridgeHeader.RamSizeEnum.None));
                }

                [Fact]
                public void Should_read_0000_to_3FFF_from_rom()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsMapped(0, 0x3FFF);
                }

                [Fact]
                public void Should_not_persist_writes_to_ROM_bank_0()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsReadOnly(0, 0x3FFF);
                }

                [Fact]
                public void Should_map_selected_rom_bank_to_4000_7FFF()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

                    var memoryBank = 15;
                    mbc.SetByte(0x2000, (byte)memoryBank);

                    var offset = 0x4000 * memoryBank;
                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
                }

                [Fact]
                public void Should_only_use_lower_5bits_from_lower_bank_select()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

                    var memoryBank = 15 + 224;
                    mbc.SetByte(0x2000, (byte)memoryBank);

                    var offset = 0x4000 * 15; //lower 4 bits
                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
                }

                [Fact]
                public void Should_use_upper_2_bits_from_upper_bank_select()
                {
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

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
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

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
                        var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                        var mbc = LoadCartridge(rom);

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
                    var rom = CreateFakeRom(CartridgeHeader.RomSizeEnum._2MB, CartridgeHeader.RamSizeEnum.None);
                    var mbc = LoadCartridge(rom);

                    SelectMemoryBank(mbc, bankNumber);

                    var offset = 0x4000 * (bankNumber + 1);
                    var assertion = new MBCAssertion(mbc, rom);
                    assertion.AssertRangeIsMapped(0x4000, 0x7FFF, romOffset: offset);
                }

                private static void SelectMemoryBank(IMBC mbc, int memoryBank)
                {
                    mbc.SetByte(0x2000, (byte)memoryBank);
                    mbc.SetByte(0x4000, (byte)(memoryBank >> 5));
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

            private static byte[] CreateFakeRom(
                CartridgeHeader.RomSizeEnum romSize,
                CartridgeHeader.RamSizeEnum ramSize,
                CartridgeHeader.CartridgeTypeEnum cartridgeType = CartridgeHeader.CartridgeTypeEnum.MBC1RAMBATTERY)
            {
                var rom = new byte[2048 * 1024];
                var random = new Random();
                random.NextBytes(rom);

                var cartridgeTypeOffset = 0x147;
                rom[cartridgeTypeOffset] = (byte)cartridgeType;

                var romSizeOffset = 0x148;
                rom[romSizeOffset] = (byte)romSize;

                var ramSizeOffset = 0x149;
                rom[ramSizeOffset] = (byte)ramSize;

                return rom;
            }
        }
    }
}