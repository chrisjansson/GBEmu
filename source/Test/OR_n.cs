using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class OR_n : CpuTestBase
    {
        private const byte OpCode = 0xF6;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_subtract_hc_and_c()
        {
            Flags(x => x.Subtract().HalfCarry().Carry());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry().ResetCarry());
        }

        [Theory, AutoData]
        public void ORs_contents_of_A_with_n(byte a, byte n)
        {
            Flags(x => x.Zero());
            Cpu.A = a;

            Execute(OpCode, n);

            Assert.Equal(a | n, Cpu.A);
            AssertFlags(x => x.ResetZero());
        }

        [Fact]
        public void Sets_zero_if_result_is_zero()
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0;

            Execute(OpCode, 0);

            AssertFlags(x => x.SetZero());
        }
    }
}