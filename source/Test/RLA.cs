using Xunit;

namespace Test
{
    public class RLA : CpuTestBase
    {
        protected byte CreateOpCode()
        {
            return 0x17;
        }

        [Fact]
        public void Advances_counters()
        {
            Execute(CreateOpCode());

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Rotates_the_content_left_and_sets_carry()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0x8F;

            Execute(CreateOpCode());

            Assert.Equal(0x1E, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Rotates_content_left_and_resets_carry()
        {
            Flags(x => x.Carry().Zero());
            Cpu.A = 0;

            Execute(CreateOpCode());

            Assert.Equal(0x01, Cpu.A);
            AssertFlags(x => x.ResetZero().ResetCarry());
        }

        [Fact]
        public void Resets_half_carry_zero_and_subtract()
        {
            Flags(x => x.HalfCarry().Subtract().Zero().ResetCarry());
            Cpu.A = 0x00;

            Execute(CreateOpCode());

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract().ResetZero());
        }
    }
}