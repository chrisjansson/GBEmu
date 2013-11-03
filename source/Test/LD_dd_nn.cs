using System.Collections.Generic;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_dd_nn : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Theory, PropertyData("RegisterPairs")]
        public void LD_dd_nn_loads_nn_into_hl_registers(int dd, RegisterMapping high, RegisterMapping low)
        {
            byte opcode = (byte)((dd << 4) | 0x01);
            _cpu.ProgramCounter = 10294;
            _cpu.Cycles = 9823;
            _fakeMmu.SetByte((ushort)(_cpu.ProgramCounter + 1), 194);
            _fakeMmu.SetByte((ushort)(_cpu.ProgramCounter + 2), 205);

            _cpu.Execute(opcode);

            Assert.Equal(205, high.Get(_cpu));
            Assert.Equal(194, low.Get(_cpu));
            Assert.Equal(10297, _cpu.ProgramCounter);
            Assert.Equal(9826, _cpu.Cycles);
        }

        public static IEnumerable<object[]> RegisterPairs
        {
            get
            {
                return
                    new List<object[]>
                    {
                        new object[]{0x02, RegisterMapping.H, RegisterMapping.L},
                        new object[]{0x01, RegisterMapping.D, RegisterMapping.E}
                    };
            }
        }

        public void SetFixture(CpuFixture fixture)
        {
            _cpu = fixture.Cpu;
            _fakeMmu = fixture.FakeMmu;
        }
    }
}