using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    [TestFixture]
    public class LD_r_r : TestBase
    {
        [Test]
        [TestCaseSource("LDCombinations")]
        public void LD_r_rp(RegisterMapping source, RegisterMapping target)
        {
            var expectedRegisterValue = Fixture.Create<byte>();
            source.Set(Sut, expectedRegisterValue);
            var opcode = Build.LD.From(source).To(target);
            var initialProgramCounter = Fixture.Create<ushort>();
            Sut.ProgramCounter = initialProgramCounter;
            var initialCycles = Fixture.Create<long>();
            Sut.Cycles = initialCycles;

            Sut.Execute(opcode);

            Assert.AreEqual(expectedRegisterValue, target.Get(Sut));
            Assert.AreEqual(initialProgramCounter + 1, Sut.ProgramCounter);
            Assert.AreEqual(initialCycles + 1, Sut.Cycles);
        }

        private static IEnumerable<TestCaseData> LDCombinations()
        {
            return from fromRegister in RegisterMapping.GetAll()
                from toRegister in RegisterMapping.GetAll()
                select CreateTestCaseData(fromRegister, toRegister);
        }

        private static TestCaseData CreateTestCaseData(RegisterMapping from, RegisterMapping to)
        {
            var testCaseData = new TestCaseData(@from, to);
            var opcode = Build.LD.From(@from).To(to);
            testCaseData.SetName(string.Format("LD {0}, {1}. Opcode: 0x{2:X}", to, from, opcode));
            return testCaseData;
        }
    }
}
