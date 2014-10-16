using Xunit;

namespace Test.CpuTests
{
    public class RLC_HLm : CpuTestBase
    {
        private const byte OpCode = 0x06;

        [Fact]
        public void Advances_counters()
        {
            ExecutingCB(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(4);
        }
    }
}