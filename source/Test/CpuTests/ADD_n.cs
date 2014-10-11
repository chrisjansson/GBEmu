using Xunit;

namespace Test.CpuTests
{
    public class ADD_n : CpuTestBase
    {
        private const byte OpCode = 0xC6;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract());
        }

        [Fact]
        public void Adds_n_to_A()
        {
            Flags(x => x.Zero().Carry());
            Cpu.A = 0x10;

            Execute(OpCode, 0x02);

            Assert.Equal(0x12, Cpu.A);
            AssertFlags(x => x.ResetZero().ResetCarry());
        }

        [Fact]
        public void Sets_zero_when_result_is_zero()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0xFF;

            Execute(OpCode, 0x01);

            Assert.Equal(0, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Sets_half_carry_and_carry()
        {
            Flags(x => x.ResetCarry().ResetHalfCarry());
            Cpu.A = 0x3C;

            Execute(OpCode, 0xFF);

            Assert.Equal(0x3B, Cpu.A);
            AssertFlags(x => x.SetCarry().HalfCarry());
        }
    }
}