using System;
using System.Collections.Generic;
using System.Reflection;
using Core;

namespace Test
{
    public class RegisterMapping
    {
        public static readonly RegisterMapping A = new RegisterMapping(7, x => x.A, (c, x) => c.A = x);
        public static readonly RegisterMapping B = new RegisterMapping(0, x => x.B, (c, x) => c.B = x);
        public static readonly RegisterMapping C = new RegisterMapping(1, x => x.C, (c, x) => c.C = x);
        public static readonly RegisterMapping D = new RegisterMapping(2, x => x.D, (c, x) => c.D = x);
        public static readonly RegisterMapping E = new RegisterMapping(3, x => x.E, (c, x) => c.E = x);
        public static readonly RegisterMapping H = new RegisterMapping(4, x => x.H, (c, x) => c.H = x);
        public static readonly RegisterMapping L = new RegisterMapping(5, x => x.L, (c, x) => c.L = x);

        private readonly byte _bitValue;
        private readonly Func<Cpu, byte> _getter;
        private readonly Action<Cpu, byte> _setter;

        private RegisterMapping(byte bitValue, Func<Cpu, byte> getter, Action<Cpu, byte> setter)
        {
            _setter = setter;
            _getter = getter;
            _bitValue = bitValue;
        }

        public byte Get(Cpu cpu)
        {
            return _getter(cpu);
        }

        public void Set(Cpu cpu, byte value)
        {
            _setter(cpu, value);
        }

        public static implicit operator byte(RegisterMapping registerMapping)
        {
            return registerMapping._bitValue;
        }

        public static IEnumerable<RegisterMapping> GetAll()
        {
            return new[]
            {
                A, B, C, D, E, H, L
            };
        }

        public override string ToString()
        {
            var fieldInfo = GetInstanceFieldInfo();
            return fieldInfo.Name;
        }

        private FieldInfo GetInstanceFieldInfo()
        {
            Type type = GetType();
            var fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (var fieldInfo in fields)
            {
                var value = fieldInfo.GetValue(this);
                if (value == this)
                {
                    return fieldInfo;
                }
            }

            throw new MissingFieldException();
        }
    }
}