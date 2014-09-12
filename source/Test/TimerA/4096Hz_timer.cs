namespace Test.TimerA
{
    public class Timer_4096Tests : TimerTestBase
    {
        protected override byte TimerSelect
        {
            get { return 0x00; }
        }

        protected override int TicksPerTimerCycle
        {
            get { return 256; }
        }
    }
}