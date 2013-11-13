using System.Collections.Generic;
using System.Security.Permissions;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class INC_ss
    {
        private Cpu _cpu;

        public INC_ss()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
        }

        [Theory, PropertyData("RegisterPairsSS")]
        public void Increments_ss(RegisterPair registerPair)
        {
            registerPair.Set(_cpu, 0x10, 0x00);
            _cpu.ProgramCounter = 47311;
            _cpu.Cycles = 92;

            _cpu.Execute(CreateOpCode(registerPair));

            Assert.Equal(0x1001, registerPair.Get(_cpu));
            Assert.Equal(47312, _cpu.ProgramCounter);
            Assert.Equal(94, _cpu.Cycles);
        }

        private static byte CreateOpCode(RegisterPair registerPair)
        {
            return (byte) (0x03 | (registerPair << 4));
        }

        public static IEnumerable<object[]> RegisterPairsSS
        {
            get
            {
                return new List<object[]>
                {
                    new object[] {RegisterPair.HL},
                };
            }
        }
    }
}