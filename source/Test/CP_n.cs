using Xunit;

namespace Test
{
    public class CP_n : CpuTestBase
    {
        [Fact]
        public void Program_counter_cycles_sets_N()
        {
            Flags(x => x.ResetSubtract());

            Execute(CreateOpCode());

            AssertFlags(x => x.SetSubtract());
            AdvancedClock(2);
            AdvancedProgramCounter(2);
        }

        [Fact]
        public void Sets_z_when_A_is_equal_to_n()
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0xAB;

            Execute(CreateOpCode(), 0xAB);

            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Resets_z_when_A_is_not_equal_to_n()
        {
            Flags(x => x.Zero());
            Cpu.A = 0x34;

            Execute(CreateOpCode(), 0xAB);

            AssertFlags(x => x.ResetZero());
        }

        [Fact]
        public void Sets_carry()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0x3C;

            Execute(CreateOpCode(), 0x40);

            AssertFlags(x => x.SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry_when_borrowing_from_4th_to_3rd_bit()
        {
            Flags(x => x.ResetHalfCarry());
            Cpu.A = 0x3C;

            Execute(CreateOpCode(), 0x2F);

            AssertFlags(x => x.SetHalfCarry().ResetCarry());
        }

        private byte CreateOpCode()
        {
            return 0xFE;
        }
    }
}