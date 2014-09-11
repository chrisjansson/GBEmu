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
                _ticks = 0;
            }
        }
    }
}