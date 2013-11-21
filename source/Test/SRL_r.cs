using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Ploeh.AutoFixture;
using Xunit;
using Xunit.Extensions;

namespace Test
{
    public class SRL_r
    {
        private readonly FakeMmu _fakeMmu;
        private readonly Cpu _sut;
        private ushort _initialProgramCounter;
        private long _initialCycles;

        public SRL_r()
        {
            var fixture = new Fixture();
            _fakeMmu = new FakeMmu();
            fixture.Inject<IMmu>(_fakeMmu);

            _sut = fixture.Create<Cpu>();

            _initialProgramCounter = _sut.ProgramCounter;
            _initialCycles = _sut.Cycles;
        }

        [Theory, PropertyData("Registers")]
        public void Advances_counters(RegisterMapping register)
        {
            ExecutingCB(CreateOpCode(register));

            AdvancedProgramCounter(2);
            AdvancedClock(2);
        }

        [Theory, PropertyData("Registers")]
        public void Shifts_contents_right_sets_carry_and_resets_zero(RegisterMapping register)
        {
            Flags(x => x.Zero().ResetCarry());
            register.Set(_sut, 0x8F);

            ExecutingCB(CreateOpCode(register));

            Assert.Equal(0x47, register.Get(_sut));
            AssertFlags(x => x.SetCarry().ResetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Resets_carry_and_sets_zero(RegisterMapping register)
        {
            Flags(x => x.Carry().ResetZero());
            register.Set(_sut, 0x00);

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetCarry().SetZero());
        }

        [Theory, PropertyData("Registers")]
        public void Subtract_and_half_carry_flags_are_reset(RegisterMapping register)
        {
            Flags(_ => _.Subtract().HalfCarry());

            ExecutingCB(CreateOpCode(register));

            AssertFlags(x => x.ResetSubtract().ResetHalfCarry());
        }

        private void AssertFlags(Action<FlagAssertion> assertion)
        {
            var flagAssertion = new FlagAssertion(_sut);
            assertion(flagAssertion);
        }

        private void Flags(Action<FlagSetter> setter)
        {
            var flagSetter = new FlagSetter(_sut);
            setter(flagSetter);
        }

        private class FlagSetter
        {
            private readonly Cpu _cpu;

            public FlagSetter(Cpu cpu)
            {
                _cpu = cpu;
            }

            public FlagSetter Subtract()
            {
                _cpu.N = 1;
                return this;
            }

            public FlagSetter HalfCarry()
            {
                _cpu.HC = 1;
                return this;
            }

            public FlagSetter Zero()
            {
                _cpu.Z = 1;
                return this;
            }

            public FlagSetter ResetCarry()
            {
                _cpu.Carry = 0;
                return this;
            }

            public FlagSetter Carry()
            {
                _cpu.Carry = 1;
                return this;
            }

            public FlagSetter ResetZero()
            {
                _cpu.Z = 0;
                return this;
            }
        }

        private class FlagAssertion
        {
            private readonly Cpu _cpu;

            public FlagAssertion(Cpu cpu)
            {
                _cpu = cpu;
            }

            public FlagAssertion ResetSubtract()
            {
                Assert.Equal(0, _cpu.N);
                return this;
            }

            public FlagAssertion ResetHalfCarry()
            {
                Assert.Equal(0, _cpu.HC);
                return this;
            }

            public FlagAssertion SetCarry()
            {
                Assert.Equal(1, _cpu.Carry);
                return this;
            }

            public FlagAssertion ResetCarry()
            {
                Assert.Equal(0, _cpu.Carry);
                return this;
            }

            public FlagAssertion ResetZero()
            {
                Assert.Equal(0, _cpu.Z);
                return this;
            }

            public FlagAssertion SetZero()
            {
                Assert.Equal(1, _cpu.Z);
                return this;
            }
        }

        private void AdvancedClock(int expectedIncrement)
        {
            Assert.Equal(_initialCycles + expectedIncrement, _sut.Cycles);
        }

        private void AdvancedProgramCounter(int expectedIncrement)
        {
            Assert.Equal(_initialProgramCounter + expectedIncrement, _sut.ProgramCounter);
        }

        private void ExecutingCB(byte opCode)
        {
            _fakeMmu.Memory[_sut.ProgramCounter + 1] = opCode;
            _sut.Execute(0xCB);
        }

        private byte CreateOpCode(RegisterMapping register)
        {
            return (byte)(0x38 | register);
        }

        public static IEnumerable<object[]> Registers
        {
            get
            {
                return RegisterMapping
                    .GetAll()
                    .Select(x => new object[] { x })
                    .ToList();
            }
        }
    }
}