using System;
using Xunit;

namespace Test.CpuA
{
    public class Halt_IME_disabled : CpuTestBase
    {
        private const byte OpCode = 0x76;

        public Halt_IME_disabled()
        {
            Cpu.IME = false;
        }

        [Fact]
        public void Halts_execution()
        {
            Execute(OpCode);

            Assert.True(Cpu.Halted);
        }

        [Fact]
        public void Executes_next_instruction_after_halt_when_interrupted()
        {
            Execute(OpCode);
            Cpu.IE = 0x04;
            Cpu.IF = 0x04;

            Cpu.Execute(0x00);

            AdvancedProgramCounter(2);
        }

        [Fact]
        public void Executes_the_same_instruction_twice_after_interrupt()
        {
            throw new NotImplementedException("DMG bug");
            //Cpu.Execute(OpCode);
            //Cpu.IE = 0x04;
            //Cpu.IF = 0x04;
            //Cpu.Execute(0x00); //Execute next instruction to wake cpu, does not advance program counter

            //Cpu.Execute(0x00); //NOP PC should not move during this op
            //Cpu.Execute(0x00);

            //AdvancedProgramCounter(2);
        }
    }
}