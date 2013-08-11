using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Core;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    [TestFixture]
    public class LD_r_r
    {
        private Cpu _sut;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture();
            _sut = new Cpu();
        }

        [Test]
        [TestCaseSource("LDCombinations")]
        public void LD(RegisterMapping from, RegisterMapping to)
        {
            AssertLD(from, to);
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

        [Test]
        public void LD_B_B()
        {
            AssertLD(RegisterMapping.B, RegisterMapping.B);
        }

        [Test]
        public void LD_B_C()
        {
            AssertLD(RegisterMapping.C, RegisterMapping.B);
        }

        [Test]
        public void LD_B_D()
        {
            AssertLD(RegisterMapping.D, RegisterMapping.B);
        }

        [Test]
        public void LD_B_E()
        {
            AssertLD(RegisterMapping.E, RegisterMapping.B);
        }

        [Test]
        public void LD_B_H()
        {
            AssertLD(RegisterMapping.H, RegisterMapping.B);
        }

        [Test]
        public void LD_B_L()
        {
            AssertLD(RegisterMapping.L, RegisterMapping.B);
        }

        private void AssertLD(RegisterMapping from, RegisterMapping to)
        {
            var expectedRegisterValue = _fixture.Create<byte>();
            @from.Set(_sut, expectedRegisterValue);
            var opcode = Build.LD.From(@from).To(to);

            _sut.Execute(opcode);

            Assert.AreEqual(expectedRegisterValue, to.Get(_sut));
        }
    }
}
