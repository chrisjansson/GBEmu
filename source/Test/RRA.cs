using Xunit;

namespace Test
{
    public class RRA : CpuTestBase
    {
        private const byte OpCode = 0x1F;

        [Fact]
        public void Rotates_the_contents_right_rotating_in_carry()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0xDD;

            Execute(OpCode);

            Assert.Equal(0x6E, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Rotates_the_contents_right_and_resets_carry()
        {
            Flags(x => x.Carry().Zero());
            Cpu.A = 0;

            Execute(OpCode);

            Assert.Equal(0x80, Cpu.A);
            AssertFlags(x => x.ResetZero().ResetCarry());
        }

        [Fact]
        public void Sets_zero_when_result_is_zero()
        {
            Flags(x => x.ResetZero().ResetCarry());
            Cpu.A = 0x01;

            Execute(OpCode);

            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Resets_half_carry_and_subtract()
        {
            Flags(x => x.HalfCarry().Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }
    }
}