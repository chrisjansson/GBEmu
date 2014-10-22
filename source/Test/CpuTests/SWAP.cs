using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SWAP : CpuTestBase
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
        public void Resets_subtract_carry_and_half_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.Subtract().Carry().HalfCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetCarry().ResetHalfCarry());
        }

        [Theory, PropertyData("Targets")]
        public void Swaps_uppper_nibble_with_lower_nibble(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0xCE);
            Flags(x => x.Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0xEC, target.Actual);
            AssertFlags(x => x.ResetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Sets_zero_when_value_is_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0);
            Flags(x => x.ResetZero());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.SetZero());
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

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
                {
                    new SWAPRegisterTestTarget(RegisterMapping.A), 
                    new SWAPRegisterTestTarget(RegisterMapping.B), 
                    new SWAPRegisterTestTarget(RegisterMapping.C), 
                    new SWAPRegisterTestTarget(RegisterMapping.D), 
                    new SWAPRegisterTestTarget(RegisterMapping.E), 
                    new SWAPRegisterTestTarget(RegisterMapping.H), 
                    new SWAPRegisterTestTarget(RegisterMapping.L), 
                    new SWAPHLTestTarget(), 
                };

                return targets
                    .Select(x => new object[] { x })
                    .ToList();
            }
        }
    }
}