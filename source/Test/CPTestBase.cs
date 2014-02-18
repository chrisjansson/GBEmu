using Core;
using Xunit;

namespace Test
{
    public abstract class CPTestBase : CpuTestBase
    {
        protected abstract void ExecuteCompareATo(byte value);
        protected abstract int Length { get; }

        [Fact]
        public void Program_counter_cycles_sets_N()
        {
            Flags(x => x.ResetSubtract());

            ExecuteCompareATo(0xAB);

            AssertFlags(x => x.SetSubtract());
            AdvancedClock(2);
            AdvancedProgramCounter(Length);
        }

        [Fact]
        public void Sets_z_when_A_is_equal_to_n()
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0xAB;

            ExecuteCompareATo(0xAB);

            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Resets_z_when_A_is_not_equal_to_n()
        {
            Flags(x => x.Zero());
            Cpu.A = 0x34;

            ExecuteCompareATo(0xAB);

            AssertFlags(x => x.ResetZero());
        }

        [Fact]
        public void Sets_carry()
        {
            Flags(x => x.ResetCarry());
            Cpu.A = 0x3C;

            ExecuteCompareATo(0x40);

            AssertFlags(x => x.SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry_when_borrowing_from_4th_to_3rd_bit()
        {
            Flags(x => x.ResetHalfCarry());
            Cpu.A = 0x3C;

            ExecuteCompareATo(0x2F);

            AssertFlags(x => x.SetHalfCarry().ResetCarry());
        }
    }

    public class CP_HL : CPTestBase
    {
        protected override void ExecuteCompareATo(byte value)
        {
            RegisterPair.HL.Set(Cpu, 0xAB, 0x34);
            FakeMmu.SetByte(0xAB34, value);
            
            Execute(0xBE);
        }

        protected override int Length
        {
            get { return 1; }
        }
    }
}