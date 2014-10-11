using Xunit;

namespace Test.CpuTests
{
    public class SCF : CpuTestBase
    {
        private const byte OpCode = 0x37;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Sets_carry()
        {
            Flags(x => x.ResetCarry());

            Execute(OpCode);

            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Resets_subtract_and_half_carry()
        {
            Flags(x => x.HalfCarry().Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }
    }
}