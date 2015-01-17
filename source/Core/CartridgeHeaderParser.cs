using System;
using System.ComponentModel;

namespace Core
{
    public class CartridgeHeaderParser
    {
        public CartridgeHeader Parse(byte[] header)
        {
            var cartridgeTypeValue = header[0x47];
            if (!typeof(CartridgeHeader.CartridgeTypeEnum).IsEnumDefined((int)cartridgeTypeValue))
                throw new NotSupportedException();

            var romSizeValue = header[0x48];
            if (!typeof(CartridgeHeader.RomSizeEnum).IsEnumDefined((int)romSizeValue))
                throw new NotSupportedException();

            var ramSizeValue = header[0x49];
            if (!typeof(CartridgeHeader.RamSizeEnum).IsEnumDefined((int)ramSizeValue))
                throw new NotSupportedException();

            var gameboyTypeValue = header[0x43];
            var gameboyType = (gameboyTypeValue == 0x80 || gameboyTypeValue == 0xC0)
                ? (CartridgeHeader.GameboyTypeEnum)gameboyTypeValue
                : CartridgeHeader.GameboyTypeEnum.DMG;

            return new CartridgeHeader(
                (CartridgeHeader.CartridgeTypeEnum) cartridgeTypeValue,
                (CartridgeHeader.RomSizeEnum) romSizeValue,
                (CartridgeHeader.RamSizeEnum) ramSizeValue,
                gameboyType);

        }
    }

    public class CartridgeHeader
    {
        public CartridgeHeader(CartridgeTypeEnum mbc, RomSizeEnum romSize, RamSizeEnum ramSize, GameboyTypeEnum gameboyType)
        {
            MBC = mbc;
            ROMSize = romSize;
            RAMSize = ramSize;
            GameboyType = gameboyType;
        }

        public CartridgeTypeEnum MBC { get; private set; }
        public RomSizeEnum ROMSize { get; private set; }
        public RamSizeEnum RAMSize { get; private set; }
        public GameboyTypeEnum GameboyType { get; private set; }

        public enum GameboyTypeEnum
        {
            //DMG,
            CGBORDMG = 0x80,
            CGB = 0xC0,
            DMG
        }

        public enum RamSizeEnum
        {
            None,
            _2KB,
            _8KB,
            _32KB
        }

        public enum RomSizeEnum
        {
            _32KB,
            _64KB,
            _128KB,
            _256KB,
            _512KB,
            _1MB,
            _2MB,
            _4MB,
            _1_1MB = 0x52,
            _1_2MB,
            _1_5MB
        }

        public enum CartridgeTypeEnum
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