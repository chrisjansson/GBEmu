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
        private int _mode = 2;
        private int _clock;
        private readonly IDisplayDataTransferService _displayDataTransferService;

        public Display(IDisplayDataTransferService displayDataTransferService)
        {
            _displayDataTransferService = displayDataTransferService;
        }

        public int Mode { get { return _mode; } }
        public byte BackgroundPaletteData { get; set; }
        public byte Line { get; set; }
        public byte LCDC { get; set; }

        private const int HorizontalBlankingTime = 51;
        private const int VerticalBlankingTime = 114;
        private const int OAMScanningTime = 20;
        private const int TransferTime = 43;
        private const int NumberOfLines = 144;
        private const int NumberOfVerticalBlankingLines = 10;

        public void Tick()
        {
            _clock++;

            if (Mode == 0 && _clock == HorizontalBlankingTime)
            {
                _clock = 0;
                Line++;

                if (Line == NumberOfLines)
                {
                    _mode = 1;
                    _displayDataTransferService.FinishFrame();
                }
                else
                {
                    _mode = 2;
                }
            }
            else if (Mode == 1 && _clock == VerticalBlankingTime)
            {
                Line++;
                _clock = 0;

                if (Line == NumberOfLines + NumberOfVerticalBlankingLines)
                {
                    Line = 0;
                    _mode = 2;
                }
            }
            else if (Mode == 2 && _clock == OAMScanningTime)
            {
                _mode = 3;
                _clock = 0;
            }
            else if (Mode == 3 && _clock == TransferTime)
            {
                _mode = 0;
                _clock = 0;
                _displayDataTransferService.TransferScanLine(Line);
            }
        }
    }

    public class RegisterAddresses
    {
        public const ushort TAC = 0xFF07; //Timer control
        public const ushort TMA = 0xFF06; //Timer modulo
        public const ushort TIMA = 0xFF05; //Timer counter
        public const ushort LCDC = 0xFF40;
        public const ushort ScrollX = 0xFF43;
        public const ushort ScrollY = 0xFF42;
        public const ushort LY = 0xFF44;
        public const ushort BGP = 0xFF47;
        public const ushort IE = 0xFFFF; //Interrupt enable
        public const ushort IF = 0xFF0F; //Interrupt request
    }
}