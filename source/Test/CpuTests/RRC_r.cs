using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class RRC : CpuTestBase
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
        public void Rotates_register_right(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x11);
            Flags(x => x.ResetCarry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x88, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Rotates_register_right_set_zero_reset_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().ResetZero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x00, target.Actual);
            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Resets_subtract_and_half_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.Subtract().HalfCarry());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
                {
                    new RRCRegisterTestTarget(RegisterMapping.A), 
                    new RRCRegisterTestTarget(RegisterMapping.B), 
                    new RRCRegisterTestTarget(RegisterMapping.C), 
                    new RRCRegisterTestTarget(RegisterMapping.D), 
                    new RRCRegisterTestTarget(RegisterMapping.E), 
                    new RRCRegisterTestTarget(RegisterMapping.H), 
                    new RRCRegisterTestTarget(RegisterMapping.L), 
                    new RRCHLTestTarget(), 
                };

                return targets
                    .Select(x => new object[] { x })
                    .ToList();
            }
        }

        public class RRCRegisterTestTarget : RegisterCBTestTargetBase
        {
            public RRCRegisterTestTarget(RegisterMapping register)
                : base(register) { }

            public override byte OpCode
            {
                get { return (byte)(0x08 | Register); }
            }
        }

        public class RRCHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x0E; }
            }
        }
    }
}