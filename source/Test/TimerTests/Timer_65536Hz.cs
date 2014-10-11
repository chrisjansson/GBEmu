namespace Test.TimerA
{
    public class Timer_65536Hz : TimerTestBase
    {
        protected override byte TimerSelect
        {
            get { return 0x02; }
        }

        protected override int TicksPerTimerCycle
        {
            get { return 16; }
        }
    }
}