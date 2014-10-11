using Core;

namespace Test.TimerTests
{
    public static class TimerExtensions
    {
        public static void Tick(this Timer timer, int ticks)
        {
            for (var i = 0; i < ticks; i++)
            {
                timer.Tick();
            }
        }
    }
}