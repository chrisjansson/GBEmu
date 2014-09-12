using System;

namespace Core
{
    public class Timer
    {
        public byte TIMA; //timer counter
        public byte TMA; //timer modulo
        public byte TAC;

        public Timer(IMmu mmu)
        {
            _mmu = mmu;
        }

        private int _ticks;
        private IMmu _mmu;

        public void Tick()
        {
            if ((TAC & 0x04) == 0)
                return;
            _ticks++;

            int ticksPerCycle;
            switch (TAC & 0x01)
            {
                case 0:
                    ticksPerCycle = 256;
                    break;
                case 1:
                    ticksPerCycle = 4;
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