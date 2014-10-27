using System.Collections;
using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class DAA : CpuTestBase
    {
        private const byte OpCode = 0x27;

        [Fact]
        public void Advances_counters()
        {
            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Resets_half_carry()
        {
            Flags(x => x.HalfCarry());

            Execute(OpCode);

            AssertFlags(x => x.ResetHalfCarry());
        }

        [Fact]
        public void Corrects_0x60_when_A_is_greater_than_0x99()
        {
            Flags(x => x.ResetCarry().ResetHalfCarry());
            Cpu.A = 0xA0;

            Execute(OpCode);

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Corrects_0x60_when_carry_is_set()
        {
            Flags(x => x.Carry().ResetHalfCarry());
            Cpu.A = 0;

            Execute(OpCode);

            Assert.Equal(0x60, Cpu.A);
            AssertFlags(x => x.SetCarry());
        }

        [Fact]
        public void Clears_carry_when_A_is_less_than_0x99()
        {
            Flags(x => x.ResetCarry().ResetHalfCarry());
            Cpu.A = 0;

            Execute(OpCode);

            Assert.Equal(0, Cpu.A);
            AssertFlags(x => x.ResetCarry());
        }

        [Fact]
        public void Corrects_0x06_when_lower_4_are_greater_than_0x09()
        {
            Flags(x => x.ResetHalfCarry().ResetCarry());
            Cpu.A = 0x0A;

            Execute(OpCode);

            Assert.Equal(0x10, Cpu.A);
        }

        [Fact]
        public void Corrects_0x06_when_half_carry_is_set()
        {
            Flags(x => x.ResetCarry().HalfCarry());
            Cpu.A = 0x00;

            Execute(OpCode);

            Assert.Equal(0x06, Cpu.A);
        }
    }
}