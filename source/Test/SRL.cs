using System.Collections.Generic;
using Test.CpuTests;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class SRL : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Shifts_contents_right_sets_carry_and_resets_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x8F);
            Flags(x => x.Zero().ResetCarry());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x47, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Resets_carry_and_sets_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().ResetZero());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Subtract_and_half_carry_flags_are_reset(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(_ => _.Subtract().HalfCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
        {
            return new ICBTestTarget[]
            {
                new SRLRegisterTestTarget(RegisterMapping.A),
                new SRLRegisterTestTarget(RegisterMapping.B),
                new SRLRegisterTestTarget(RegisterMapping.C),
                new SRLRegisterTestTarget(RegisterMapping.D),
                new SRLRegisterTestTarget(RegisterMapping.E),
                new SRLRegisterTestTarget(RegisterMapping.H),
                new SRLRegisterTestTarget(RegisterMapping.L),
                new SRLHLTestTarget(),
            };
        }

        private class SRLRegisterTestTarget : RegisterCBTestTargetBase
        {
            public SRLRegisterTestTarget(RegisterMapping register) : base(register) { }

            public override byte OpCode
            {
                get { return (byte)(0x38 | Register); }
            }
        }

        private class SRLHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x3E; }
            }
        }
    }
}