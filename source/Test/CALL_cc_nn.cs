using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class CALL_cc_nn : CpuTestBase
    {
        [Theory, PropertyData("Conditions")]
        public void Call_sets_program_counter(JumpCondition jumpCondition)
        {
            jumpCondition.Set(Cpu);

            Execute(CreateOpCode(jumpCondition), 0x35, 0x21);

            Assert.Equal(0x2135, Cpu.ProgramCounter);
            AdvancedClock(6);
        }

        [Theory, PropertyData("Conditions")]
        public void Call_stores_return_address_at_stack_pointer(JumpCondition jumpCondition)
        {
            Cpu.ProgramCounter = 0x8243;
            Cpu.SP = 0x500;
            jumpCondition.Set(Cpu);

            Execute(CreateOpCode(jumpCondition));

            Assert.Equal(0x82, FakeMmu.GetByte(0x4FF));
            Assert.Equal(0x46, FakeMmu.GetByte(0x4FE));
        }

        [Theory, PropertyData("Conditions")]
        public void Call_decrements_stack_pointer(JumpCondition jumpCondition)
        {
            Cpu.SP = 0x833;
            jumpCondition.Set(Cpu);

            Execute(CreateOpCode(jumpCondition));

            Assert.Equal(0x831, Cpu.SP);
        }

        [Theory, PropertyData("Conditions")]
        public void Continues_when_condition_is_false(JumpCondition jumpCondition)
        {
            jumpCondition.Reset(Cpu);
            Cpu.SP = 921;
            FakeMmu.Memory[920] = 1;
            FakeMmu.Memory[919] = 2;

            Execute(CreateOpCode(jumpCondition));

            AdvancedProgramCounter(3);
            AdvancedClock(3);
            Assert.Equal(921, Cpu.SP);
            Assert.Equal(1, FakeMmu.GetByte(920));
            Assert.Equal(2, FakeMmu.GetByte(919));
        }

        private byte CreateOpCode(JumpCondition jumpCondition)
        {
            return (byte) (0xC4 | jumpCondition << 3);
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