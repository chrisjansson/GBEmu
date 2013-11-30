using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class XOR_r : RegisterTestBase
    {
        [Theory, PropertyData("Registers")]
        public void Resets_flags(RegisterMapping register)
        {
            Flags(x => x.Subtract().Carry().HalfCarry());

            Execute(CreateOpCode(register));
            
            AssertFlags(x => x.ResetCarry().ResetHalfCarry().ResetSubtract());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void XORs_register_with_A(RegisterMapping register)
        {
            Flags(x => x.Zero());
            Cpu.A = 0x96;
            register.Set(Cpu, 0x5D);

            Execute(CreateOpCode(register));

            Assert.Equal(0xCB, Cpu.A);
            AssertFlags(x => x.ResetZero());
        }

        [Theory, PropertyData("RegistersExceptA")]
        public void XORs_register_with_A_sets_zero(RegisterMapping register)
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0xF0;
            register.Set(Cpu, 0xF0);

            Execute(CreateOpCode(register));

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero());
        }

        [Fact]
        public void XORs_A_with_A_and_sets_zero()
        {
            Flags(x => x.ResetZero());
            Cpu.A = 0xAB;

            Execute(0xAF);

            Assert.Equal(0x00, Cpu.A);
            AssertFlags(x => x.SetZero());
        }

        protected override byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0xA8 | register);
        }
    }
}