namespace Core
{
    //0xFF40 - LCDC - LCD Control
    //0xFF41 - STAT - LCDC Status
    //0xFF42 - SCY - Scroll Y (R/W)
    //0xFF43 - SCX - Scroll X (R/W)
    //0xFF44 - LY - LCDC Y-Coordinate (R)
    //0xFF45 - LYC - LY Compare (R/W)
    //0xFF4A - WY - Window Y Position (R/W)
    //0xFF4B - WX - Window X Position minus 7 (R/W)
    public class Display
    {
        private int _mode = 2;
        private int _clock = 0;

        public int Mode { get { return _mode; }}

        public void Tick()
        {
            _clock++;

            if (_clock == 51 && Mode == 0)
            {
                _clock = 0;
                _mode = 2;
            }

            if (_clock == 20 && Mode == 2)
            {
                _mode = 3;
                _clock = 0;
            }

            if (_clock == 43 && Mode == 3)
            {
                _mode = 0;
                _clock = 0;
            }
        }
    }
}