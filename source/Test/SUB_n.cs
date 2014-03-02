using Xunit;

namespace Test
{
    public class SUB_n : CpuTestBase
    {
        private const byte OpCode = 0xD6;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Sets_z_when_result_is_zero()
        {
            Flags(x => x.ResetZero().Carry().HalfCarry());
            Cpu.A = 0xAB;

            Execute(OpCode, 0xAB);

            Assert.Equal(0, Cpu.A);
            AssertFlags(x => x.SetZero().ResetCarry().ResetCarry());
        }

        [Fact]
        public void Resets_z_when_A_is_not_zero()
        {
            Flags(x => x.Zero().ResetCarry().ResetHalfCarry());
            Cpu.A = 0x34;

            Execute(OpCode, 0xAB);

            Assert.Equal(0x89, Cpu.A);
            AssertFlags(x => x.ResetZero().SetCarry().SetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry_when_borrow_from_4th_to_3rd_bit()
        {
            Flags(x => x.ResetHalfCarry().ResetCarry());
            Cpu.A = 0x00;

            Execute(OpCode, 0x01);

            Assert.Equal(0xFF, Cpu.A);
            AssertFlags(x => x.SetHalfCarry().SetCarry());
        }

        [Fact]
        public void Sets_subtract()
        {
            Flags(x => x.ResetSubtract());

            Cpu.Execute(OpCode);

            AssertFlags(x => x.SetSubtract());
        }
    }
}