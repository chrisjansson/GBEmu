using Core;
using Xunit;

namespace Test.CpuA
{
    public class Flags
    {
        private Cpu _sut;

        public Flags()
        {
            var fakeMmu = new FakeMmu();
            _sut = new Cpu(fakeMmu);
        }

        [Fact]
        public void Zero()
        {
            _sut.F = 0x80;

            Assert.Equal(1, _sut.Z);
        }

        [Fact]
        public void F_zero()
        {
            _sut.Z = 1;

            Assert.Equal(0x80, _sut.F);
        }

        [Fact]
        public void Subtract()
        {
            _sut.F = 0x40;

            Assert.Equal(1, _sut.N);
        }

        [Fact]
        public void F_subtract()
        {
            _sut.N = 1;

            Assert.Equal(1, _sut.N);
        }

        [Fact]
        public void Half_carry()
        {
            _sut.F = 0x20;

            Assert.Equal(1, _sut.HC);
        }

        [Fact]
        public void F__half_carry()
        {
            _sut.HC = 1;

            Assert.Equal(0x20, _sut.F);
        }

        [Fact]
        public void Carry()
        {
            _sut.F = 0x10;

            Assert.Equal(1, _sut.Carry);
        }

        [Fact]
        public void F_carry()
        {
            _sut.Carry = 1;

            Assert.Equal(0x10, _sut.F);
        }

        [Fact]
        public void Set_flags()
        {
            _sut.N = 1;
            _sut.Carry = 1;
            _sut.HC = 1;
            _sut.Z = 1;

            Assert.Equal(0xF0, _sut.F);
        }

        [Fact]
        public void Carry_mask()
        {
            _sut.Carry = 0xFF;

            Assert.Equal(0x10, _sut.F);
        }

        [Fact]
        public void Zero_mask()
        {
            _sut.Z = 0xFF;

            Assert.Equal(0x80, _sut.F);
        }

        [Fact]
        public void Half_carry_mask()
        {
            _sut.HC = 0xFF;

            Assert.Equal(0x20, _sut.F);
        }

        [Fact]
        public void Subtract_mask()
        {
            _sut.N = 0xFF;

            Assert.Equal(0x40, _sut.F);
        }
    }
}