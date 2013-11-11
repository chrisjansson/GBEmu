using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class PUSH_qq : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Theory, PropertyData("RegisterPairs")]
        public void Pushes_HL_to_the_stack(RegisterPair registerPair)
        {
            _cpu.ProgramCounter = 2911;
            _cpu.Cycles = 2912349;
            _cpu.SP = 33812;
            registerPair.Set(_cpu, 0xAe, 0xBC);

            _cpu.Execute(CreateOpCode(registerPair));

            Assert.Equal(0xAE, _fakeMmu.GetByte(33811));
            Assert.Equal(0xBC, _fakeMmu.GetByte(33810));
            Assert.Equal(2912, _cpu.ProgramCounter);
            Assert.Equal(2912353, _cpu.Cycles);
        }

        [Theory, PropertyData("RegisterPairs")]
        public void Decrements_sp(RegisterPair registerPair)
        {
            _cpu.SP = 33812;

            _cpu.Execute(CreateOpCode(registerPair));

            Assert.Equal(33810, _cpu.SP);
        }

        public static IEnumerable<object[]> RegisterPairs
        {
            get
            {
                return RegisterPair.GetAll()
                    .Select(x => new object[] {x});
            }
        }

        private byte CreateOpCode(RegisterPair registerPair)
        {
            return (byte)(0xC5 | (registerPair << 4));
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}