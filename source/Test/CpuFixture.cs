using Core;

namespace Test
{
    public class CpuFixture
    {
        public FakeMmu FakeMmu;
        public Cpu Cpu;

        public CpuFixture()
        {
            FakeMmu = new FakeMmu();
            Cpu = new Cpu(FakeMmu);
        }
    }
}