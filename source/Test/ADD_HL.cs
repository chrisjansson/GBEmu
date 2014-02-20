using Xunit;

namespace Test
{
    public class ADD_HL : CpuTestBase
    {
        [Fact]
        public void AdvancesCounters()
        {
            Execute(CreateOpCode());

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());

            Execute(CreateOpCode());

            AssertFlags(x => x.ResetSubtract());
        }

        [Fact]
        public void Adds_memory_at_hl_to_a()
        {
            Flags(x => x.ResetCarry().ResetZero().ResetHalfCarry());
            Cpu.A = 0x3A;
            RegisterPair.HL.Set(Cpu, 0x12, 0xAB);
            FakeMmu.SetByte(0x12AB, 0xC6);

            Execute(CreateOpCode());

            Assert.Equal(0, Cpu.A);
            AssertFlags(x => x.SetCarry().SetZero().SetHalfCarry());
        }

        [Fact]
        public void Sets_half_carry()
        {
            Flags(x => x.Carry().Zero().HalfCarry());
            Cpu.A = 0x11;
            RegisterPair.HL.Set(Cpu, 0x12, 0xAB);
            FakeMmu.SetByte(0x12AB, 0x11);

            Execute(CreateOpCode());

            Assert.Equal(0x22, Cpu.A);
            AssertFlags(x => x.ResetCarry().ResetHalfCarry().ResetZero());
        }

        private byte CreateOpCode()
        {
            return 0x86;
        }
    }
}