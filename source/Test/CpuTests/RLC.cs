using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class RLC : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Rotates_value_left(ICBTestTarget target)
        {
            Flags(x => x.ResetCarry().Zero());
            target.SetUp(this);
            target.ArrangeArgument(0xC0);

            ExecutingCB(target.OpCode);

            Assert.Equal(0x81, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Rotates_value_left_set_zero_reset_carry(ICBTestTarget target)
        {
            Flags(x => x.Carry().ResetZero());
            target.SetUp(this);
            target.ArrangeArgument(0x00);

            ExecutingCB(target.OpCode);

            Assert.Equal(0x00, target.Actual);
            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Resets_subtract_and_half_carry(ICBTestTarget target)
        {
            Flags(x => x.Subtract().HalfCarry());
            target.SetUp(this);

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
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
            return targets;
        }

        private class RegisterCBTestTarget : RegisterCBTestTargetBase
        {
            public RegisterCBTestTarget(RegisterMapping register)
                : base(register) { }

            public override byte OpCode
            {
                get { return Register; }
            }

        }

        private class RLCHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x06; }
            }
        }
    }
}