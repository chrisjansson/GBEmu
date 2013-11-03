using Core;
using Xunit;

namespace Test
{
    public class Nop : IUseFixture<CpuFixture>
    {
        private Cpu _sut;

        [Fact]
        public void Nop_advances_program_counter_and_clock()
        {
            _sut.ProgramCounter = 475;
            _sut.Cycles = 8569;

            _sut.Execute(0x00);

            Assert.Equal(476, _sut.ProgramCounter);
            Assert.Equal(8570, _sut.Cycles);
        }

        public void SetFixture(CpuFixture fixture)
        {
            _sut = fixture.Cpu;
        }
    }
}
