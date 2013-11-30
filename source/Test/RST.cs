using System.Collections.Generic;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class RST : CpuTestBase
    {
        [Theory, PropertyData("Combinations")]
        public void Pushes_program_counter_to_stack_and_sets_program_counter_to_expected(int t, int expectedProgramCounter)
        {
            Cpu.ProgramCounter = 0x8213;
            Cpu.SP = 0x8012;

            Execute(CreateOpCode(t));

            Assert.Equal(expectedProgramCounter, Cpu.ProgramCounter);
            Assert.Equal(0x82, FakeMmu.GetByte(0x8011));
            Assert.Equal(0x14, FakeMmu.GetByte(0x8010));
            Assert.Equal(0x8010, Cpu.SP);
            AdvancedClock(4);
        }

        public byte CreateOpCode(int t)
        {
            return (byte) (0xC7 | t << 3);
        }

        public static IEnumerable<object[]> Combinations
        {
            get
            {
                return new List<object[]>
                {
                    new object[]{ 0x0, 0x0000 },
                    new object[]{ 0x1, 0x0008 },
                    new object[]{ 0x2, 0x0010 },
                    new object[]{ 0x3, 0x0018 },
                    new object[]{ 0x4, 0x0020 },
                    new object[]{ 0x5, 0x0028 },
                    new object[]{ 0x6, 0x0030 },
                    new object[]{ 0x7, 0x0038 },
                };
            }
        } 
    }
}