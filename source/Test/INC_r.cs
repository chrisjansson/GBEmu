using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class INC_r : IUseFixture<CpuFixture>
    {
        private Cpu _sut;

        [Theory, PropertyData("GetRegisters")]
        public void Increments_contents_of_register(RegisterMapping register)
        {
            register.Set(_sut, 0xFF);
            _sut.N = 1;
            _sut.ProgramCounter = 8471;
            _sut.Cycles = 92371;

            var instruction = CreateOpCode(register);
            _sut.Execute(instruction);

            Assert.Equal(0x00, register.Get(_sut));
            Assert.Equal(0, _sut.N);
            Assert.Equal(8472, _sut.ProgramCounter);
            Assert.Equal(92372, _sut.Cycles);
        }

        [Theory, PropertyData("GetRegisters")]
        public void Sets_half_carry_if_lower_nibble_overflows(RegisterMapping register)
        {
            register.Set(_sut, 0x0F);
            _sut.HC = 0;

            var instruction = CreateOpCode(register);
            _sut.Execute(instruction);

            Assert.Equal(1, _sut.HC);
        }

        [Theory, PropertyData("GetRegisters")]
        public void Unsets_half_carry_if_lower_nibble_overflows(RegisterMapping register)
        {
            register.Set(_sut, 0x01);
            _sut.HC = 1;

            var instruction = CreateOpCode(register);
            _sut.Execute(instruction);

            Assert.Equal(0, _sut.HC);
        }

        [Theory, PropertyData("GetRegisters")]
        public void Sets_zero_if_resulting_operation_results_in_zero(RegisterMapping register)
        {
            register.Set(_sut, 0xFF);
            _sut.Z = 0;

            var instruction = CreateOpCode(register);
            _sut.Execute(instruction);

            Assert.Equal(1, _sut.Z);
        }

        [Theory, PropertyData("GetRegisters")]
        public void Unsets_zero_if_resulting_operation_does_not_result_in_zero(RegisterMapping register)
        {
            register.Set(_sut, 0x05);
            _sut.Z = 1;

            var instruction = CreateOpCode(register);
            _sut.Execute(instruction);

            Assert.Equal(0, _sut.Z);
        }

        public void SetFixture(CpuFixture data)
        {
            _sut = data.Cpu;
        }

        public static IEnumerable<object[]> GetRegisters
        {
            get
            {
                return RegisterMapping.GetAll()
                    .Select(x => new[] { x })
                    .ToList();
            }
        }

        private byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x04 | (register << 3));
        }
    }
}