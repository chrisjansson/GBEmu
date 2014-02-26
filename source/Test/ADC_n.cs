using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class ADC_n : CpuTestBase
    {
        [Fact]
        public void Adds_n_to_a()
        {
            Flags(x => x.ResetHalfCarry().Carry().Zero());
            Cpu.A = 0x0B;

            Execute(0xCE, 0x06);

            Assert.Equal(0x12, Cpu.A);
            AssertFlags(x => x.HalfCarry().ResetCarry().ResetZero());
        }

        [Theory]
        [InlineData((byte)0xF0, (byte)0xF0, (byte)0xE0)]
        [InlineData((byte)0x80, (byte)0x80, (byte)0x00)]
        public void Adds_n_to_a_carry(byte register, byte n, byte result)
        {
            Flags(x => x.ResetCarry().HalfCarry());
            Cpu.A = register;

            Execute(0xCE, n);

            Assert.Equal(result, Cpu.A);
            AssertFlags(x => x.SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Calculates_half_carry_with_carry()
        {
            Flags(x => x.Carry().ResetHalfCarry());

            Cpu.A = 0x0A;

            Execute(0xCE, 0x05);

            Assert.Equal(0x10, Cpu.A);
            AssertFlags(x => x.HalfCarry());
        }
        
        [Fact]
        public void Sets_zero()
        {
            Flags(x => x.ResetZero().ResetCarry());
            Cpu.A = 0xFF;

            Execute(0xCE, 0x01);

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Advances_counters()
        {
            Execute(0xCE);

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());

            Execute(0xCE);

            AssertFlags(x => x.ResetSubtract());
        }
    }
}