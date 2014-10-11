using Xunit;

namespace Test.CpuTests
{
    public class RRCA : CpuTestBase
    {
        private const byte OpCode = 0x0F;
        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_ZHN()
        {
            Flags(x => x.Zero().Subtract().HalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.ResetZero().ResetHalfCarry().ResetSubtract());
        }

        [Fact]
        public void Rotates_A_to_the_right()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0x11;

            Execute(OpCode);

            Assert.Equal(0x88, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Rotates_A_to_the_right_resetting_carry()
        {
            Flags(x => x.Carry());
            Cpu.A = 0x12;

            Execute(OpCode);

            Assert.Equal(0x9, Cpu.A);
            AssertFlags(x => x.ResetCarry());
        }
    }
}