using Xunit;

namespace Test.Display
{
    public class Scanline_tests
    {
        private readonly Core.Display _sut;

        public Scanline_tests()
        {
            _sut = new Core.Display();
        }

        [Fact]
        public void Starts_at_line_0()
        {
            Assert.Equal(0, _sut.Line);
        }

        [Fact]
        public void Increments_line_after_completing_a_scan_line()
        {
            _sut.AdvanceScanLine();

            Assert.Equal(1, _sut.Line);
        }

        [Fact]
        public void Increments_line_to_153()
        {
            for (var i = 0; i < 153; i++)
            {
                _sut.AdvanceScanLine();    
            }
            
            Assert.Equal(153, _sut.Line);
        }

        [Fact]
        public void Resets_line_count_to_zero_after_153()
        {
            for (var i = 0; i < 154; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(0, _sut.Line);
        }
    }
}