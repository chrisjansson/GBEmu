namespace Core
{
    public static class RamSizeExtensions
    {
        public static int ToKB(this CartridgeHeader.RamSizeEnum ramSize)
        {
            switch (ramSize)
            {
                case CartridgeHeader.RamSizeEnum._8KB:
                    return 8 * 1024;
                case CartridgeHeader.RamSizeEnum._2KB:
                    return 2 * 1024;
                case CartridgeHeader.RamSizeEnum._32KB:
                    return 32 * 1024;
                default:
                    return 0;
            }
        }
    }
}