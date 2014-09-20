using System.Collections.Generic;

namespace Test.CpuA.Interrupts
{
    public class InterruptTestBase : CpuTestBase
    {
        public static IEnumerable<object[]> Interrupts
        {
            get
            {
                return new[]
                {
                    new[] {Interrupt.Timer},
                    new[] {Interrupt.VBlank},
                };
            }
        }

        protected void Enable(Interrupt interrupt)
        {
            Cpu.IE = interrupt.InterruptMask;
        }

        protected void Request(Interrupt interrupt)
        {
            Cpu.IF = interrupt.InterruptMask;
        }
    }
}