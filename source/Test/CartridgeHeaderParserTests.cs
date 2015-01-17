using System;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class CartridgeHeaderParserTests
    {
        //[Fact]
        //public void Should_parse_title_from_34_to_43()
        //{
        //    var header = new byte[0x4F];
        //    ""

        //    var parser = new CartridgeHeaderParser();

        //    parser.Parse()
        //} 


        [Theory]
        [InlineData(0x80, CartridgeHeader.GameboyTypeEnum.CGBORDMG)]
        [InlineData(0xC0, CartridgeHeader.GameboyTypeEnum.CGB)]
        public void Should_parse_CGB_flag_from_43h(int value, CartridgeHeader.GameboyTypeEnum expectedType)
        {
            var header = new byte[0x4F];
            header[0x43] = (byte)value;

            var parser = new CartridgeHeaderParser();

            var result = parser.Parse(header);
            Assert.Equal(expectedType, result.GameboyType);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(93)]
        public void Should_parse_CGB_flag_from_43h_as_dmg(int value)
        {
            var header = new byte[0x4F];
            header[0x43] = (byte)value;

            var parser = new CartridgeHeaderParser();

            var result = parser.Parse(header);
            Assert.Equal(CartridgeHeader.GameboyTypeEnum.DMG, result.GameboyType);
        }

        [Theory]
        [InlineData(0, CartridgeHeader.CartridgeTypeEnum.None)]
        [InlineData(1, CartridgeHeader.CartridgeTypeEnum.MBC1)]
        [InlineData(2, CartridgeHeader.CartridgeTypeEnum.MBC1RAM)]
        [InlineData(3, CartridgeHeader.CartridgeTypeEnum.MBC1RAMBATTERY)]
        [InlineData(5, CartridgeHeader.CartridgeTypeEnum.MBC2)]
        [InlineData(6, CartridgeHeader.CartridgeTypeEnum.MBC2BATTERY)]
        [InlineData(8, CartridgeHeader.CartridgeTypeEnum.ROMRAM)]
        [InlineData(9, CartridgeHeader.CartridgeTypeEnum.ROMRAMBATTERY)]
        [InlineData(0x0B, CartridgeHeader.CartridgeTypeEnum.MMM01)]
        [InlineData(0x0C, CartridgeHeader.CartridgeTypeEnum.MMM01RAM)]
        [InlineData(0x0D, CartridgeHeader.CartridgeTypeEnum.MMM01RAMBATTERY)]
        [InlineData(0x0F, CartridgeHeader.CartridgeTypeEnum.MBC3TIMERBATTERY)]
        [InlineData(0x10, CartridgeHeader.CartridgeTypeEnum.MBC3TIMERRAMBATTERY)]
        [InlineData(0x11, CartridgeHeader.CartridgeTypeEnum.MBC3)]
        [InlineData(0x12, CartridgeHeader.CartridgeTypeEnum.MBC3RAM)]
        [InlineData(0x13, CartridgeHeader.CartridgeTypeEnum.MBC3RAMBATTERY)]
        [InlineData(0x15, CartridgeHeader.CartridgeTypeEnum.MBC4)]
        [InlineData(0x16, CartridgeHeader.CartridgeTypeEnum.MBC4RAM)]
        [InlineData(0x17, CartridgeHeader.CartridgeTypeEnum.MBC4RAMBATTERY)]
        [InlineData(0x19, CartridgeHeader.CartridgeTypeEnum.MBC5)]
        [InlineData(0x1A, CartridgeHeader.CartridgeTypeEnum.MBC5RAM)]
        [InlineData(0x1B, CartridgeHeader.CartridgeTypeEnum.MBC5RAMBATTERY)]
        [InlineData(0x1C, CartridgeHeader.CartridgeTypeEnum.MBC5RUMBLE)]
        [InlineData(0x1D, CartridgeHeader.CartridgeTypeEnum.MBC5RUMBLERAM)]
        [InlineData(0x1E, CartridgeHeader.CartridgeTypeEnum.MBC5RUMBLERAMBATTERY)]
        [InlineData(0xFC, CartridgeHeader.CartridgeTypeEnum.POCKETCAMERA)]
        [InlineData(0xFD, CartridgeHeader.CartridgeTypeEnum.BANDAITAMA5)]
        [InlineData(0xFE, CartridgeHeader.CartridgeTypeEnum.HUC3)]
        [InlineData(0xFF, CartridgeHeader.CartridgeTypeEnum.HUC1RAMBATTERY)]
        public void Should_parse_cartridge_type_from_47h(int value, CartridgeHeader.CartridgeTypeEnum expectedTypeEnum)
        {
            var header = new byte[0x4F];
            header[0x47] = (byte)value;

            var parser = new CartridgeHeaderParser();

            var result = parser.Parse(header);

            Assert.Equal(expectedTypeEnum, result.MBC);
        }

        [Fact]
        public void Should_not_allow_undefined_cartridge_types()
        {
            var header = new byte[0x4F];
            header[0x47] = 123;

            var parser = new CartridgeHeaderParser();

            Assert.Throws<NotSupportedException>(() => parser.Parse(header));
        }

        [Theory]
        [InlineData(0, CartridgeHeader.RomSizeEnum._32KB)]
        [InlineData(1, CartridgeHeader.RomSizeEnum._64KB)]
        [InlineData(2, CartridgeHeader.RomSizeEnum._128KB)]
        [InlineData(3, CartridgeHeader.RomSizeEnum._256KB)]
        [InlineData(4, CartridgeHeader.RomSizeEnum._512KB)]
        [InlineData(5, CartridgeHeader.RomSizeEnum._1MB)]
        [InlineData(6, CartridgeHeader.RomSizeEnum._2MB)]
        [InlineData(7, CartridgeHeader.RomSizeEnum._4MB)]
        [InlineData(0x52, CartridgeHeader.RomSizeEnum._1_1MB)]
        [InlineData(0x53, CartridgeHeader.RomSizeEnum._1_2MB)]
        [InlineData(0x54, CartridgeHeader.RomSizeEnum._1_5MB)]
        public void Should_parse_rom_size_from_48h(int value, CartridgeHeader.RomSizeEnum expectedSize)
        {
            var header = new byte[0x4F];
            header[0x48] = (byte)value;

            var parser = new CartridgeHeaderParser();

            var result = parser.Parse(header);

            Assert.Equal(expectedSize, result.ROMSize);
        }

        [Fact]
        public void Should_not_support_unknown_rom_size_values()
        {
            var header = new byte[0x4F];
            header[0x48] = 123;

            var parser = new CartridgeHeaderParser();

            Assert.Throws<NotSupportedException>(() => parser.Parse(header));
        }

        [Theory]
        [InlineData(0, CartridgeHeader.RamSizeEnum.None)]
        [InlineData(1, CartridgeHeader.RamSizeEnum._2KB)]
        [InlineData(2, CartridgeHeader.RamSizeEnum._8KB)]
        [InlineData(3, CartridgeHeader.RamSizeEnum._32KB)]
        public void Should_parse_ram_size_from_49h(int value, CartridgeHeader.RamSizeEnum expectedSize)
        {
            var header = new byte[0x4F];
            header[0x49] = (byte)value;

            var parser = new CartridgeHeaderParser();

            var result = parser.Parse(header);

            Assert.Equal(expectedSize, result.RAMSize);
        }

        [Fact]
        public void Should_not_suport_unknown_ram_size_values()
        {
            var header = new byte[0x4F];
            header[0x49] = 123;

            var parser = new CartridgeHeaderParser();

            Assert.Throws<NotSupportedException>(() => parser.Parse(header));
        }
    }
}