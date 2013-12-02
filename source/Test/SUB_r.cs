using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class SUB_r : RegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x90 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Sets_z_when_result_is_zero(RegisterMapping register)
        {
            Flags(x => x.ResetZero().Carry().HalfCarry());
            Cpu.A = 0xAB;
            register.Set(Cpu, 0xAB);

            Execute(CreateOpCode(register));

            Assert.Equal(0, Cpu.A);
            AssertFlags(x => x.SetZero().ResetCarry().ResetCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Resets_z_when_A_is_not_zero(RegisterMapping register)
        {
            Flags(x => x.Zero().ResetCarry().ResetHalfCarry());
            Cpu.A = 0x34;
            register.Set(Cpu, 0xAB);

            Cpu.Execute(CreateOpCode(register));

            Assert.Equal(0x89, Cpu.A);
            AssertFlags(x => x.ResetZero().SetCarry().SetHalfCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Sets_half_carry_when_borrow_from_4th_to_3rd_bit(RegisterMapping register)
        {
            Flags(x => x.ResetHalfCarry().ResetCarry());
            Cpu.A = 0x00;
            register.Set(Cpu, 0x1);

            Cpu.Execute(CreateOpCode(register));

            Assert.Equal(0xFF, Cpu.A);
            AssertFlags(x => x.SetHalfCarry().SetCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Sets_subtract(RegisterMapping register)
        {
            Flags(x => x.ResetSubtract());

            Cpu.Execute(CreateOpCode(register));

            AssertFlags(x => x.SetSubtract());
        }
    }
}