namespace Core
{
    public class Joypad : IJoypad
    {
        private byte _p1;
        public byte P1
        {
            get
            {
                var buttons = 0x00;
                if ((_p1 & 0x20) == 0x20)
                {
                    buttons = buttons | (A ? 0x01 : 0x00);
                    buttons = buttons | (B ? 0x02 : 0x00);
                    buttons = buttons | (Select ? 0x04 : 0x00);
                    buttons = buttons | (Start ? 0x08 : 0x00);
                }
                else
                {
                    buttons = buttons | (Right ? 0x01 : 0x00);
                    buttons = buttons | (Left ? 0x02 : 0x00);
                    buttons = buttons | (Up ? 0x04 : 0x00);
                    buttons = buttons | (Down ? 0x08 : 0x00);
                }
                int result = _p1;
                result = result | (~buttons & 0x0F);
                return (byte)result;
            }
            set { _p1 = (byte)(value & 0xF0); }
        }

        public bool Right { get; set; }
        public bool Left { get; set; }
        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool A { get; set; }
        public bool B { get; set; }
        public bool Select { get; set; }
        public bool Start { get; set; }
    }
}