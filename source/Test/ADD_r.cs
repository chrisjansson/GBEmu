using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class ADD_r : RegisterTestBase 
    {
        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte) (0x80 | register);
        }

        [Theory, PropertyData("Registers")]
        public void Resets_subtract(RegisterMapping register)
        {
            Flags(x => x.Subtract());

            Execute(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void Adds_r_to_a(RegisterMapping register)
        {
            Flags(x => x.ResetCarry().ResetZero().ResetHalfCarry());
            Cpu.A = 0x3A;
            register.Set(Cpu, 0xC6);

            Execute(CreateOpCode(register));

            Assert.Equal(0, Cpu.A);
            AssertFlags(x => x.SetCarry().SetZero().SetHalfCarry());
        }

        [Fact]
        public void Adds_a_to_a()
        {
            Flags(x => x.Carry().Zero().ResetHalfCarry());
            Cpu.A = 0x3A;
            
            Execute(0x87);

            Assert.Equal(0x74, Cpu.A);
            AssertFlags(x => x.ResetCarry().ResetZero().SetHalfCarry());
        }

        [Theory, PropertyData("Registers")]
        public void Sets_half_carry(RegisterMapping register)
        {
            Flags(x => x.Carry().Zero().HalfCarry());
            Cpu.A = 0x11;
            register.Set(Cpu, 0x11);

            Execute(CreateOpCode(register));

            Assert.Equal(0x22, Cpu.A);
            AssertFlags(x => x.ResetCarry().ResetHalfCarry().ResetZero());
        }
    }
}