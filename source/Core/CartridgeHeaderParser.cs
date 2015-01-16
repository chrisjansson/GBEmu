using System;
using System.ComponentModel;

namespace Core
{
    public class CartridgeHeaderParser
    {
        public CartridgeHeader Parse(byte[] header)
        {
            var cartridgeTypeValue = header[0x47];
            if(!typeof (CartridgeHeader.CartridgeType).IsEnumDefined((int)cartridgeTypeValue))
                throw new NotSupportedException();

            return new CartridgeHeader((CartridgeHeader.CartridgeType) cartridgeTypeValue);
        }
    }

    public class CartridgeHeader
    {
        public CartridgeHeader(CartridgeType mbc)
        {
            MBC = mbc;
        }

        public CartridgeType MBC { get; private set; }

        public enum CartridgeType
        {
            None,
            MBC1,
            MBC1RAM,
            MBC1RAMBATTERY,
            MBC2 = 0x05,
            MBC2BATTERY,
            ROMRAM = 0x08,
            ROMRAMBATTERY
        }
    }
}