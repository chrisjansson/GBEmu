using Core;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    public class TestBase
    {
        protected Cpu Sut;
        protected Fixture Fixture;
        protected FakeMmu FakeMmu;

        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
            FakeMmu = new FakeMmu();
            Sut = new Cpu(FakeMmu);
        }
    }
}