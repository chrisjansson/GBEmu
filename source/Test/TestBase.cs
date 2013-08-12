using Core;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Test
{
    public class TestBase
    {
        protected Cpu Sut;
        protected Fixture Fixture;

        [SetUp]
        public void SetUp()
        {
            Fixture = new Fixture();
            Sut = new Cpu();
        }
    }
}