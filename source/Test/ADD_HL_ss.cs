using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class ADD_HL_ss : Arithmetic16BitTestBase
    {
        [Theory, PropertyData("ArithmeticRegisterPairs")]
        public void Advances_counters(RegisterPair registerPair)
        {
            Execute(CreateOpCode(registerPair));

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Theory, PropertyData("RegisterPairsExceptHL")]
        public void Adds_ss_to_HL(RegisterPair registerPair)
        {
            Flags(x => x.ResetCarry().ResetHalfCarry().Subtract());
            Cpu.H = 0x8A;
            Cpu.L = 0x23;
            registerPair.Set(Cpu, 0x8B, 0x22);

            Execute(CreateOpCode(registerPair));

            Assert.Equal(0x1545, RegisterPair.HL.Get(Cpu));
            AssertFlags(x => x.ResetSubtract().SetCarry().SetHalfCarry());
        }

        [Theory, PropertyData("RegisterPairsExceptHL")]
        public void Resets_C_HC(RegisterPair registerPair)
        {
            Flags(x => x.Carry().HalfCarry());
            Cpu.H = 0x20;
            Cpu.L = 0x10;
            registerPair.Set(Cpu, 0x21, 0x11);

            Execute(CreateOpCode(registerPair));

            Assert.Equal(0x4121, RegisterPair.HL.Get(Cpu));
            AssertFlags(x => x.ResetCarry().ResetHalfCarry());
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
        public void Adds_HL_to_HL_and_resets_C_HC()
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

        public static IEnumerable<object[]> RegisterPairsExceptHL
        {
            get
            {
                return new[]
                {
                    new[] {RegisterPair.SP},
                    new[] {RegisterPair.BC},
                    new[] {RegisterPair.DE}
                };
            }
        }
    }
}