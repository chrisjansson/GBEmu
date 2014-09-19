using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuA
{
    public class LD_HL_n : CpuTestBase
    {
        private const byte OpCode = 0x36;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(3);
        }

        [Theory, AutoData]
        public void Stores_value_at_memory_pointed_to_by_HL(ushort hl, byte value)
        {
            RegisterPair.HL.Set(Cpu, (byte) (hl >> 8), (byte) hl);

            Execute(OpCode, value);

            Assert.Equal(value, FakeMmu.Memory[hl]);
        }
    }
}