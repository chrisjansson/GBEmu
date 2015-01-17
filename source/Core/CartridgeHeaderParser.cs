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
            ROMRAMBATTERY,
            MMM01 = 0x0B,
            MMM01RAM,
            MMM01RAMBATTERY,
            MBC3TIMERBATTERY = 0x0F,
            MBC3TIMERRAMBATTERY,
            MBC3,
            MBC3RAM,
            MBC3RAMBATTERY,
            MBC4 = 0x15,
            MBC4RAM,
            MBC4RAMBATTERY,
            MBC5 = 0x19,
            MBC5RAM,
            MBC5RAMBATTERY,
            MBC5RUMBLE,
            MBC5RUMBLERAM,
            MBC5RUMBLERAMBATTERY,
            POCKETCAMERA = 0xFC,
            BANDAITAMA5,
            HUC3,
            HUC1RAMBATTERY
        }
    }
}