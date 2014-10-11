using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class INC_HLm : CpuTestBase
    {
        private const byte OpCode = 0x34;
        
        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(3);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract());
        }

        [Theory, AutoData]
        public void Increments_memory_point_at_by_hl(ushort address)
        {
            RegisterPair.HL.Set(Cpu, (byte) (address >> 8), (byte) address);
            FakeMmu.Memory[address] = 0xFF;
            Flags(x => x.ResetHalfCarry().ResetZero());

            Execute(OpCode);

            Assert.Equal(0, FakeMmu.Memory[address]);
            AssertFlags(x => x.HalfCarry().SetZero());
        }

        [Theory, AutoData]
        public void Increments_memory_point_at_by_hl_and_resets_zero_and_half_carry(ushort address)
        {
            RegisterPair.HL.Set(Cpu, (byte) (address >> 8), (byte) address);
            FakeMmu.Memory[address] = 0x01;
            Flags(x => x.HalfCarry().Zero());

            Execute(OpCode);

            Assert.Equal(0x02, FakeMmu.Memory[address]);
            AssertFlags(x => x.ResetHalfCarry().ResetZero());
        }

    }
}