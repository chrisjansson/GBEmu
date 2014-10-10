using System;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuA.Interrupts
{
    public class InterruptTests
    {
        public class EnabledAndRequested : InterruptTestBase
        {
            public EnabledAndRequested()
            {
                Cpu.IME = true;
            }

            [Theory, PropertyData("Interrupts")]
            public void Sets_program_counter_to_interrupt_vector(Interrupt interrupt)
            {
                Enable(interrupt);
                Request(interrupt);

                Cpu.Execute(0x00);

                Assert.Equal(interrupt.InterruptVector, Cpu.ProgramCounter);
            }

            [Theory, PropertyData("Interrupts")]
            public void Restes_master_interrupt_enable(Interrupt interrupt)
            {
                Enable(interrupt);
                Request(interrupt);

                Cpu.Execute(0x00);

               Assert.False(Cpu.IME);
            }

            [Theory, PropertyData("Interrupts")]
            public void Pushes_current_program_counter_onto_the_stack(Interrupt interrupt)
            {
                Enable(interrupt);
                Request(interrupt);
                var originalProgramCounter = Cpu.ProgramCounter;

                Cpu.Execute(0x00);

                DecrementedStackPointer(2);
                var low = FakeMmu.GetByte(Cpu.SP);
                var high = FakeMmu.GetByte((ushort)(Cpu.SP + 1));
                Assert.Equal(originalProgramCounter, high << 8 | low);
            }

            [Theory, PropertyData("Interrupts")]
            public void Resets_interrupt_request(Interrupt interrupt)
            {
                Enable(interrupt);
                Request(interrupt);

                Cpu.Execute(0x00);

                Assert.Equal(0, Cpu.IF);
            }

            [Theory, PropertyData("Interrupts")]
            public void Only_resets_own_interrupt_request(Interrupt interrupt)
            {
                Enable(interrupt);
                Cpu.IF = 0x1F;

                Cpu.Execute(0x00);

                var expectedRequest = ~interrupt.InterruptMask & 0x1F;
                Assert.Equal(expectedRequest, Cpu.IF);
            }

            [Fact]
            public void Interrupt_routine_takes_five_cycles()
            {
                throw new NotImplementedException();
            }
        }

        public class EnabledAndNotRequested : InterruptTestBase
        {
            public EnabledAndNotRequested()
            {
                Cpu.IF = 0;
                Cpu.IME = true;
            }

            [Theory, PropertyData("Interrupts")]
            public void Does_not_execute_interrupt(Interrupt interrupt)
            {
                Enable(interrupt);

                Cpu.Execute(0x00);

                AdvancedProgramCounter(1);
                DecrementedStackPointer(0);

            }
        }

        public class NotEnabledAndRequested : InterruptTestBase
        {
            public NotEnabledAndRequested()
            {
                Cpu.IE = 0;
                Cpu.IME = true;
            }

            [Theory, PropertyData("Interrupts")]
            public void Does_not_execute_interrupt(Interrupt interrupt)
            {
                Request(interrupt);

                Cpu.Execute(0x00);

                AdvancedProgramCounter(1);
                DecrementedStackPointer(0);
            }
        }

    }
}