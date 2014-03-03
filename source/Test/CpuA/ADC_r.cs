using Xunit;

namespace Test.CpuA
{
    public class ADC_r : CpuTestBase
    {
        private static byte OpCode = 0x88;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());
            
            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract());
        }

        [Fact]
        public void Adds_register_to_a_sets_half_carry()
        {
            Flags(x => x.Zero().Carry().ResetHalfCarry());
            Cpu.A = 0xE1;
            Cpu.B = 0x0E;

            Execute(OpCode);

            Assert.Equal(0xF0, Cpu.A);
            AssertFlags(x => x.ResetZero().ResetCarry().SetHalfCarry());
        }

        [Fact]
        public void Adds_register_to_a_sets_carry()
        {
            Flags(x => x.Zero().Carry().ResetHalfCarry());
            Cpu.A = 0xE1;
            Cpu.B = 0x3B;

            Execute(OpCode);

            Assert.Equal(0x1D, Cpu.A);
            AssertFlags(x => x.ResetZero().SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Adds_register_to_a_sets_zero()
        {
            Flags(x => x.ResetZero().Carry().ResetHalfCarry());
            Cpu.A = 0xE1;
            Cpu.B = 0x1E;

            Execute(OpCode);

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero().SetCarry().SetHalfCarry());
        }
    }
}