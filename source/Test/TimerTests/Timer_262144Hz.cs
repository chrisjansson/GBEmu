namespace Test.TimerA
{
    public class Timer_262144Hz : TimerTestBase
    {
        protected override byte TimerSelect
        {
            get { return 0x01; }
        }

        protected override int TicksPerTimerCycle
        {
            get { return 4; }
        }
    }
}