using Xunit.Extensions;

namespace Test
{
    public class CP_r : RegisterTestBase
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0xB8 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Sets_z_when_A_is_equal_to_r(RegisterMapping register)
        {
            Flags(x => x.ResetZero().Carry().HalfCarry());
            Cpu.A = 0xAB;
            register.Set(Cpu, 0xAB);

            Execute(CreateOpCode(register));

            AssertFlags(x => x.SetZero().ResetCarry().ResetHalfCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Resets_z_when_A_is_not_equal_to_r(RegisterMapping register)
        {
            Flags(x => x.Zero().ResetCarry().ResetHalfCarry());
            Cpu.A = 0x34;
            register.Set(Cpu, 0xAB);

            Cpu.Execute(CreateOpCode(register));

            AssertFlags(x => x.ResetZero().SetCarry().SetHalfCarry());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Sets_half_carry_when_borrow_from_4th_to_3rd_bit(RegisterMapping register)
        {
            Flags(x => x.ResetHalfCarry().ResetCarry());
            Cpu.A = 0x00;
            register.Set(Cpu, 0x1);

            Cpu.Execute(CreateOpCode(register));

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