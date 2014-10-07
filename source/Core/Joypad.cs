namespace Core
{
    public class Joypad : IJoypad
    {
        private byte _p1;
        public byte P1
        {
            get { return _p1; }
            set { _p1 = (byte) (value & 0xF0); }
        }
    }
}