using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    [TestFixture]
    public class LD_r_HL : TestBase
    {
        [Test]
        [TestCaseSource("CreateTestCases")]
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

            Assert.AreEqual(expectedLoadedValue, register.Get(Sut));
            Assert.AreEqual(initialProgramCounter + 1, Sut.ProgramCounter);
            Assert.AreEqual(initialCycles + 2, Sut.Cycles);
        }

        private IEnumerable<TestCaseData> CreateTestCases()
        {
            return RegisterMapping.GetAll()
                .Select(x =>
                {
                    var testCase = new TestCaseData(x);
                    var opcode = CreateOpcode(x);
                    testCase.SetName(string.Format("LD {0}, (HL) Opcode: 0x{1:x8}", x, opcode));
                    return testCase;
                })
                .ToList();
        }

        private byte CreateOpcode(RegisterMapping register)
        {
            return (byte)(0x46 | register << 3);
        }
    }
}
