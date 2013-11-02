using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_r_n : TestBase
    {
        [Theory]
        [PropertyData("GetRegisterCombinations")]
        public void LD_r_n_immediate_data(RegisterMapping target)
        {
            var expectedConstant = Fixture.Create<byte>();
            var programCounter = Fixture.Create<ushort>();
            var initialCyles = Fixture.Create<long>();
            Sut.ProgramCounter = programCounter;
            Sut.Cycles = initialCyles;
            FakeMmu.Memory[programCounter + 1] = expectedConstant;

            var opcode = CreateOpcode(target);
            Sut.Execute(opcode);

            Assert.Equal(expectedConstant, target.Get(Sut));
            Assert.Equal(programCounter + 2, Sut.ProgramCounter);
            Assert.Equal(initialCyles + 2, Sut.Cycles);
        }

        private byte CreateOpcode(RegisterMapping register)
        {
            return (byte)(0x06 | (register << 3));
        }

        public static IEnumerable<object[]> GetRegisterCombinations
        {
            get
            {
                return RegisterMapping.GetAll()
                    .Select(x => new object[] { x });
            }
        }
    }
}
