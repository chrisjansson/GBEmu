using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class DEC_ss : CpuTestBase
    {
        [Theory, PropertyData("RegisterPairs")]
        public void Increments_ss(RegisterPair registerPair)
        {
            registerPair.Set(Cpu, 0x10, 0x00);
            Cpu.ProgramCounter = 47311;
            Cpu.Cycles = 92;

            Cpu.Execute(CreateOpCode(registerPair));

            Assert.Equal(0x0FFF, registerPair.Get(Cpu));
            Assert.Equal(47312, Cpu.ProgramCounter);
            Assert.Equal(94, Cpu.Cycles);
        }

        private static byte CreateOpCode(RegisterPair registerPair)
        {
            return (byte)(0x0B | registerPair << 4);
        }

        public static IEnumerable<object[]> RegisterPairs
        {
            get
            {
                return new List<object[]>
                {
                    new object[] {RegisterPair.HL},
                    new object[] {RegisterPair.BC},
                    new object[] {RegisterPair.DE},
                    new object[] {RegisterPair.SP}
                };
            }
        }
         
    }
}