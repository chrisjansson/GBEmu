using System.Collections.Generic;
using System.Linq;
using Test.CpuTests;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class RR_r : CpuTestBase
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
        public void Rotates_the_contents_right_rotating_in_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0xDD);
            Flags(x => x.ResetCarry());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x6E, target.Actual);
            AssertFlags(x => x.SetCarry());
        }

        [Theory, PropertyData("Targets")]
        public void Rotates_the_contents_right_and_resets_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x80, target.Actual);
            AssertFlags(x => x.ResetZero().ResetCarry());
        }

        [Theory, PropertyData("Targets")]
        public void Sets_zero_when_result_is_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.ResetZero().ResetCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.SetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Resets_half_carry_and_subtract(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.HalfCarry().Subtract());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }

        public class RRRegisterTestTarget : RegisterCBTestTargetBase
        {
            public RRRegisterTestTarget(RegisterMapping register)
                : base(register)
            {
            }

            public override byte OpCode
            {
                get { return (byte)(0x18 | Register); }
            }
        }

        public class RRHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x1E; }
            }
        }

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
                {
                    new RRRegisterTestTarget(RegisterMapping.A),
                    new RRRegisterTestTarget(RegisterMapping.B),
                    new RRRegisterTestTarget(RegisterMapping.C),
                    new RRRegisterTestTarget(RegisterMapping.D),
                    new RRRegisterTestTarget(RegisterMapping.E),
                    new RRRegisterTestTarget(RegisterMapping.H),
                    new RRRegisterTestTarget(RegisterMapping.L),
                    new RRHLTestTarget(), 
                };

                return targets
                    .Select(x => new[] { x })
                    .ToList();
            }
        }

    }
}