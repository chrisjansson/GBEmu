using Xunit;

namespace Test.CpuA
{
    public class RLCA : CpuTestBase
    {
        private const byte OpCode = 0x07;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_HNZ()
        {
            Flags(x => x.Subtract().Zero().HalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract().ResetZero().ResetHalfCarry());
        }

        [Fact]
        public void Rotates_contents_of_A_left()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0x81;

            Execute(OpCode);

            Assert.Equal(0x03, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Rotates_contents_of_A_left_does_not_set_carry()
        {
            Flags(x => x.Carry());
            Cpu.A = 0x42;

            Execute(OpCode);

            Assert.Equal(0x84, Cpu.A);
            AssertFlags(x => x.ResetCarry());
        }
    }
}