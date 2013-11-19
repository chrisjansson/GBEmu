using System;
using System.Collections.Generic;
using Core;
using Ploeh.AutoFixture;
using Xunit;

namespace Test
{
    public class CpuAssertion
    {
        private readonly Cpu _cpu;
        private readonly FakeMmu _fakeMmu;
        private long _initialCycles;
        private ushort _initialProgramCounter;
        private List<Action> _assertions;

        public CpuAssertion()
        {
            var fixture = new Fixture();
            _fakeMmu = fixture.Create<FakeMmu>();
            fixture.Inject<IMmu>(_fakeMmu);
            _cpu = fixture.Create<Cpu>();
            _initialCycles = _cpu.Cycles;
            _initialProgramCounter = _cpu.ProgramCounter;

            _assertions = new List<Action>();
        }

        public CpuAssertion Executing(byte opCode, params byte[] args)
        {
            return this;
        }

        public CpuAssertion ExecutingCB(byte opCode)
        {
            _fakeMmu.Memory[_cpu.ProgramCounter + 1] = opCode;
            _cpu.Execute(0xCB);

            return this;
        }

        public void End()
        {
            foreach (var assertion in _assertions)
            {
                assertion();
            }
        }

        public CpuAssertion ShouldAdvanceProgramCounter(int i)
        {
            _assertions.Add(() => Assert.Equal(_initialProgramCounter + i, _cpu.ProgramCounter));
            return this;
        }

        public CpuAssertion ShouldAdvanceClock(int i)
        {
            _assertions.Add(() => Assert.Equal(_initialCycles + i, _cpu.Cycles));
            return this;
        }

        public CpuAssertion ShouldSetN()
        {
            return this;
        }

        public CpuAssertion ShouldResetN()
        {
            return this;
        }

        public CpuAssertion Memory(ushort address, params byte[] memory)
        {
            return this;
        }

        public CpuAssertion AShouldContain(byte expected)
        {
            return this;
        }
    }
}