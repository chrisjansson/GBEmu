using Xunit;

namespace Test.CpuA
{
    public class ADD_SP_e : CpuTestBase
    {
        private const byte OpCode = 0xE8;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(4);
        }

        [Fact]
        public void Resets_N_and_Z()
        {
            Flags(x => x.Subtract().Zero());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract().ResetZero());
        }

        [Fact]
        public void Sets_carry_when_overflowing()
        {
            Flags(x => x.ResetCarry().HalfCarry());
            Cpu.SP = 0xF0;

            Execute(OpCode, 0x10);

            Assert.Equal(0x100, Cpu.SP);
            AssertFlags(x => x.SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry()
        {
            Flags(x => x.ResetHalfCarry().Carry());
            Cpu.SP = 0xF;

            Execute(OpCode, 0x01);

            Assert.Equal(0x10, Cpu.SP);
            AssertFlags(x => x.SetHalfCarry().ResetCarry());
        }

        [Fact]
        public void Resets_half_carry()
        {
            Flags(x => x.HalfCarry());
            Cpu.SP = 1;

            Execute(OpCode, 0);

            AssertFlags(x => x.ResetHalfCarry());
        }

        [Fact]
        public void Adds_signed_e_to_sp()
        {
            Cpu.SP = 0xFF;

            Execute(OpCode, 0x80); //-128

            Assert.Equal(0x7F, Cpu.SP);
        }
    }
}