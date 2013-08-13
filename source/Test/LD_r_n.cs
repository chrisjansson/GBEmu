using System.Collections.Generic;
using System.Linq;
using Core;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    public class FakeMmu : IMmu
    {
        public readonly byte[] Memory = new byte[ushort.MaxValue];

        public byte GetByte(ushort address)
        {
            return Memory[address];
        }
    }

    [TestFixture]
    public class LD_r_n : TestBase
    {
        [Test]
        [TestCaseSource("GetRegisterCombinations")]
        public void LD_r_n_immediate_data(RegisterMapping target)
        {
            var opcode = (byte) (0x06 | (target << 3));
            var expectedConstant = Fixture.Create<byte>();
            var programCounter = Fixture.Create<ushort>();
            Sut.ProgramCounter = programCounter;
            FakeMmu.Memory[programCounter + 1] = expectedConstant;

            Sut.Execute(opcode);

            Assert.AreEqual(expectedConstant, target.Get(Sut));
        }

        private IEnumerable<TestCaseData> GetRegisterCombinations()
        {
            return RegisterMapping.GetAll()
                .Select(x =>
                {
                    var testCaseData = new TestCaseData(x);
                    testCaseData.SetName(string.Format("LD {0}, n Opcode: 0x{1:x8}", x, (0x06 | (x << 3))));

                    return testCaseData;
                })
                .ToList();
        } 
    }
}
