using System.Collections.Generic;
using System.Linq;
using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class OR_r
    {
        private Cpu _cpu;

        public OR_r()
        {
            var cpuFixture = new CpuFixture();
            _cpu = cpuFixture.Cpu;
        }

        [Theory, PropertyData("AllRegistersExceptA")]
        public void ORs_value_of_register_and_stores_register_result_in_A(RegisterMapping register)
        {
            _cpu.ProgramCounter = 9381;
            _cpu.Cycles = 938711;
            _cpu.A = 0x12;
            _cpu.Z = 1;
            register.Set(_cpu, 0x48);

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(0x5A, _cpu.A);
            Assert.Equal(0, _cpu.Z);
            Assert.Equal(9382, _cpu.ProgramCounter);
            Assert.Equal(938712, _cpu.Cycles);
        }

        [Fact]
        public void ORs_value_of_A_and_stores_register_result_in_A()
        {
            _cpu.ProgramCounter = 9381;
            _cpu.Cycles = 938711;
            _cpu.A = 0x12;
            _cpu.Z = 1;

            _cpu.Execute(CreateOpCode(RegisterMapping.A));

            Assert.Equal(0x12, _cpu.A);
            Assert.Equal(0, _cpu.Z);
            Assert.Equal(9382, _cpu.ProgramCounter);
            Assert.Equal(938712, _cpu.Cycles);
        }

        [Theory, PropertyData("AllRegisters")]
        public void Resets_flags(RegisterMapping register)
        {
            _cpu.HC = 1;
            _cpu.N = 1;
            _cpu.Carry = 1;

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(0, _cpu.HC);
            Assert.Equal(0, _cpu.N);
            Assert.Equal(0, _cpu.Carry);
        }

        [Theory, PropertyData("AllRegisters")]
        public void Sets_zero_when_result_is_zero(RegisterMapping register)
        {
            _cpu.Z = 0;

            _cpu.Execute(CreateOpCode(register));

            Assert.Equal(1, _cpu.Z);
        }

        private static byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0xB0 | register);
        }

        public static IEnumerable<object[]> AllRegisters
        {
            get
            {
                return RegisterMapping.GetAll()
                    .Select(x => new object[] {x})
                    .ToList();
            }
        }

        public static IEnumerable<object[]> AllRegistersExceptA
        {
            get
            {
                return RegisterMapping.GetAll()
                    .Where(x => x != RegisterMapping.A)
                    .Select(x => new object[] { x })
                    .ToList();
            }
        } 
    }
}