using Xunit;

namespace Test.CpuA
{
    public class ADD_SP_n : CpuTestBase
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
            Flags(x => x.ResetCarry().ResetHalfCarry());
            Cpu.SP = 0xFFFF;

            Execute(OpCode, 0x01);

            Assert.Equal(0, Cpu.SP);
            AssertFlags(x => x.SetCarry().SetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry()
        {
            Flags(x => x.ResetHalfCarry().Carry());
            Cpu.SP = 0xFFF;

            Execute(OpCode, 0x01);

            Assert.Equal(0x1000, Cpu.SP);
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
    }
}