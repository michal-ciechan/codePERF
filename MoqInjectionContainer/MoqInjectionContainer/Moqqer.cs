using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace MoqInjectionContainer
{
    public class Moqqer
    {
        internal readonly Dictionary<Type, Mock> Mocks = new Dictionary<Type, Mock>();
        
        

        public T Get<T>() where T : class
        {
            var type = typeof (T);

            var ctor = FindConstructor(type);

            var parameters = CreateParameters(ctor);

            var res = ctor.Invoke(parameters);

            return res as T;
        }

        private object[] CreateParameters(ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters();

            var res = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].ParameterType;

                res[i] = Of(type).Object;
            }

            return res;
        }

        internal ConstructorInfo FindConstructor(Type type)
        {
            var ctors = type.GetConstructors();

            var potentialCtors = ctors.Where(c => c.GetParameters().All(p => !p.ParameterType.IsValueType)).ToList();

            if (potentialCtors.Count == 0)
                throw new MoqqerException("Could not find any possible constructors for type: " + type.Name);

            var maxParams = potentialCtors.Max(c => c.GetParameters().Length);

            return potentialCtors.First(c => c.GetParameters().Length == maxParams);
        }

        public Mock<T> Of<T>() where T : class
        {
            var type = typeof (T);

            if (Mocks.ContainsKey(type))
                return Mocks[type] as Mock<T>;

            var mock = new Mock<T>();

            Mocks.Add(type, mock);

            return mock;
        }

        public void VerifyAll()
        {
            foreach (var mock in Mocks.Values)
            {
                mock.VerifyAll();
            }
        }

        internal Mock Of(Type type)
        {
            if (Mocks.ContainsKey(type))
                return Mocks[type];

            var mock = MockOfType(type);

            Mocks.Add(type, mock);

            return mock;
        }

        internal static Mock MockOfType(Type type)
        {
            var mock = typeof (Mock<>);

            var genericMock = mock.MakeGenericType(type);

            return Activator.CreateInstance(genericMock) as Mock; 
        }
    }
}
