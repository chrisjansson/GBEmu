using Xunit;

namespace Test.CpuA
{
    public class Halt : CpuTestBase
    {
        private const byte OpCode = 0x76;

        [Fact]
        public void Advances_program_counter_when_interupts_are_enabled()
        {
            Cpu.IME = true;

            Execute(OpCode);

            AdvancedProgramCounter(1);
            AdvancedClock(1);
        }

        [Fact]
        public void Does_not_continue_executing_next_instruction_after_halted_but_cycles_pass()
        {
            Cpu.IME = true;

            Execute(OpCode);
            Execute(0x40);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        [Fact]
        public void Resumes_at_interrupt_vector_when_interrupt_is_raised()
        {
            Cpu.IME = true;
            FakeMmu.Memory[0x0050] = 0xC9; //RET
            Execute(OpCode); //HALT

            Cpu.IE = 0x04;
            Cpu.IF = 0x04;
            Execute(0x00); //NOP

            Assert.Equal(0x0050, Cpu.ProgramCounter);
            Assert.False(Cpu.Halted);
        }
    }
}