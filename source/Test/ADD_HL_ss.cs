using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class Arithmetic16BitTestBase : CpuTestBase
    {
        public static IEnumerable<object[]> ArithmeticRegisterPairs
        {
            get
            {
                return RegisterPair.GetArithmeticPairs()
                    .Select(x => new[] {x})
                    .ToList();
            }
        }
    }

    public class ADD_HL_ss : Arithmetic16BitTestBase
    {
        [Theory, PropertyData("ArithmeticRegisterPairs")]
        public void Advances_counters(RegisterPair registerPair)
        {
            Execute(CreateOpCode(registerPair));

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Fact]
        public void Adds_HL_to_HL()
        {
            Cpu.H = 0x8A;
            Cpu.L = 0x23;
            Flags(x => x.ResetHalfCarry().ResetCarry().Subtract());

            Execute(0x29);

            Assert.Equal(0x14, Cpu.H);
            Assert.Equal(0x46, Cpu.L);
            AssertFlags(x => x.SetHalfCarry().SetCarry().ResetSubtract());
        }

        [Fact]
        public void FactMethodName()
        {
            Cpu.H = 0x20;
            Cpu.L = 0x10;
            Flags(x => x.Carry().HalfCarry());

            Execute(0x29);

            Assert.Equal(0x40, Cpu.H);
            Assert.Equal(0x20, Cpu.L);
            AssertFlags(x => x.ResetCarry().ResetHalfCarry());
        }

        private byte CreateOpCode(RegisterPair registerPair)
        {
            return (byte) (0x09 | registerPair << 4);
        }
    }
}