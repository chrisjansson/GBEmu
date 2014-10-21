using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class SRA : CpuTestBase
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
        public void Shifts_content_right_into_carry(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x81);
            Flags(x => x.ResetCarry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0xC0, target.Actual);
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Targets")]
        public void Shifts_content_right_zero(ICBTestTarget target)
        {
            target.SetUp(this);
            target.ArrangeArgument(0x00);
            Flags(x => x.Carry().Zero());

            ExecutingCB(target.OpCode);

            Assert.Equal(0x00, target.Actual);
            AssertFlags(x => x.ResetCarry().SetZero());
        }

        public class SRARegisterTestTarget : RegisterCBTestTargetBase
        {
            public SRARegisterTestTarget(RegisterMapping register) : base(register) { }

            public override byte OpCode
            {
                get { return (byte)(0x28 | Register); }
            }
        }

        public static IEnumerable<object[]> Targets
        {
            get
            {
                var targets = new ICBTestTarget[]
                {
                    new SRARegisterTestTarget(RegisterMapping.A), 
                    new SRARegisterTestTarget(RegisterMapping.B), 
                    new SRARegisterTestTarget(RegisterMapping.C), 
                    new SRARegisterTestTarget(RegisterMapping.D), 
                    new SRARegisterTestTarget(RegisterMapping.E), 
                    new SRARegisterTestTarget(RegisterMapping.H), 
                    new SRARegisterTestTarget(RegisterMapping.L), 
                };

                return targets
                    .Select(x => new[] { x })
                    .ToList();
            }
        }
    }
}