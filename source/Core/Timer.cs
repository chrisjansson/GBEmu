namespace Core
{
    public class Timer
    {
        public byte TIMA; //timer counter
        public byte TMA; //timer modulo
        public byte TAC;

        public Timer(IMmu fakeMmu)
        {
        }

        private int _ticks;
        public void Tick()
        {
            _ticks++;
            if (_ticks == 256)
            {
                TIMA++;
                if (TIMA == 0)
                {
                    TIMA = TMA;
                }
                _ticks = 0;
            }
        }
    }
}