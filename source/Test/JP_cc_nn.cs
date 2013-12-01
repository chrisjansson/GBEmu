using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class JP_cc_nn : CpuTestBase
    {
        [Theory, PropertyData("Conditions")]
        public void Jumps_when_condition_is_true(JumpCondition condition)
        {
            condition.Set(Cpu);
            Cpu.ProgramCounter = 0;

            Execute(CreateOpCode(condition), 0x15, 0x20);

            Assert.Equal(0x2015, Cpu.ProgramCounter);
            AdvancedClock(4);
        }

        [Theory, PropertyData("Conditions")]
        public void Continues_when_condition_is_false(JumpCondition condition)
        {
            condition.Reset(Cpu);
            Cpu.ProgramCounter = 1823;

            Execute(CreateOpCode(condition));

            Assert.Equal(1826, Cpu.ProgramCounter);
            AdvancedClock(3);
        }

        private byte CreateOpCode(JumpCondition condition)
        {
            return (byte) (0xC2 | condition << 3);
        }

        public static IEnumerable<object[]> Conditions
        {
            get
            {
                return JumpCondition.GetAll()
                    .Select(x => new[] {x})
                    .ToList();
            }
        } 
    }
}