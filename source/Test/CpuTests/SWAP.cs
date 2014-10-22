using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SWAP : CBTestTargetBase
    {
        [Theory, InstancePropertyData("Targets")]
        public void Resets_subtract_carry_and_half_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.Subtract().Carry().HalfCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetCarry().ResetHalfCarry());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Swaps_uppper_nibble_with_lower_nibble(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0xCE);
            Flags(x => x.Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0xEC, target.Actual);
            AssertFlags(x => x.ResetZero());
        }

        [Theory, InstancePropertyData("Targets")]
        public void Sets_zero_when_value_is_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0);
            Flags(x => x.ResetZero());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.SetZero());
        }

        protected override IEnumerable<ICBTestTarget> GetTargets()
        {
            foreach (var register in RegisterMapping.GetAll())
            {
                yield return new SWAPRegisterTestTarget(register);
            }
            yield return new SWAPHLTestTarget();
        }

        private class SWAPRegisterTestTarget : RegisterCBTestTargetBase
        {
            public SWAPRegisterTestTarget(RegisterMapping register) : base(register) { }

            public override byte OpCode
            {
                get { return (byte)(0x30 | Register); }
            }
        }

        public class SWAPHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x36; }
            }
        }
    }
}