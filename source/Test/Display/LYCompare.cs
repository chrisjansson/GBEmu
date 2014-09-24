using Xunit;

namespace Test.Display
{
    public class LYCompare : CpuTestBase
    {
        private readonly Core.Display _sut;
        private FakeMmu _fakeMmu;

        public LYCompare()
        {
            _fakeMmu = new FakeMmu();
            _sut = new Core.Display(_fakeMmu, new FakeDisplayDataTransferService());
        }

        [Fact]
        public void Coincidence_flag_is_zero_when_LYC_and_LY_are_different()
        {
            _sut.LYC = 100;
            for (var i = 0; i < 99; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.False(CoincidenceFlag);
        }

        [Fact]
        public void Coincidence_flag_is_one_when_LYC_and_LY_are_same()
        {
            _sut.LYC = 123;
            for (int i = 0; i < 123; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.True(CoincidenceFlag);
        }

        private bool CoincidenceFlag
        {
            get { return ((_sut.LCDC >> 2) & 0x1) == 0x1; }
        }
    }
}