using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class RLC : CpuTestBase
    {
        [Theory, PropertyData("Targets")]
        public void Advances_counters(ICBTestTarget target)
        {
            target.SetUp(this);

            ExecutingCB(target.OpCode);

            AdvancedProgramCounter(target.InstructionLength);
            AdvancedClock(target.InstructionTime);
        }

        [Theory, PropertyData("Targets")]
        public void Rotates_value_left(ICBTestTarget target)
        {
            Flags(x => x.ResetCarry().Zero());
            target.SetUp(this);
            target.ArrangeArgument(0xC0);

            ExecutingCB(target.OpCode);

            Assert.Equal(0x81, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Rotates_value_left_set_zero_reset_carry(ICBTestTarget target)
        {
            Flags(x => x.Carry().ResetZero());
            target.SetUp(this);
            target.ArrangeArgument(0x00);

            ExecutingCB(target.OpCode);

            Assert.Equal(0x00, target.Actual);
            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Resets_subtract_and_half_carry(ICBTestTarget target)
        {
            Flags(x => x.Subtract().HalfCarry());
            target.SetUp(this);

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
                {
                    new RegisterCBTestTarget(RegisterMapping.A), 
                    new RegisterCBTestTarget(RegisterMapping.B), 
                    new RegisterCBTestTarget(RegisterMapping.C), 
                    new RegisterCBTestTarget(RegisterMapping.D), 
                    new RegisterCBTestTarget(RegisterMapping.E), 
                    new RegisterCBTestTarget(RegisterMapping.H), 
                    new RegisterCBTestTarget(RegisterMapping.L), 
                    new RLCHLTestTarget(), 
                };

                return targets
                    .Select(x => new[] { x })
                    .ToList();
            }
        }

        public class RLCHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x06; }
            }
        }
    }
}