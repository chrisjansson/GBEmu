using System.Collections.Generic;
using Test.CpuTests;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class RL : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Rotates_the_content_left_and_sets_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x8F);
            Flags(x => x.ResetCarry());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x1E, target.Actual);
            AssertFlags(x => x.SetCarry());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Rotates_content_left_and_resets_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x01, target.Actual);
            AssertFlags(x => x.ResetZero().ResetCarry());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Sets_zero_when_result_is_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.ResetZero().ResetCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.SetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Resets_half_carry_and_subtract(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.HalfCarry().Subtract());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
        {
            return new ICBTestTarget[]
                {
                    new RLRegisterTestTarget(RegisterMapping.A), 
                    new RLRegisterTestTarget(RegisterMapping.B), 
                    new RLRegisterTestTarget(RegisterMapping.C), 
                    new RLRegisterTestTarget(RegisterMapping.D), 
                    new RLRegisterTestTarget(RegisterMapping.E), 
                    new RLRegisterTestTarget(RegisterMapping.H), 
                    new RLRegisterTestTarget(RegisterMapping.L), 
                    new RLHLTestTarget()
                };
        }

        public class RLHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x16; }
            }
        }

        public class RLRegisterTestTarget : RegisterCBTestTargetBase
        {
            public RLRegisterTestTarget(RegisterMapping register)
                : base(register) { }

            public override byte OpCode
            {
                get { return (byte)(0x10 | Register); }
            }
        }
    }

}