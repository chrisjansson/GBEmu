using System;
using System.Collections.Generic;
using Core;

namespace Test
{
    public class JumpCondition
    {
        private readonly byte _cc;
        private readonly Action<Cpu> _set;
        private readonly Action<Cpu> _reset;

        private JumpCondition(byte cc, Action<Cpu> set, Action<Cpu> reset)
        {
            _cc = cc;
            _set = set;
            _reset = reset;
        }

        public static readonly JumpCondition NZ = new JumpCondition(0x0, x => x.Z = 0, x => x.Z = 1);
        public static readonly JumpCondition Z = new JumpCondition(0x1, x => x.Z = 1, x => x.Z = 0);
        public static readonly JumpCondition NC = new JumpCondition(0x2, x => x.Carry = 0, x => x.Carry = 1);
        public static readonly JumpCondition C = new JumpCondition(0x3, x => x.Carry = 1, x => x.Carry = 0);

        public void Set(Cpu cpu)
        {
            _set(cpu);
        }

        public void Reset(Cpu cpu)
        {
            _reset(cpu);
        }

        public static implicit operator byte(JumpCondition condition)
        {
            return condition._cc;
        }

        public static IEnumerable<JumpCondition> GetAll()
        {
            return new List<JumpCondition>
            {
                NZ,
                Z,
                NC,
                C
            };
        }
    }
}