using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_qqm_A : CpuTestBase
    {
        [Theory, PropertyData("RegisterPairs")]
        public void Stores_A_at_memory_qq(RegisterPair registerPair, byte opCode)
        {
            Cpu.A = 123;
            registerPair.Set(Cpu, 0x20, 0xAC);

            Cpu.Execute(opCode);

            Assert.Equal(123, FakeMmu.GetByte(0x20AC));
            AdvancedProgramCounter(1);
            AdvancedClock(2);
        }

        public static IEnumerable<object[]> RegisterPairs
        {
            get
            {
                return new[]
                {
                    new object[]{ RegisterPair.DE, (byte)0x12},
                    new object[]{ RegisterPair.BC, (byte)0x02},
                };
            }
        }
    }
}