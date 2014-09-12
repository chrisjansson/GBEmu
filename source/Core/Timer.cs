using System;

namespace Core
{
    public class Timer
    {
        private readonly IMmu _mmu;
        private int _ticks;

        public Timer(IMmu mmu)
        {
            _mmu = mmu;
        }

        public byte TIMA; //timer counter
        public byte TMA; //timer modulo
        public byte TAC; //timer control

        public void Tick()
        {
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