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
    public class Display : IDisplay
    {
        private readonly IDisplayDataTransferService _displayDataTransferService;
        private readonly IMmu _mmu;
        private int _mode = 2;
        private int _clock;
        public int LYC;

        public Display(IMmu mmu, IDisplayDataTransferService displayDataTransferService)
        {
            _mmu = mmu;
            _displayDataTransferService = displayDataTransferService;
        }

        public byte BackgroundPaletteData { get; set; }

        private byte _line;
        public byte Line
        {
            get { return _line; }
            set
            {
                _line = value;
                CheckLYCountInterrupt();
            }
        }

        private byte _coincidenceInterrupt;
        private byte _hblankInterrupt;

        private int _lcdcRest;
        public byte LCDC
        {
            get
            {
                var result = _lcdcRest;
                result = result | ((LYC == Line) ? 0x4 : 0);
                result = result | _mode;
                result = result | (_coincidenceInterrupt << 6);
                result = result | (_hblankInterrupt << 3);
                return (byte)result;
            }
            set
            {
                _coincidenceInterrupt = (byte)((value >> 6) & 1);
                _hblankInterrupt = (byte) ((value >> 3) & 1);
                _lcdcRest = value & 0xB0;
            }
        }

        private const int HorizontalBlankingTime = 51;
        private const int VerticalBlankingTime = 114;
        private const int OAMScanningTime = 20;
        private const int TransferTime = 43;
        private const int NumberOfLines = 144;
        private const int NumberOfVerticalBlankingLines = 10;

        public void Tick()
        {
            _clock++;

            if (_mode == 0 && _clock == HorizontalBlankingTime)
            {
                _clock = 0;
                Line++;

                if (Line == NumberOfLines)
                {
                    _mode = 1;
                    var newIf = _mmu.GetByte(RegisterAddresses.IF) | 0x01;
                    _mmu.SetByte(RegisterAddresses.IF, (byte)newIf);
                    _displayDataTransferService.FinishFrame();
                }
                else
                {
                    _mode = 2;
                }
            }
            else if (_mode == 1 && _clock == VerticalBlankingTime)
            {
                Line++;
                _clock = 0;

                if (Line == NumberOfLines + NumberOfVerticalBlankingLines)
                {
                    Line = 0;
                    _mode = 2;
                }
            }
            else if (_mode == 2 && _clock == OAMScanningTime)
            {
                _mode = 3;
                _clock = 0;
            }
            else if (_mode == 3 && _clock == TransferTime)
            {
                _mode = 0;
                _clock = 0;
                _displayDataTransferService.TransferScanLine(Line);
                if (_hblankInterrupt == 0)
                {
                    return;
                }
                var newIf = _mmu.GetByte(RegisterAddresses.IF) | 0x02;
                _mmu.SetByte(RegisterAddresses.IF, (byte)newIf);
            }
        }

        private void CheckLYCountInterrupt()
        {
            if (LYC != Line || _coincidenceInterrupt == 0)
            {
                return;
            }
            var newIf = _mmu.GetByte(RegisterAddresses.IF) | 0x02;
            _mmu.SetByte(RegisterAddresses.IF, (byte)newIf);
        }
    }
}