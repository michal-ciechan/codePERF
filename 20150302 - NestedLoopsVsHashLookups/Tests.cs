using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NestedLoopsVsHashLookups
{

    public class ClassA
    {
        public int Id { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
    }

    public class ClassB
    {
        public int Id { get; set; }
        public string String1 { get; set; }
        public string String2 { get; set; }
    }

    [TestFixture]
    public class Tests
    {
        private List<ClassA> _listA;
        private List<ClassB> _listB;
        private Stopwatch _sw;

        public List<ClassA> CreateXRandomClassA(int x)
        {
            var r = new Random();
            var res = new List<ClassA>();
            for (int i = 0; i < x; i++)
            {
                res.Add(new ClassA {Id = r.Next(), String1 = r.NextDouble().ToString(), String2 = r.NextDouble().ToString()});
            }
            return res;
        }
        public List<ClassB> CreateXRandomClassB(int x)
        {
            var r = new Random();
            var res = new List<ClassB>();
            for (int i = 0; i < x; i++)
            {
                res.Add(new ClassB {Id = r.Next(), String1 = r.NextDouble().ToString(), String2 = r.NextDouble().ToString()});
            }
            return res;
        }

        [SetUp]
        public void Init()
        {
            _sw = new Stopwatch();
        }

        [TestCase(1000,1000)]
        [TestCase(1000,10000)]
        [TestCase(1000,100000)]
        [TestCase(10000, 1000)]
        [TestCase(10000, 10000)]
        [TestCase(10000, 100000)]
        [TestCase(100000, 1000)]
        [TestCase(100000, 10000)]
        [TestCase(100000, 100000)]
        public void NestedLoopStringTest(int aCount, int bCount)
        {
            _listA = CreateXRandomClassA(aCount);
            _listB = CreateXRandomClassB(bCount);
            _sw.Start();


            foreach (var a in _listA)
            {
                foreach (var b in _listB)
                {
                    if (a.String1 == b.String1) continue;
                    // Do some work
                }
            }


            _sw.Stop();
            Console.WriteLine("Time taken for ClassA[{0}] ClassB[{1}]: {2}", aCount, bCount, _sw.Elapsed);
        }

        [TestCase(1000,1000)]
        [TestCase(1000,10000)]
        [TestCase(1000,100000)]
        [TestCase(10000, 1000)]
        [TestCase(10000, 10000)]
        [TestCase(10000, 100000)]
        [TestCase(100000, 1000)]
        [TestCase(100000, 10000)]
        [TestCase(100000, 100000)]
        public void NestedLoopIntTest(int aCount, int bCount)
        {
            _listA = CreateXRandomClassA(aCount);
            _listB = CreateXRandomClassB(bCount);
            _sw.Start();


            foreach (var a in _listA)
            {
                foreach (var b in _listB)
                {
                    if (a.Id == b.Id) continue;
                    // Do some work
                }
            }


            _sw.Stop();
            Console.WriteLine("Time taken for ClassA[{0}] ClassB[{1}]: {2}", aCount, bCount, _sw.Elapsed);
        }

        [TestCase(1000,1000)]
        [TestCase(1000,10000)]
        [TestCase(1000,100000)]
        [TestCase(10000, 1000)]
        [TestCase(10000, 10000)]
        [TestCase(10000, 100000)]
        [TestCase(100000, 1000)]
        [TestCase(100000, 10000)]
        [TestCase(100000, 100000)]
        public void HashLookupStringTest(int aCount, int bCount)
        {
            _listA = CreateXRandomClassA(aCount);
            _listB = CreateXRandomClassB(bCount);
            _sw.Start();

            // Could be HashSet or Dictionary or any other Hashbased collection
            var bLookup = _listB.ToLookup(b => b.String1); 

            foreach (var a in _listA)
            {
                if (bLookup.Contains(a.String1)) continue;
                // Do some work
            }


            _sw.Stop();
            Console.WriteLine("Time taken for ClassA[{0}] ClassB[{1}]: {2}", aCount, bCount, _sw.Elapsed);
        }


    }

}
