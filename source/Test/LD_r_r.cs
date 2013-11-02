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
            var initialProgramCounter = Fixture.Create<ushort>();
            var initialCycles = Fixture.Create<long>();
            source.Set(Sut, expectedRegisterValue);
            Sut.ProgramCounter = initialProgramCounter;
            Sut.Cycles = initialCycles;

            var opcode = CreateOpcode(source, target);
            Sut.Execute(opcode);

            Assert.AreEqual(expectedRegisterValue, target.Get(Sut));
            Assert.AreEqual(initialProgramCounter + 1, Sut.ProgramCounter);
            Assert.AreEqual(initialCycles + 1, Sut.Cycles);
        }

        private static byte CreateOpcode(RegisterMapping source, RegisterMapping target)
        {
            return Build.LD.From(source).To(target);
        }

        private static IEnumerable<TestCaseData> LDCombinations()
        {
            return from fromRegister in RegisterMapping.GetAll()
                from toRegister in RegisterMapping.GetAll()
                select CreateTestCaseData(fromRegister, toRegister);
        }

        private static TestCaseData CreateTestCaseData(RegisterMapping source, RegisterMapping target)
        {
            var testCaseData = new TestCaseData(source, target);
            var opcode = CreateOpcode(source, target);
            testCaseData.SetName(string.Format("LD {0}, {1}. Opcode: 0x{2:X}", target, source, opcode));
            return testCaseData;
        }
    }
}
