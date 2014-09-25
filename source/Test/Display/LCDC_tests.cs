using Xunit;

namespace Test.Display
{
    public class LCDC_tests
    {
        private readonly Core.Display _sut;

        public LCDC_tests()
        {
            var mmu = new FakeMmu();
            _sut = new Core.Display(mmu, new FakeDisplayDataTransferService());
        }

        [Fact]
        public void First_3_bits_of_lcdc_are_read_only()
        {
            _sut.Tick(20);
            _sut.Tick(43);
            _sut.LYC = 1; //Set first three LCDC status flags to zero

            _sut.LCDC = 0xFF;

            Assert.Equal(0xF8, _sut.LCDC);
        }
    }
}