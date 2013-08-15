using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    [TestFixture]
    public class LD_r_n : TestBase
    {
        [Test]
        [TestCaseSource("GetRegisterCombinations")]
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

            Assert.AreEqual(expectedConstant, target.Get(Sut));
            Assert.AreEqual(programCounter + 2, Sut.ProgramCounter);
            Assert.AreEqual(initialCyles + 2, Sut.Cycles);
        }

        private byte CreateOpcode(RegisterMapping register)
        {
            return (byte)(0x06 | (register << 3));
        }

        private IEnumerable<TestCaseData> GetRegisterCombinations()
        {
            return RegisterMapping.GetAll()
                .Select(x =>
                {
                    var testCaseData = new TestCaseData(x);
                    testCaseData.SetName(string.Format("LD {0}, n Opcode: 0x{1:x8}", x, CreateOpcode(x)));

                    return testCaseData;
                })
                .ToList();
        }
    }
}
