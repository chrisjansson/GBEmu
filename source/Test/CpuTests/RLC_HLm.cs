using Ploeh.AutoFixture;
using Xunit;

namespace Test.CpuTests
{
    public class RLC_HLm : CpuTestBase
    {
        private const byte OpCode = 0x06;

        private readonly ushort _hl;

        public RLC_HLm()
        {
            _hl = Fixture.Create<ushort>();
        }

        [Fact]
        public void Advances_counters()
        {
            ExecutingCB(OpCode);

            AdvancedProgramCounter(2);
            AdvancedClock(4);
        }

        [Fact]
        public void Rotates_register_left()
        {
            Flags(x => x.ResetCarry().Zero());
            Set(RegisterPair.HL, _hl);
            FakeMmu.Memory[_hl] = 0xC0;

            ExecutingCB(OpCode);

            Assert.Equal(0x81, FakeMmu.Memory[_hl]);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Fact]
        public void Rotates_register_left_set_zero_reset_carry()
        {
            Flags(x => x.Carry().ResetZero());
            Set(RegisterPair.HL, _hl);
            FakeMmu.Memory[_hl] = 0x00;

            ExecutingCB(OpCode);

            Assert.Equal(0x00, FakeMmu.Memory[_hl]);
            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Fact]
        public void Resets_subtract_and_half_carry()
        {
            Flags(x => x.Subtract().HalfCarry());

            ExecutingCB(OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

    }
}