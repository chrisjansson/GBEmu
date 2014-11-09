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

        public byte LCDC { get; set; }

        private byte _coincidenceInterrupt;
        private byte _hblankInterrupt;
        private byte _vblankInterrupt;
        private byte _oamInterrupt;
        private int _lcdcRest;

        public byte STAT
        {
            get
            {
                var result = _lcdcRest;
                result = result | _mode;
                result = result | ((LYC == Line) ? 0x4 : 0);
                result = result | (_coincidenceInterrupt << 6);
                result = result | (_hblankInterrupt << 3);
                result = result | (_vblankInterrupt << 4);
                result = result | (_oamInterrupt << 5);
                return (byte)result;
            }
            set
            {
                _coincidenceInterrupt = (byte)((value >> 6) & 1);
                _hblankInterrupt = (byte)((value >> 3) & 1);
                _vblankInterrupt = (byte)((value >> 4) & 1);
                _oamInterrupt = (byte)((value >> 5) & 1);
                _lcdcRest = value & 0x80;
            }
        }

        private byte _dma;
        public byte DMA
        {
            get { return _dma; }
            set
            {
                _dma = value;
                _dmaCycles = 168;
            }
        }

        private int _dmaCycles;
        public int DMACycles
        {
            get { return _dmaCycles; }
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
            DMATransfer();

            if (_mode == 0 && _clock == HorizontalBlankingTime)
            {
                _clock = 0;
                Line++;

                if (Line == NumberOfLines)
                {
                    _mode = 1;
                    VBlank();
                }
                else
                {
                    _mode = 2;
                    OAM();
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
                HBlank();
            }
        }

        private void DMATransfer()
        {
            if (_dmaCycles > 0)
            {
                var offset = 168 - _dmaCycles;
                if (offset < 160)
                {
                    var source = ((_dma << 8) + offset);
                    var target = 0xFE00 + offset;
                    var value = _mmu.GetByte((ushort) source);
                    _mmu.SetByte((ushort) target, value);
                }
                _dmaCycles--;
            }
        }

        private void VBlank()
        {
            var newIf = _mmu.GetByte(RegisterAddresses.IF) | 0x01;
            if (_vblankInterrupt == 1)
            {
                newIf = newIf | 0x02;
            }
            _mmu.SetByte(RegisterAddresses.IF, (byte)newIf);
            _displayDataTransferService.FinishFrame();
        }

        private void OAM()
        {
            if (_oamInterrupt == 0)
            {
                return;
            }
            var newIf = _mmu.GetByte(RegisterAddresses.IF) | 0x02;
            _mmu.SetByte(RegisterAddresses.IF, (byte)newIf);
        }

        private void HBlank()
        {
            _displayDataTransferService.TransferScanLine(Line);
            if (_hblankInterrupt == 0)
            {
                return;
            }
            var newIf = _mmu.GetByte(RegisterAddresses.IF) | 0x02;
            _mmu.SetByte(RegisterAddresses.IF, (byte)newIf);
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