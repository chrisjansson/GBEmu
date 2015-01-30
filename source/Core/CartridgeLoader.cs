using System;

namespace Core
{
    public class CartridgeLoader
    {
        public IMBC Load(byte[] rom)
        {
            var headerBytes = new byte[0x4F];
            Array.Copy(rom, 0x100, headerBytes, 0, headerBytes.Length);
            var header = new CartridgeHeaderParser().Parse(headerBytes);

            switch (header.MBC)
            {
                case CartridgeHeader.CartridgeTypeEnum.MBC1RAMBATTERY:
                    return new MBC1(rom, header.ROMSize, header.RAMSize);
            }

            return null;
        }
    }
}