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
        private const int IE = 0xFFFF;
        private const int IF = 0xFF0F;

        public InterruptRegisters()
        {
            _mmu = new Core.MMU(null);
            _cpu = new Cpu(_mmu);
            _mmu.Cpu = _cpu;
        }

        [Theory, AutoData]
        public void IE_is_read_through_0xFFFF_from_CPU(byte expectedIE)
        {
            _cpu.IE = expectedIE;

            var actual = _mmu.GetByte(IE);

            Assert.Equal(expectedIE, actual);
        }

        [Theory, AutoData]
        public void IE_is_written_through_CPU_to_0xFFFF(byte expectedIE)
        {
            _mmu.SetByte(IE, expectedIE);

            Assert.Equal(expectedIE, _cpu.IE);
        }

        [Theory, AutoData]
        public void IF_is_read_through_0xFF0F_from_CPU(byte expectedIF)
        {
            _cpu.IF = expectedIF;
            
            var actual = _mmu.GetByte(IF);

            Assert.Equal(expectedIF, actual);
        }

        [Theory, AutoData]
        public void IF_is_written_through_CPU_to_0xFF0F(byte expectedIF)
        {
            _mmu.SetByte(IF, expectedIF);

            Assert.Equal(expectedIF, _cpu.IF);
        }
    }
}