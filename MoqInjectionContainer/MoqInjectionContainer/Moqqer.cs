using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using Moq;
using Moq.Language.Flow;

namespace MoqInjectionContainer
{
    public class Moqqer
    {
        internal readonly Dictionary<Type, Mock> Mocks = new Dictionary<Type, Mock>();
        internal static MethodInfo ObjectGenericMethod;


        static Moqqer ()
        {
            var moqType = typeof (Moqqer);

            ObjectGenericMethod = moqType.GetMethod("Object");


        }

        public T Get<T>() where T : class
        {
            var type = typeof (T);

            var ctor = FindConstructor(type);

            var parameters = CreateParameters(ctor);

            var res = ctor.Invoke(parameters);

            return res as T;
        }

        public Mock<T> Of<T>() where T : class
        {
            var type = typeof (T);

            if (Mocks.ContainsKey(type))
                return Mocks[type] as Mock<T>;

            var mock = new Mock<T>();

            Mocks.Add(type, mock);

            SetupMockMethods(mock, type);

            return mock;
        }


        public T Object<T>() where T : class
        {
            return Of<T>().Object;
        }

        public void VerifyAll()
        {
            foreach (var mock in Mocks.Values)
            {
                mock.VerifyAll();
            }
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

        internal static Mock MockOfType(Type type)
        {
            var mock = typeof (Mock<>);

            var genericMock = mock.MakeGenericType(type);

            return Activator.CreateInstance(genericMock) as Mock;
        }

        internal Mock Of(Type type)
        {
            if (Mocks.ContainsKey(type))
                return Mocks[type];

            var mock = MockOfType(type);

            Mocks.Add(type, mock);

            SetupMockMethods(mock, type);

            return mock;
        }

        internal void SetupMockMethods(Mock mock, Type type)
        {
            var methods = GetMockableMethods(type).ToList();

            if (!methods.Any()) return;

            var mockType = typeof (Mock<>).MakeGenericType(type);
            
            var mockSetupFuncMethod = mockType.GetMethods().Single(x => x.Name == "Setup" && x.ContainsGenericParameters);

            var itType = typeof(It);
            var isAnyMethod = itType.GetMethod("IsAny");

            var inputParameter = Expression.Parameter(type, "x"); // Moqqer Lambda Param

            foreach (var method in methods)
            {
                var parameters =method.GetParameters();
                var args = parameters.Select(x => Expression.Call(isAnyMethod.MakeGenericMethod(x.ParameterType))).ToArray();
                
                var reflectedExpression = Expression.Call(inputParameter, method, args);

                var setupFuncType = typeof(Func<,>).MakeGenericType(type, method.ReturnType);

                var lambda = Expression.Lambda(setupFuncType, reflectedExpression, inputParameter);

                var genericMockSetupFuncMethod = mockSetupFuncMethod.MakeGenericMethod(method.ReturnType);
                var setup = genericMockSetupFuncMethod.Invoke(mock, new object[] {lambda});

                var funcType = typeof (Func<>).MakeGenericType(method.ReturnType);

                var delgate = Delegate.CreateDelegate(funcType, this, ObjectGenericMethod.MakeGenericMethod(method.ReturnType));

                var setupType = setup.GetType();

                var returnsMethod = setupType.GetMethod("Returns", new[] {funcType});

                returnsMethod.Invoke(setup, new object[] {delgate});
            }
        }

        internal static IEnumerable<MethodInfo> GetMockableMethods(Type type)
        {
            return type
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.ReturnType.IsInterface);
        }

        private object[] CreateParameters(ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters();

            var res = new object[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var type = parameters[i].ParameterType;

                res[i] = Of(type).Object;
            }

            return res;
        }
    }
}