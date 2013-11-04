using Core;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class LD_A_HLI : IUseFixture<CpuFixture>
    {
        private Cpu _sut;
        private FakeMmu _fakeMmu;

        [Theory]
        [InlineData((byte)0x01, (byte)0xFF, (byte)0x02, (byte)0x00)]
        [InlineData((byte)0x00, (byte)0xCA, (byte)0x00, (byte)0xCB)]
        public void FactMethodName(byte high, byte low, byte expectedHigh, byte expectedLow)
        {
            _sut.ProgramCounter = 9284;
            _sut.Cycles = 103284;
            _sut.H = high;
            _sut.L = low;
            _fakeMmu.SetByte((ushort)(high << 8 | low), 194);

            _sut.Execute(0x2A);

            Assert.Equal(194, _sut.A);
            Assert.Equal(expectedHigh, _sut.H);
            Assert.Equal(expectedLow, _sut.L);
            Assert.Equal(9285, _sut.ProgramCounter);
            Assert.Equal(103286, _sut.Cycles);
        }

        public void SetFixture(CpuFixture data)
        {
            _sut = data.Cpu;
            _fakeMmu = data.FakeMmu;
        }
    }
}