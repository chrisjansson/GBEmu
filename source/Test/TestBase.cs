using Core;
using Ploeh.AutoFixture;

namespace Test
{
    public class TestBase
    {
        protected Cpu Sut;
        protected Fixture Fixture;
        protected FakeMmu FakeMmu;

        public TestBase()
        {
            Fixture = new Fixture();
            FakeMmu = new FakeMmu();
            Sut = new Cpu(FakeMmu);
        }
    }
}