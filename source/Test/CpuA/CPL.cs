using Xunit;

namespace Test.CpuA
{
    public class CPL : CpuTestBase
    {
        private const byte OpCode = 0x2F;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Sets_half_carry_and_subtract()
        {
            Flags(x => x.ResetHalfCarry().ResetSubtract());
            
            Execute(OpCode);

            AssertFlags(x => x.HalfCarry().SetSubtract());
        }

        [Fact]
        public void Inverts_A()
        {
            Cpu.A = 0x35;

            Execute(OpCode);

            Assert.Equal(0xCA, Cpu.A);
        }
    }
}