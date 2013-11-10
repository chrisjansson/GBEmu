using Core;
using Xunit;

namespace Test
{
    public class Call : IUseFixture<CpuFixture>
    {
        private Cpu _cpu;
        private FakeMmu _fakeMmu;

        [Fact]
        public void Call_sets_program_counter()
        {
            _cpu.ProgramCounter = 8271;
            _cpu.Cycles = 9182;
            _fakeMmu.SetByte(8272, 0x16);
            _fakeMmu.SetByte(8273, 0xFA);

            _cpu.Execute(0xCD);

            Assert.Equal(0xFA16, _cpu.ProgramCounter);
            Assert.Equal(9188, _cpu.Cycles);
        }

        [Fact]
        public void Call_stores_return_address_at_stack_pointer()
        {
            _cpu.ProgramCounter = 0x8243;
            _cpu.SP = 0x500;

            _cpu.Execute(0xCD);

            Assert.Equal(0x82, _fakeMmu.GetByte(0x4FF));
            Assert.Equal(0x46, _fakeMmu.GetByte(0x4FE));
        }

        [Fact]
        public void Call_decrements_stack_pointer()
        {
            _cpu.SP = 0x833;

            _cpu.Execute(0xCD);

            Assert.Equal(0x831, _cpu.SP);
        }

        public void SetFixture(CpuFixture data)
        {
            _cpu = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}