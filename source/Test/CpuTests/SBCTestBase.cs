using Xunit;

namespace Test.CpuTests
{
    public abstract class SBCTestBase : CpuTestBase
    {
        protected abstract byte OpCode { get; }
        protected abstract byte InstructionLength { get; }
        protected abstract void ArrangeArgument(byte argument);

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(InstructionLength);
            AdvancedClock(2);
        }

        [Fact]
        public void Sets_subtract()
        {
            Flags(x => x.ResetSubtract());
            
            Execute(OpCode);

            AssertFlags(x => x.SetSubtract());
        }

        [Fact]
        public void Sets_zero()
        {
            Flags(x => x.ResetZero().Carry().HalfCarry());
            Cpu.A = 0x3B;
            ArrangeArgument(0x3A);

            Execute(OpCode);

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero().ResetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Sets_carry()
        {
            Flags(x => x.Carry().Zero().HalfCarry());
            Cpu.A = 0x3B;
            ArrangeArgument(0x40);

            Execute(OpCode);

            Assert.Equal(0xFA, Cpu.A);
            AssertFlags(x => x.SetCarry().ResetHalfCarry().ResetZero());
        }

        [Fact]
        public void Sets_half_carry()
        {
            Flags(x => x.Carry().Zero().ResetHalfCarry());
            Cpu.A = 0x15;
            ArrangeArgument(0x05);

            Execute(OpCode);

            Assert.Equal(0x0F, Cpu.A);
            AssertFlags(x => x.SetHalfCarry().ResetCarry().ResetZero());
        }
        
    }
}