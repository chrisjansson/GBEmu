using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SLA : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Resets_half_carry_and_subtract(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.HalfCarry().Subtract());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Shifts_contents_left(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x81);
            Flags(x => x.ResetCarry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x02, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Shifts_contents_left_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().ResetZero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x00, target.Actual);
            AssertFlags(x => x.SetZero().ResetCarry());
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
        {
            return new ICBTestTarget[]
            {
                new SLARegisterTestTarget(RegisterMapping.A),
                new SLARegisterTestTarget(RegisterMapping.B),
                new SLARegisterTestTarget(RegisterMapping.C),
                new SLARegisterTestTarget(RegisterMapping.D),
                new SLARegisterTestTarget(RegisterMapping.E),
                new SLARegisterTestTarget(RegisterMapping.H),
                new SLARegisterTestTarget(RegisterMapping.L),
                new SLAHLTestTarget()
            };
        }

        private class SLAHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x26; }
            }
        }

        private class SLARegisterTestTarget : RegisterCBTestTargetBase
        {
            public SLARegisterTestTarget(RegisterMapping register) : base(register)
            {
            }

            public override byte OpCode
            {
                get { return (byte) (0x20 | Register); }
            }
        }
    }
}