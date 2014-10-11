namespace Test.TimerTests
{
    public class Timer_16384Hz : TimerTestBase
    {
        protected override byte TimerSelect
        {
            get { return 0x03; }
        }

        protected override int TicksPerTimerCycle
        {
            get { return 64; }
        }
    }
}