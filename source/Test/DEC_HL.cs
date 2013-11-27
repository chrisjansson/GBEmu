using System;
using Xunit;

namespace Test
{
    public class DEC_HL : CpuTestBase
    {
        [Fact]
        public void Decrements_result_pointed_to_by_HL()
        {
            Flags(x => x.Zero());
            Cpu.H = 0x31;
            Cpu.L = 0x8B;
            FakeMmu.SetByte(0x318B, 0x32);

            Execute(0x35);

            Assert.Equal(0x31, FakeMmu.GetByte(0x318B));
            AssertFlags(x => x.ResetZero());
        }

        [Fact]
        public void Decrements_result_pointed_to_by_HL_sets_zero()
        {
            Flags(x => x.ResetZero());
            Cpu.H = 0x31;
            Cpu.L = 0x8B;
            FakeMmu.SetByte(0x318B, 0x01);

            Execute(0x35);

            Assert.Equal(0x00, FakeMmu.GetByte(0x318B));
            AssertFlags(x => x.SetZero());
        }


        [Fact]
        public void Sets_hc_when_borrow_from_bit_4()
        {
            Flags(x => x.ResetHalfCarry());
            Cpu.H = 0x31;
            Cpu.L = 0x8B;
            FakeMmu.SetByte(0x318B, 0xA0);

            Execute(0x35);

            AssertFlags(x => x.SetHalfCarry());
        }

        [Fact]
        public void Resets_hc_when_not_borrow_from_bit_4()
        {
            Flags(x => x.HalfCarry());
            Cpu.H = 0x31;
            Cpu.L = 0x8B;
            FakeMmu.SetByte(0x318B, 0x01);
            
            Execute(0x35);

            AssertFlags(x => x.ResetHalfCarry());
        }

        [Fact]
        public void Increments_counters()
        {
            Execute(0x35);

            AdvancedProgramCounter(1);
            AdvancedClock(3);
        }

        [Fact]
        public void Sets_subtract()
        {
            Flags(x => x.ResetSubtract());

            Execute(0x35);

            AssertFlags(x => x.SetSubtract());
        }

    }
}