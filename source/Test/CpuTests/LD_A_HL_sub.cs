using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class LD_A_HL_sub : CpuTestBase
    {
        private const int OpCode = 0x3A;

        [Fact]
        public void Advances_counters()
        {
            Cpu.Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Theory, AutoData]
        public void Loads_HL_into_A(ushort hl, byte value)
        {
            FakeMmu.SetByte(hl, value);
            RegisterPair.HL.Set(Cpu, (byte)(hl >> 8), (byte)hl);

            Execute(OpCode);

            Assert.Equal(value, Cpu.A);
        }

        [Theory, AutoData]
        public void Decrements_hl(ushort hl)
        {
            RegisterPair.HL.Set(Cpu, (byte)(hl >> 8), (byte)hl);

            Execute(OpCode);

            var actual = RegisterPair.HL.Get(Cpu);
            Assert.Equal(hl - 1, actual);
        }
    }
}