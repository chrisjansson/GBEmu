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
            Cpu.A = a;

            Execute(OpCode, n);

            Assert.Equal(a | n, Cpu.A);
        }
    }
}