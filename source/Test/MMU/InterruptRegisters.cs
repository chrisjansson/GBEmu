using Core;
using Ploeh.AutoFixture.Xunit;
using Xunit;
using Xunit.Extensions;

namespace Test.MMU
{
    public class InterruptRegisters
    {
        private Core.MMU _mmu;
        private Cpu _cpu;

        public InterruptRegisters()
        {
            _mmu = new Core.MMU();
            _cpu = new Cpu(_mmu);
            _mmu.Cpu = _cpu;
        }

        [Theory,AutoData]
        public void IE_is_read_through_0xFFFF_from_CPU(byte expectedIE)
        {
            _cpu.IE = expectedIE;

            var actual = _mmu.GetByte(RegisterAddresses.IE);

            Assert.Equal(expectedIE, actual);
        }

        [Theory,AutoData]
        public void IE_is_written_through_CPU_to_0xFFFF(byte expectedIE)
        {
            _mmu.SetByte(RegisterAddresses.IE, expectedIE);

            Assert.Equal(expectedIE, _cpu.IE);
        }
    }
}