using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace Michal.Ciechan.Library.Tests
{
    [TestFixture]
    public class EnumerableDataReaderEfFixture
    {
        [Test]
        public void TestEnumerableDataReaderWithIQueryableOfAnonymousType()
        {
            var list = new List<TestClass>
            {
                new TestClass() {Prop1 = "MCTEST1", Prop2 = "Random1"},
                new TestClass() {Prop1 = "Mikey2", Prop2 = "Meeeee2"},
            };
            var r = new EnumerableDataReader(list);

            for (int i = 0; i < r.FieldCount; i++)
            {
                Console.Write(r.GetName(i) + "|");
            }
            Console.WriteLine();

            while (r.Read())
            {

                var values = new object[2];
                r.GetValues(values);
                Console.WriteLine("{0} {1}", values);
            }
        }


    }
}