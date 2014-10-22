using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit.Extensions;

namespace Test.CpuTests
{
    public class InstancePropertyDataAttribute : PropertyDataAttribute
    {
        public InstancePropertyDataAttribute(string propertyName) 
            : base(propertyName) { }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest, Type[] parameterTypes)
        {
            var declaringType = methodUnderTest.ReflectedType;
            var constructor = declaringType.GetConstructors()
                .Single(x => x.GetParameters().Length == 0);

            var instance = constructor.Invoke(null);
            var property = declaringType.GetProperty(PropertyName);
            return (IEnumerable<object[]>) property.GetValue(instance);
        }
    }
}