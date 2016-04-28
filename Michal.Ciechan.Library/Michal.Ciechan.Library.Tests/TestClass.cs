using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;

namespace Michal.Ciechan.Library.Tests
{
    public class TestClass
    {
        public string Prop1 { get; set; }
        public string Prop2 { get; set; }
    }

    [TestFixture]
    public class SomeTests
    {
        sealed class ReferencedPropertyFinder : ExpressionVisitor
        {
            private readonly Type _ownerType;
            private readonly List<PropertyInfo> _properties = new List<PropertyInfo>();

            public ReferencedPropertyFinder(Type ownerType)
            {
                _ownerType = ownerType;
            }

            public IList<PropertyInfo> Properties
            {
                get { return _properties; }
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                var propertyInfo = node.Member as PropertyInfo;
                if (propertyInfo != null && _ownerType.IsAssignableFrom(propertyInfo.DeclaringType))
                {
                    // probably more filtering required
                    _properties.Add(propertyInfo);
                }
                return base.VisitMember(node);
            }
        }

        private static IList<PropertyInfo> GetReferencedProperties<T>(Expression expression)
        {
            var v = new ReferencedPropertyFinder(typeof(T));
            v.Visit(expression);
            return v.Properties;
        }

        sealed class TestEntity
        {
            public int PropertyA { get; set; }

            public int PropertyB { get; set; }

            public int PropertyC { get; set; }
        }

        public LambdaExpression GetExpressionsFromPropertyInfo(Type classType, PropertyInfo prop)
        {
            var parameter = Expression.Parameter(classType, "entity");
            var property = Expression.Property(parameter, prop);
            var funcType = typeof(Func<,>).MakeGenericType(classType, prop.PropertyType);
            return Expression.Lambda(funcType, property, parameter);
        }

        [Test]
        public void Main()
        {

            var props = typeof (TestEntity).GetProperties();



            foreach (var property in props)
            {
                Console.WriteLine("Property name = " + property.Name);

                var lambda = GetExpressionsFromPropertyInfo(typeof (TestEntity), property);

                var refProps = GetReferencedProperties<TestEntity>(lambda);

                foreach (var p in refProps)
                {
                    Console.WriteLine("Ref prop = " + p.Name);
                }


            }
        }
    }
}