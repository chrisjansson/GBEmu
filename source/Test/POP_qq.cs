using System.Collections.Generic;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class POP_qq
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        public POP_qq()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
            _fakeMmu = cpuFixture.FakeMmu;
        }

        [Theory, PropertyData("RegiserPairs")]
        public void Pops_qq_from_the_stack(RegisterPair registerPair)
        {
            _cpu.ProgramCounter = 9381;
            _cpu.Cycles = 2912349;
            _cpu.SP = 33812;
            _fakeMmu.SetByte(33812, 0xBC);
            _fakeMmu.SetByte(33813, 0xAE);

            _cpu.Execute(CreateOpCode(registerPair));

            Assert.Equal(0xAEBC, registerPair.Get(_cpu));
            Assert.Equal(9382, _cpu.ProgramCounter);
            Assert.Equal(2912352, _cpu.Cycles);
        }

        [Theory, PropertyData("RegiserPairs")]
        public void Increments_sp(RegisterPair registerPair)
        {
            _cpu.SP = 33812;

            _cpu.Execute(CreateOpCode(registerPair));

            Assert.Equal(33814, _cpu.SP);
        }

        private byte CreateOpCode(RegisterPair registerPair)
        {
            return (byte)(0xC1 | registerPair << 4);
        }

        public static IEnumerable<object[]> RegiserPairs
        {
            get
            {
                return new List<object[]>
                {
                    new object[] {RegisterPair.HL},
                    new object[] {RegisterPair.AF}
                };
            }
        }
    }
}