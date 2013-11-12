using System;
using System.Collections.Generic;
using System.Reflection;
using Core;

namespace Test
{
    public class RegisterPair
    {
        public static readonly RegisterPair BC = new RegisterPair(0x00, cpu => 0, (cpu, b, arg3) => { });
        public static readonly RegisterPair DE = new RegisterPair(0x01, cpu => 0, (cpu, b, arg3) => { });

        public static readonly RegisterPair HL = new RegisterPair(0x02, cpu => (ushort)(cpu.H << 8 | cpu.L),
            (cpu, h, l) =>
            {
                cpu.H = h;
                cpu.L = l;
            });
        public static readonly RegisterPair AF = new RegisterPair(0x03, cpu => (ushort)(cpu.A << 8 | cpu.L),
            (cpu, h, l) =>
            {
                cpu.A = h;
                cpu.F = l;
            });

        public static implicit operator byte(RegisterPair registerPair)
        {
            return (byte)registerPair._value;
        }

        public ushort Get(Cpu cpu)
        {
            return _getter(cpu);
        }

        public void Set(Cpu cpu, byte high, byte low)
        {
            _setter(cpu, high, low);
        }

        private readonly int _value;
        private readonly Func<Cpu, ushort> _getter;
        private readonly Action<Cpu, byte, byte> _setter;

        private RegisterPair(int value, Func<Cpu, ushort> getter, Action<Cpu, byte, byte> setter)
        {
            _setter = setter;
            _getter = getter;
            _value = value;
        }

        public static IEnumerable<RegisterPair> GetAll()
        {
            return new List<RegisterPair>
            {
                BC,
                DE,
                HL,
                AF
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