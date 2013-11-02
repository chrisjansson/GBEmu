using System.Collections.Generic;
using System.Linq;
using Core;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    [TestFixture]
    public class LD_HL_r
    {
        private FakeMmu _fakeMmu;
        private Cpu _cpu;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _fakeMmu = new FakeMmu();
            _cpu = new Cpu(_fakeMmu);
            _fixture = new Fixture();
        }

        [Test]
        [TestCaseSource("NonDestructiveLDCombinations")]
        public void LD_HL_r_stores_value_of_r_in_HL(RegisterMapping source, byte value)
        {
            var instruction = CreateOpCode(source);
            
            var h = _fixture.Create<byte>();
            var l = _fixture.Create<byte>();

            RegisterMapping.H.Set(_cpu, h);
            RegisterMapping.L.Set(_cpu, l);

            source.Set(_cpu, value);

            _cpu.Execute(instruction);

            AssertHLPointsTo(value, h, l);
        }

        [Test]
        public void LD_HL_H_stores_value_of_H_in_HL()
        {
            var instruction = CreateOpCode(RegisterMapping.H);

            var h = _fixture.Create<byte>();
            var l = _fixture.Create<byte>();

            RegisterMapping.H.Set(_cpu, h);
            RegisterMapping.L.Set(_cpu, l);

            _cpu.Execute(instruction);

            AssertHLPointsTo(h, h, l);
        }

        [Test]
        public void LD_HL_L_stores_value_of_L_in_HL()
        {
            var instruction = CreateOpCode(RegisterMapping.L);

            var h = _fixture.Create<byte>();
            var l = _fixture.Create<byte>();

            RegisterMapping.H.Set(_cpu, h);
            RegisterMapping.L.Set(_cpu, l);

            _cpu.Execute(instruction);

            AssertHLPointsTo(l, h, l);
        }

        [Test]
        [TestCaseSource("AllLDCombinations")]
        public void LD_HL_r_advances_program_counter_and_clock(RegisterMapping source, byte value)
        {
            var opCode = CreateOpCode(source);
            _cpu.ProgramCounter = 200;
            _cpu.Cycles = 500;

            _cpu.Execute(opCode);

            Assert.AreEqual(201, _cpu.ProgramCounter, "Expected program counter to increment by 1");
            Assert.AreEqual(502, _cpu.Cycles, "Expected cycles to advance by 2");
        }

        private void AssertHLPointsTo(byte expectedValue, byte h, byte l)
        {
            var address = (h << 8) | l;
            var actual = _fakeMmu.GetByte((ushort)address);
            Assert.AreEqual(expectedValue, actual);
        }

        private static IEnumerable<TestCaseData> NonDestructiveLDCombinations()
        {
            var fixture = new Fixture();
            return RegisterMapping.GetAll()
                .Where(x => x != RegisterMapping.H && x != RegisterMapping.L)
                .Select(x => CreateTestCase(x, fixture))
                .ToList();
        }

        private static IEnumerable<TestCaseData> AllLDCombinations()
        {
            var fixture = new Fixture();
            return RegisterMapping.GetAll()
                .Select(x => CreateTestCase(x, fixture))
                .ToList();
        }

        private static TestCaseData CreateTestCase(RegisterMapping register, Fixture fixture)
        {
            var value = fixture.Create<byte>();
            var testCaseData = new TestCaseData(register, value);
            var opcode = CreateOpCode(register);
            testCaseData.SetName(string.Format("LD (HL), {0}. Opcode: 0x{1:X}", register, opcode));
            return testCaseData;
        }

        private static byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x70 | register);
        }
    }
}