using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class DEC_r : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;

        [Theory, PropertyData("Registers")]
        public void Decrements_r(RegisterMapping register)
        {
            register.Set(_cpu, 0x05);
            _cpu.ProgramCounter = 18349;
            _cpu.Cycles = 934821;

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(0x04, register.Get(_cpu));
            Assert.Equal(18350, _cpu.ProgramCounter);
            Assert.Equal(934822, _cpu.Cycles);
        }

        [Theory, PropertyData("Registers")]
        public void Sets_hc_when_borrow_from_bit_4(RegisterMapping register)
        {
            register.Set(_cpu, 0xA0);
            _cpu.HC = 0;

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(1, _cpu.HC);
        }

        [Theory, PropertyData("Registers")]
        public void Resets_hc_when_not_borrow_from_bit_4(RegisterMapping register)
        {
            register.Set(_cpu, 0x01);
            _cpu.HC = 1;

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(0, _cpu.HC);
        }

        [Theory, PropertyData("Registers")]
        public void Sets_N_when_dec(RegisterMapping register)
        {
            _cpu.N = 0;

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(1, _cpu.N);
        }

        [Theory, PropertyData("Registers")]
        public void Unsets_z_when_result_is_non_zero(RegisterMapping register)
        {
            _cpu.Z = 1;
            register.Set(_cpu, 0x02);

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(0, _cpu.Z);
        }

        [Theory, PropertyData("Registers")]
        public void Sets_z_when_result_is_zero(RegisterMapping register)
        {
            _cpu.Z = 0;
            register.Set(_cpu, 0x01);

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(1, _cpu.Z);
        }

        private byte CreateOpCode(RegisterMapping registerMapping)
        {
            return (byte)(0x05 | registerMapping << 3);
        }

        public static IEnumerable<object[]> Registers
        {
            get { return RegisterMapping.GetAll().Select(x => new object[] { x }); }
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
        }
    }
}