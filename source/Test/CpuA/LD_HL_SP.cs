using Xunit;

namespace Test.CpuA
{
    public class LD_HL_SP : CpuTestBase
    {
        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(3);
        }

        [Fact]
        public void Resets_zero_and_subtract()
        {
            Flags(x => x.Zero().Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetZero().ResetSubtract());
        }

        [Fact]
        public void Adds_e_to_sp_and_stores_in_HL()
        {
            Flags(x => x.Carry().HalfCarry());
            Cpu.SP = 0xFF01;

            Execute(OpCode, 254); //-2, twos complement


            Assert.Equal(0xFEFF, RegisterPair.HL.Get(Cpu));
            AssertFlags(x => x.ResetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Sets_carry_when_carrying_from_bit_7()
        {
            Flags(x => x.ResetCarry().ResetHalfCarry());
            Cpu.SP = 0x00FF;

            Execute(OpCode, 1);

            Assert.Equal(0x100, RegisterPair.HL.Get(Cpu));
            AssertFlags(x => x.SetCarry().SetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry_when_carrying_from_bit_e()
        {
            Flags(x => x.Carry().ResetHalfCarry());
            Cpu.SP = 0x2F0F;

            Execute(OpCode, 1);


            Assert.Equal(0x2F10, RegisterPair.HL.Get(Cpu));
            AssertFlags(x => x.ResetCarry().SetHalfCarry());
            
        }

        private const byte OpCode = 0xF8;
    }
}