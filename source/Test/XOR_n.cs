using Xunit;

namespace Test
{
    public class XOR_n : CpuTestBase
    {
        [Fact]
        public void Advances_counters()
        {
            Execute(0xEE);

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Sets_zero_if_result_is_zero()
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0;

            Execute(0xEE);

            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Xors_A_and_n()
        {
            Flags(x => x.Zero());
            Cpu.A = 0x96;

            Execute(0xEE, 0x5D);

            Assert.Equal(0xCB, Cpu.A);
            AssertFlags(x => x.ResetZero());
        }

        [Fact]
        public void Resets_carry_subtact_half_carry()
        {
            Flags(x => x.Carry().HalfCarry().Subtract());

            Execute(0xEE);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry().ResetCarry());
        }
    }
}