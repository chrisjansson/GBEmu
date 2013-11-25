using Xunit;

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

        [Fact]
        public void Adds_n_to_a_carry()
        {
            Flags(x => x.ResetCarry().HalfCarry());
            Cpu.A = 0xF0;

            Execute(0xCE, 0xF0);

            Assert.Equal(0xE0, Cpu.A);
            AssertFlags(x => x.SetCarry().ResetHalfCarry());
        }

        [Fact]
        public void Sets_zero()
        {
            Flags(x => x.ResetZero().ResetCarry());
            Cpu.A = 0;

            Execute(0xCE, 0x00);

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