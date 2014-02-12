using Xunit;    

namespace Test.Display
{
    public class Mode_tests
    {
        private readonly Core.Display _sut;

        public Mode_tests()
        {
            _sut = new Core.Display(new FakeDisplayDataTransferService());
        }

        [Fact]
        public void Starts_in_mode_2()
        {
            Assert.Equal(2, _sut.Mode);
        }


        [Fact]
        public void Is_in_mode_2_for_20_cycles()
        {
            for (var i = 0; i < 19; i++)
            {
                _sut.Tick();

                Assert.Equal(2, _sut.Mode);
            }
        }

        [Fact]
        public void Is_in_mode_3_after_20_cycles()
        {
            Tick(20);

            Assert.Equal(3, _sut.Mode);
        }

        [Fact]
        public void Is_in_mode_3_for_43_cycles()
        {
            Tick(20);

            for (var i = 0; i < 42; i++)
            {
                _sut.Tick();

                Assert.Equal(3, _sut.Mode);
            }
        }

        [Fact]
        public void Is_in_mode_0_after_63_cycles()
        {
            Tick(63);

            Assert.Equal(0, _sut.Mode);
        }

        [Fact]
        public void Is_in_mode_0_for_51_cycles()
        {
            Tick(63);

            for (var i = 0; i < 50; i++)
            {
                _sut.Tick();
           
                Assert.Equal(0, _sut.Mode);
            }
        }

        [Fact]
        public void Returns_to_mode_2_after_114_cycles()
        {
            Tick(114);

            Assert.Equal(2, _sut.Mode);
        }

        [Fact]
        public void Returs_to_mode_3_after_134_cycles()
        {
            Tick(134);

            Assert.Equal(3, _sut.Mode);
        }

        [Fact]
        public void Is_in_mode_2_after_143_scan_lines()
        {
            for (var i = 0; i < 143; i++)
            {
                _sut.AdvanceScanLine();    
            }
            
            Assert.Equal(2, _sut.Mode);
        }

        [Fact]
        public void Is_in_mode_1_after_144_scan_lines()
        {
            for (var i = 0; i < 144; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(1, _sut.Mode);
        }

        [Fact]
        public void Is_in_mode_1_for_1140_cycles()
        {
            for (var i = 0; i < 144; i++)
            {
                _sut.AdvanceScanLine();
            }

            for (var i = 0; i < 1139; i++)
            {
                Assert.Equal(1, _sut.Mode);
            }
        }

        [Fact]
        public void Is_in_mode_2_after_154_scan_lines()
        {
            for (int i = 0; i < 154; i++)
            {
                _sut.AdvanceScanLine();
            }

            Assert.Equal(2, _sut.Mode);
        }

        private void Tick(int clocks)
        {
            for (var i = 0; i < clocks; i++)
            {
                _sut.Tick();
            }
        }
    }
}