    using System;
using Core;
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
        public void C_and_half_carry()
        {
            //A - n is the order of subtraction
            throw new NotImplementedException();
        }

        private byte CreateOpCode()
        {
            return 0xFE;
        }
    }
}