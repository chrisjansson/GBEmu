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