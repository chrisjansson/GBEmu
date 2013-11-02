using System.Collections.Generic;
using System.Linq;
using Core;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_HL_r
    {
        private readonly FakeMmu _fakeMmu;
        private readonly Cpu _cpu;
        private readonly Fixture _fixture;

        public LD_HL_r()
        {
            _fakeMmu = new FakeMmu();
            _cpu = new Cpu(_fakeMmu);
            _fixture = new Fixture();
        }

        [Theory, PropertyData("NonDestructiveLDCombinations")]
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

        [Fact]
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

        [Fact]
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

        [Theory, PropertyData("AllLDCombinations")]
        public void LD_HL_r_advances_program_counter_and_clock(RegisterMapping source, byte value)
        {
            var opCode = CreateOpCode(source);
            _cpu.ProgramCounter = 200;
            _cpu.Cycles = 500;

            _cpu.Execute(opCode);

            Assert.Equal(201, _cpu.ProgramCounter);
            Assert.Equal(502, _cpu.Cycles);
        }

        private void AssertHLPointsTo(byte expectedValue, byte h, byte l)
        {
            var address = (h << 8) | l;
            var actual = _fakeMmu.GetByte((ushort)address);
            Assert.Equal(expectedValue, actual);
        }

        public static IEnumerable<object[]> NonDestructiveLDCombinations
        {
            get
            {
                var fixture = new Fixture();
                return RegisterMapping.GetAll()
                    .Where(x => x != RegisterMapping.H && x != RegisterMapping.L)
                    .Select(x => CreateTestCase(x, fixture))
                    .ToList();
            }
        }

        public static IEnumerable<object[]> AllLDCombinations
        {
            get
            {
                var fixture = new Fixture();
                return RegisterMapping.GetAll()
                    .Select(x => CreateTestCase(x, fixture))
                    .ToList();
            }
        }

        private static object[] CreateTestCase(RegisterMapping register, Fixture fixture)
        {
            var value = fixture.Create<byte>();
            return new object[] { register, (byte)123 };
        }

        private static byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x70 | register);
        }
    }
}