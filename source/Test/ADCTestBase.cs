using Xunit;
using Xunit.Extensions;

namespace Test
{
    public abstract class ADCTestBase : CpuTestBase
    {
        protected abstract byte OpCode { get; }
        protected abstract int OpCodeLength { get; }

        protected abstract void ArrangeArgument(byte argument);


        [Fact]
        public void Adds_arg_to_a()
        {
            Flags(x => x.ResetHalfCarry().Carry().Zero());
            Cpu.A = 0x0B;
            ArrangeArgument(0x06);

            Execute(OpCode);

            Assert.Equal(0x12, Cpu.A);
            AssertFlags(x => x.HalfCarry().ResetCarry().ResetZero());
        }

        [Theory]
        [InlineData((byte)0xF0, (byte)0xF0, (byte)0xE0)]
        [InlineData((byte)0x80, (byte)0x80, (byte)0x00)]
        public void Adds_arg_to_a_carry(byte register, byte arg, byte result)
        {
            Flags(x => x.ResetCarry().HalfCarry());
            Cpu.A = register;
            ArrangeArgument(arg);

            Execute(OpCode);

            Assert.Equal(result, Cpu.A);
            AssertFlags(x => x.SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Calculates_half_carry_with_carry()
        {
            Flags(x => x.Carry().ResetHalfCarry());
            Cpu.A = 0x0A;
            ArrangeArgument(0x05);

            Execute(OpCode);

            Assert.Equal(0x10, Cpu.A);
            AssertFlags(x => x.HalfCarry());
        }
        
        [Fact]
        public void Sets_zero()
        {
            Flags(x => x.ResetZero().ResetCarry());
            Cpu.A = 0xFF;
            ArrangeArgument(0x01);

            Execute(OpCode);

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(OpCodeLength);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract());
        }
    }
}