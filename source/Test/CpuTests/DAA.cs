using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class DAA : CpuTestBase
    {
        private const byte OpCode = 0x27;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_half_carry()
        {
            Flags(x => x.HalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.ResetHalfCarry());
        }

        [Theory]
        [InlineData(0, 0, 0x99, 0x99, 0)]
        [InlineData(0, 0, 0x8A, 0x90, 0)]
        [InlineData(0, 1, 0x93, 0x99, 0)]
        [InlineData(0, 0, 0xA9, 0x09, 1)]
        [InlineData(0, 0, 0x9A, 0x00, 1)]
        public void Adjusts_accumulator_add(int c, int hc, int a, int expectedA, int expectedC)
        {
            Cpu.N = 0;
            Cpu.Carry = (byte)c;
            Cpu.HC = (byte)hc;
            Cpu.A = (byte)a;

            Execute(OpCode);

            Assert.Equal(expectedA, Cpu.A);
            Assert.Equal(expectedC, Cpu.Carry);
        }
    }
}