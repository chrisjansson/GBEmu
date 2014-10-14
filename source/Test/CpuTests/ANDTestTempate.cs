using Ploeh.AutoFixture;
using Xunit;

namespace Test.CpuTests
{
    public abstract class ANDTestTempate : CpuTestBase
    {
        protected abstract byte OpCode { get; }

        protected abstract int InstructionLength { get; }

        protected abstract void ArrangeArgument(byte argument);

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(InstructionLength);
            AdvancedClock(2);
        }

        [Fact]
        public void Resets_subtract()
        {
            Flags(x => x.Subtract());

            Execute(OpCode);

            AssertFlags(x => x.ResetSubtract());
        }

        [Fact]
        public void Sets_half_carry()
        {
            Flags(x => x.ResetHalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.SetHalfCarry());
        }


        [Fact]
        public void Resets_carry()
        {
            Flags(x => x.Carry());

            Execute(OpCode);

            AssertFlags(x => x.ResetCarry());
        }

        [Fact]
        public void Sets_zero_when_result_is_zero()
        {
            Flags(x => x.ResetZero());
            ArrangeArgument(0);
            Cpu.A = 0;

            Execute(OpCode);

            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void Ands_A_and_r()
        {
            Flags(x => x.Zero());
            ArrangeArgument(0x3F);
            Cpu.A = 0x5A;

            Execute(OpCode);

            Assert.Equal(0x1A, Cpu.A);
            AssertFlags(x => x.ResetZero());
        }
    }

    public class AND_HLm : ANDTestTempate
    {
        protected override byte OpCode
        {
            get { return 0xA6; }
        }

        protected override int InstructionLength
        {
            get { return 1; }
        }

        protected override void ArrangeArgument(byte argument)
        {
            var hl = Fixture.Create<ushort>();
            Set(RegisterPair.HL, hl);
            FakeMmu.Memory[hl] = argument;
        }
    }
}