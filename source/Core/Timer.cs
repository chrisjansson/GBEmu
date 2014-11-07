using System;

namespace Core
{
    public class Timer
    {
        private readonly IMmu _mmu;
        private int _ticks;
        private int _divTicks;

        public Timer(IMmu mmu)
        {
            _mmu = mmu;
        }

        public byte TIMA; //timer counter
        public byte TMA; //timer modulo
        public byte TAC; //timer control

        private byte _div;
        public byte DIV //Divider
        {
            get { return _div; }
            set { _div = 0; }
        }

        public void Tick()
        {
            _divTicks++;
            if (_divTicks == 64)
            {
                _divTicks = 0;
                _div++;
            }

            if ((TAC & 0x04) == 0)
                return;
            _ticks++;

            int ticksPerCycle;
            switch (TAC & 0x03)
            {
                case 0:
                    ticksPerCycle = 256;
                    break;
                case 1:
                    ticksPerCycle = 4;
                    break;
                case 2:
                    ticksPerCycle = 16;
                    break;
                case 3:
                    ticksPerCycle = 64;
                    break;
                default:
                    throw new InvalidOperationException();
            }

            if (_ticks == ticksPerCycle)
            {
                TIMA++;
                if (TIMA == 0) //overflorw
                {
                    var result = _mmu.GetByte(RegisterAddresses.IF) | 0x04;
                    _mmu.SetByte(RegisterAddresses.IF, (byte)result);
                    TIMA = TMA;
                }
                _ticks = 0;
            }
        }
    }
}