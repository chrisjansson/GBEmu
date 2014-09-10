using System;
using Xunit;

namespace Test.CpuA.Interrupts
{
    public class TimerInterruptTests
    {
        public class EnabledAndRequested : CpuTestBase
        {
            public EnabledAndRequested()
            {
                Cpu.IE = 0x04;
                Cpu.IF = 0x04;
                Cpu.IME = true;
            }

            [Fact]
            public void Sets_program_counter_to_interrupt_vector()
            {
                Cpu.Execute(0x00);

                Assert.Equal(0x50, Cpu.ProgramCounter);
            }

            [Fact]
            public void Pushes_current_program_counter_onto_the_stack()
            {
                var originalProgramCounter = Cpu.ProgramCounter;

                Cpu.Execute(0x00);

                DecrementedStackPointer(2);
                var low = FakeMmu.GetByte(Cpu.SP);
                var high = FakeMmu.GetByte((ushort)(Cpu.SP + 1));
                Assert.Equal(originalProgramCounter, high << 8 | low);
            }

            [Fact]
            public void Resets_interrupt_request()
            {
                Cpu.Execute(0x00);

                Assert.Equal(0, Cpu.IF);
            }

            [Fact]
            public void Interrupt_routine_takes_five_cycles()
            {
                throw new NotImplementedException();
            }
        }

        public class EnabledAndNotRequested : CpuTestBase
        {
            public EnabledAndNotRequested()
            {
                Cpu.IE = 0x04;
                Cpu.IF = 0;
                Cpu.IME = true;
            }

            [Fact]
            public void Does_not_execute_interrupt()
            {
                Cpu.Execute(0x00);

                AdvancedProgramCounter(1);
                DecrementedStackPointer(0);

            }
        }

        public class NotEnabledAndRequested : CpuTestBase
        {
            public NotEnabledAndRequested()
            {
                Cpu.IE = 0;
                Cpu.IF = 0x04;
                Cpu.IME = true;
            }

            [Fact]
            public void Does_not_execute_interrupt()
            {
                Cpu.Execute(0x00);

                AdvancedProgramCounter(1);
                DecrementedStackPointer(0);
            }
        }

    }
}