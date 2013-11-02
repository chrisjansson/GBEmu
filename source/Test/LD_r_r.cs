using System.Collections.Generic;
using System.Linq;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_r_r : TestBase
    {
        [Theory, PropertyData("LDCombinations")]
        public void LD_r_rp(RegisterMapping source, RegisterMapping target)
        {
            var expectedRegisterValue = Fixture.Create<byte>();
            var initialProgramCounter = Fixture.Create<ushort>();
            var initialCycles = Fixture.Create<long>();
            source.Set(Sut, expectedRegisterValue);
            Sut.ProgramCounter = initialProgramCounter;
            Sut.Cycles = initialCycles;

            var opcode = CreateOpcode(source, target);
            Sut.Execute(opcode);

            Assert.Equal(expectedRegisterValue, target.Get(Sut));
            Assert.Equal(initialProgramCounter + 1, Sut.ProgramCounter);
            Assert.Equal(initialCycles + 1, Sut.Cycles);
        }

        public static IEnumerable<object[]> LDCombinations
        {
            get
            {
                return from fromRegister in RegisterMapping.GetAll()
                       from toRegister in RegisterMapping.GetAll()
                       select new object[] { fromRegister, toRegister};
            }

        }

        private static byte CreateOpcode(RegisterMapping source, RegisterMapping target)
        {
            return Build.LD.From(source).To(target);
        }
    }
}
