using System.Collections.Generic;
using System.Linq;
using Test.CpuTests;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class SRL_r : CpuTestBase
    {
        [Theory, PropertyData("Targets")]
        public void Advances_counters(ICBTestTarget target)
        {
            ExecutingCB(target.OpCode);

            AdvancedProgramCounter(target.InstructionLength);
            AdvancedClock(target.InstructionTime);
        }

        [Theory, PropertyData("Targets")]
        public void Shifts_contents_right_sets_carry_and_resets_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x8F);
            Flags(x => x.Zero().ResetCarry());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x47, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Resets_carry_and_sets_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().ResetZero());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Subtract_and_half_carry_flags_are_reset(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(_ => _.Subtract().HalfCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
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

                return targets
                    .Select(x => new[] { x })
                    .ToList();
            }
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