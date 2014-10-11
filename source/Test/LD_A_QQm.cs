using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_A_QQm : CpuTestBase
    {
        [Theory, PropertyData("RegisterPairs")]
        public void Loads_memory_at_qq_into_A(RegisterPair registerPair, byte opCode)
        {
            registerPair.Set(Cpu, 0xAB, 0xFA);
            FakeMmu.SetByte(0xABFA, 0x32);

            Execute(opCode);

            AdvancedProgramCounter(1);
            AdvancedClock(2);
            Assert.Equal(0x32, Cpu.A);
        }

        public static IEnumerable<object[]> RegisterPairs
        {
            get
            {
                return new[]
                {
                    new object[] {RegisterPair.DE, (byte)0x1A},
                    new object[] {RegisterPair.BC, (byte) 0x0A},
                };
            }
        }
    }
}