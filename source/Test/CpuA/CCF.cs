using Xunit;

namespace Test.CpuA
{
    public class CCF : CpuTestBase
    {
        private const byte OpCode = 0x3F;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_subtract_and_half_carry()
        {
            Flags(x => x.Subtract().HalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        [Fact]
        public void Flips_carry()
        {
            Cpu.Carry = 1;

            Execute(OpCode);

            Assert.Equal(0, Cpu.Carry);
        }

        [Fact]
        public void Flips_carry_inverse()
        {
            Cpu.Carry = 0;

            Execute(OpCode);

            Assert.Equal(1, Cpu.Carry);
        }
    }
}