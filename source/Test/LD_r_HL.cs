using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_r_HL : TestBase
    {
        [Theory, PropertyData("CreateTestCases")]
        public void LD_r_HL_loads_memory_pointed_to_by_HL_into_r(RegisterMapping register)
        {
            var memoryLocation = Fixture.Create<ushort>();
            var expectedLoadedValue = Fixture.Create<byte>();
            var initialProgramCounter = Fixture.Create<ushort>();
            var initialCycles = Fixture.Create<long>();
            FakeMmu.Memory[memoryLocation] = expectedLoadedValue;
            Sut.H = (byte)(memoryLocation >> 8);
            Sut.L = (byte)(memoryLocation & 0xFF);
            Sut.ProgramCounter = initialProgramCounter;
            Sut.Cycles = initialCycles;
            var opcode = CreateOpcode(register);

            Sut.Execute(opcode);

            Assert.Equal(expectedLoadedValue, register.Get(Sut));
            Assert.Equal(initialProgramCounter + 1, Sut.ProgramCounter);
            Assert.Equal(initialCycles + 2, Sut.Cycles);
        }

        public static IEnumerable<object[]> CreateTestCases
        {
            get
            {
                return RegisterMapping.GetAll()
                    .Select(x => new[] {x}); 
            }
        }

        private byte CreateOpcode(RegisterMapping register)
        {
            return (byte)(0x46 | register << 3);
        }
    }
}
