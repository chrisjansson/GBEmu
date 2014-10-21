using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SLA : CpuTestBase
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
        public void Resets_half_carry_and_subtract(ICBTestTarget target)
        {
            target.SetUp(this);
            Flags(x => x.HalfCarry().Subtract());

            ExecutingCB(target.OpCode);

            AssertFlags(x => x.ResetHalfCarry().ResetSubtract());
        }

        [Theory, PropertyData("Targets")]
        public void Shifts_contents_left(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x81);
            Flags(x => x.ResetCarry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x02, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Shifts_contents_left_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().ResetZero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x00, target.Actual);
            AssertFlags(x => x.SetZero().ResetCarry());
        }

        public class SLARegisterTestTarget : RegisterCBTestTargetBase
        {
            public SLARegisterTestTarget(RegisterMapping register) : base(register) { }

            public override byte OpCode
            {
                get { return (byte)(0x20 | Register); }
            }
        }

        public class SLAHLTestTarget : HLCBTestTargetBase
        {
            public override byte OpCode
            {
                get { return 0x26; }
            }
        }

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
                {
                    new SLARegisterTestTarget(RegisterMapping.A), 
                    new SLARegisterTestTarget(RegisterMapping.B), 
                    new SLARegisterTestTarget(RegisterMapping.C), 
                    new SLARegisterTestTarget(RegisterMapping.D), 
                    new SLARegisterTestTarget(RegisterMapping.E), 
                    new SLARegisterTestTarget(RegisterMapping.H), 
                    new SLARegisterTestTarget(RegisterMapping.L), 
                    new SLAHLTestTarget(), 
                };

                return targets
                    .Select(x => new[] { x })
                    .ToList();
            }
        }
    }
}