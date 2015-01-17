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
        [InlineData(0, CartridgeHeader.CartridgeType.None)]
        [InlineData(1, CartridgeHeader.CartridgeType.MBC1)]
        [InlineData(2, CartridgeHeader.CartridgeType.MBC1RAM)]
        [InlineData(3, CartridgeHeader.CartridgeType.MBC1RAMBATTERY)]
        [InlineData(5, CartridgeHeader.CartridgeType.MBC2)]
        [InlineData(6, CartridgeHeader.CartridgeType.MBC2BATTERY)]
        [InlineData(8, CartridgeHeader.CartridgeType.ROMRAM)]
        [InlineData(9, CartridgeHeader.CartridgeType.ROMRAMBATTERY)]
        [InlineData(0x0B, CartridgeHeader.CartridgeType.MMM01)]
        [InlineData(0x0C, CartridgeHeader.CartridgeType.MMM01RAM)]
        [InlineData(0x0D, CartridgeHeader.CartridgeType.MMM01RAMBATTERY)]
        [InlineData(0x0F, CartridgeHeader.CartridgeType.MBC3TIMERBATTERY)]
        [InlineData(0x10, CartridgeHeader.CartridgeType.MBC3TIMERRAMBATTERY)]
        [InlineData(0x11, CartridgeHeader.CartridgeType.MBC3)]
        [InlineData(0x12, CartridgeHeader.CartridgeType.MBC3RAM)]
        [InlineData(0x13, CartridgeHeader.CartridgeType.MBC3RAMBATTERY)]
        [InlineData(0x15, CartridgeHeader.CartridgeType.MBC4)]
        [InlineData(0x16, CartridgeHeader.CartridgeType.MBC4RAM)]
        [InlineData(0x17, CartridgeHeader.CartridgeType.MBC4RAMBATTERY)]
        [InlineData(0x19, CartridgeHeader.CartridgeType.MBC5)]
        [InlineData(0x1A, CartridgeHeader.CartridgeType.MBC5RAM)]
        [InlineData(0x1B, CartridgeHeader.CartridgeType.MBC5RAMBATTERY)]
        [InlineData(0x1C, CartridgeHeader.CartridgeType.MBC5RUMBLE)]
        [InlineData(0x1D, CartridgeHeader.CartridgeType.MBC5RUMBLERAM)]
        [InlineData(0x1E, CartridgeHeader.CartridgeType.MBC5RUMBLERAMBATTERY)]
        [InlineData(0xFC, CartridgeHeader.CartridgeType.POCKETCAMERA)]
        [InlineData(0xFD, CartridgeHeader.CartridgeType.BANDAITAMA5)]
        [InlineData(0xFE, CartridgeHeader.CartridgeType.HUC3)]
        [InlineData(0xFF, CartridgeHeader.CartridgeType.HUC1RAMBATTERY)]
        public void Should_parse_cartridge_type_from_47h(int value, CartridgeHeader.CartridgeType expectedType)
        {
            var header = new byte[0x4F];
            header[0x47] = (byte)value;

            var parser = new CartridgeHeaderParser();

            var result = parser.Parse(header);

            Assert.Equal(expectedType, result.MBC);
        }

        [Fact]
        public void Should_not_allow_undefined_cartridge_types()
        {
            var header = new byte[0x4F];
            header[0x47] = 123;

            var parser = new CartridgeHeaderParser();

            Assert.Throws<NotSupportedException>(() => parser.Parse(header));
        }
    }
}