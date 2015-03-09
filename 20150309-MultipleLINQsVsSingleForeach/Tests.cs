using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using NUnit.Framework;

namespace MultipleLINQsVsSingleForeach
{
    [TestFixture]
    public class Tests
    {
        private Stopwatch _sw;

        [SetUp]
        public void Init()
        {
            _sw = new Stopwatch();
        }

        private List<ClassA> CreateXRandomClassA(int x)
        {
            var res = new List<ClassA>();
            for (var i = 0; i < x; i++)
            {
                res.Add(new ClassA());
            }
            return res;
        }

        [Test]
        public void AllTests()
        {
            var counts = new[] {5000, 50000, 500000};
            var lists = new List<ClassA>[counts.Length];
            const int samples = 10;

            // Initialise lists
            for (var i = 0; i < counts.Length; i++)
                lists[i] = CreateXRandomClassA(counts[i]);

            Console.WriteLine("Running Tests");

            // Item1 = Method, Item2 = Count, Item3 = Time Taken, Item 4 = Memory
            var results = new List<Tuple<string, int, TimeSpan, long>>();

            foreach (var list in lists)
            {
                results.Add(RunSingleTest(samples, list, x => MultipleLinqs(x)));
                results.Add(RunSingleTest(samples, list, x => SingleForeachUsingList(x)));
                results.Add(RunSingleTest(samples, list, x => SingleForeachUsingArray(x)));
            }

            var groups = results.GroupBy(t => t.Item1).ToList();

            Console.WriteLine("Time Taken");
            foreach (var c in counts)
            {
                Console.Write("\r");
                Console.Write(c);
                Console.WriteLine();
            }

            foreach (var g in groups)
            {
                Console.Write(g.Key);
                foreach (var res in g.OrderBy(t => t.Item2))
                {
                    Console.Write("\t");
                    Console.Write(res.Item3.TotalSeconds);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Memory Usage");


            foreach (var c in counts)
            {
                Console.Write("\r");
                Console.Write(c);
                Console.WriteLine();
            }

            foreach (var g in groups)
            {
                Console.Write(g.Key);
                foreach (var res in g.OrderBy(t => t.Item2))
                {
                    Console.Write("\t{0:F2}mb", ((double) res.Item4/1024)/1024);
                }
                Console.WriteLine();
            }
        }

        private Tuple<string, int, TimeSpan, long> RunSingleTest(int samples, List<ClassA> list,
            Expression<Func<List<ClassA>, TestResult>> methodToRun)
        {
            var methodCall = methodToRun.Body as MethodCallExpression;
            var method = methodCall.Method.Name;
            var totalTime = new TimeSpan();
            long totalMem = 0;

            for (var i = 0; i < samples; i++)
            {
                var res1 = methodToRun.Compile()(list);
                totalTime += res1.TimeTaken;
                totalMem += res1.MemoryUsed;
            }

            return new Tuple<string, int, TimeSpan, long>(method, list.Count, new TimeSpan(totalTime.Ticks/samples),
                totalMem/samples);
        }

        [TestCase(5000)]
        [TestCase(50000)]
        [TestCase(500000)]
        public void MutlipleLinqsTest(int count)
        {
            var list = CreateXRandomClassA(count);

            var res = MultipleLinqs(list);

            PrintResults(res);
        }

        private static void PrintResults(TestResult res)
        {
            Console.WriteLine("--------Test Results for Count: {0} -------------", res.Count);
            Console.WriteLine("Time Taken: {0:F3}s", res.TimeTaken.TotalSeconds);
            Console.WriteLine("Memory Used: {0:F2}mb", ((double) res.MemoryUsed/1024)/1024);
            Console.WriteLine("Garbage Collection Count: {0}", res.GCCount);
        }

        private TestResult MultipleLinqs(List<ClassA> list)
        {
            GC.Collect();
            var mem = GC.GetTotalMemory(true);
            var gcPreCount = GC.CollectionCount(0) + GC.CollectionCount(1) + GC.CollectionCount(2);


            _sw.Restart();

            // Could be HashSet or Dictionary or any other Hashbased collection
            var i1Lookup = list.ToLookup(b => b.Int1);
            var s1Lookup = list.ToLookup(b => b.String1);
            var s2Lookup = list.ToLookup(b => b.String2);
            var s3Lookup = list.ToLookup(b => b.String3);
            var s4Lookup = list.ToLookup(b => b.String4);
            var s5Lookup = list.ToLookup(b => b.String5);
            var s6Lookup = list.ToLookup(b => b.String6);
            var s7Lookup = list.ToLookup(b => b.String7);
            var s8Lookup = list.ToLookup(b => b.String8);
            var s9Lookup = list.ToLookup(b => b.String9);

            _sw.Stop();

            var gcPostCount = GC.CollectionCount(0) + GC.CollectionCount(1) + GC.CollectionCount(2);
            var memUsed = GC.GetTotalMemory(true) - mem;
            GC.Collect();


            return new TestResult
            {
                Count = list.Count,
                TimeTaken = _sw.Elapsed,
                GCCount = gcPostCount - gcPreCount,
                MemoryUsed = memUsed,
                TestHash = i1Lookup.GetHashCode()
                           ^ s1Lookup.GetHashCode()
                           ^ s2Lookup.GetHashCode()
                           ^ s3Lookup.GetHashCode()
                           ^ s4Lookup.GetHashCode()
                           ^ s5Lookup.GetHashCode()
                           ^ s6Lookup.GetHashCode()
                           ^ s7Lookup.GetHashCode()
                           ^ s8Lookup.GetHashCode()
                           ^ s9Lookup.GetHashCode()
            };
        }

        [TestCase(5000)]
        [TestCase(50000)]
        [TestCase(500000)]
        public void SingleForeachUsingListTest(int count)
        {
            var list = CreateXRandomClassA(count);

            var res = SingleForeachUsingList(list);

            PrintResults(res);
        }

        private TestResult SingleForeachUsingList(List<ClassA> list)
        {
            var mem = GC.GetTotalMemory(true);
            var gcPreCount = GC.CollectionCount(0) + GC.CollectionCount(1) + GC.CollectionCount(2);
            _sw.Restart();

            // Could be HashSet or Dictionary or any other Hashbased collection
            var i1Lookup = new Dictionary<int, List<ClassA>>(list.Count);
            var s1Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s2Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s3Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s4Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s5Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s6Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s7Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s8Lookup = new Dictionary<string, List<ClassA>>(list.Count);
            var s9Lookup = new Dictionary<string, List<ClassA>>(list.Count);

            foreach (var item in list)
            {
                GetValueOrAdd(i1Lookup, item, a => a.Int1);
                GetValueOrAdd(s1Lookup, item, a => a.String1);
                GetValueOrAdd(s2Lookup, item, a => a.String2);
                GetValueOrAdd(s3Lookup, item, a => a.String3);
                GetValueOrAdd(s4Lookup, item, a => a.String4);
                GetValueOrAdd(s5Lookup, item, a => a.String5);
                GetValueOrAdd(s6Lookup, item, a => a.String6);
                GetValueOrAdd(s7Lookup, item, a => a.String7);
                GetValueOrAdd(s8Lookup, item, a => a.String8);
                GetValueOrAdd(s9Lookup, item, a => a.String9);
            }

            _sw.Stop();
            var gcPostCount = GC.CollectionCount(0) + GC.CollectionCount(1) + GC.CollectionCount(2);

            var memUsed = GC.GetTotalMemory(true) - mem;

            return new TestResult
            {
                Count = list.Count,
                TimeTaken = _sw.Elapsed,
                GCCount = gcPostCount - gcPreCount,
                MemoryUsed = memUsed,
                TestHash = i1Lookup.GetHashCode()
                           ^ s1Lookup.GetHashCode()
                           ^ s2Lookup.GetHashCode()
                           ^ s3Lookup.GetHashCode()
                           ^ s4Lookup.GetHashCode()
                           ^ s5Lookup.GetHashCode()
                           ^ s6Lookup.GetHashCode()
                           ^ s7Lookup.GetHashCode()
                           ^ s8Lookup.GetHashCode()
                           ^ s9Lookup.GetHashCode()
            };
        }

        [TestCase(5000)]
        [TestCase(50000)]
        [TestCase(500000)]
        public void SingleForeachUsingArrayTest(int count)
        {
            var list = CreateXRandomClassA(count);

            var res = SingleForeachUsingArray(list);

            PrintResults(res);
        }

        private TestResult SingleForeachUsingArray(List<ClassA> list)
        {
            var mem = GC.GetTotalMemory(true);
            var gcPreCount = GC.CollectionCount(0) + GC.CollectionCount(1) + GC.CollectionCount(2);
            _sw.Restart();

            // Could be HashSet or Dictionary or any other Hashbased collection
            var i1Lookup = new Dictionary<int, ClassA[]>(list.Count);
            var s1Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s2Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s3Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s4Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s5Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s6Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s7Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s8Lookup = new Dictionary<string, ClassA[]>(list.Count);
            var s9Lookup = new Dictionary<string, ClassA[]>(list.Count);

            foreach (var item in list)
            {
                GetValueOrAdd(i1Lookup, item, a => a.Int1);
                GetValueOrAdd(s1Lookup, item, a => a.String1);
                GetValueOrAdd(s2Lookup, item, a => a.String2);
                GetValueOrAdd(s3Lookup, item, a => a.String3);
                GetValueOrAdd(s4Lookup, item, a => a.String4);
                GetValueOrAdd(s5Lookup, item, a => a.String5);
                GetValueOrAdd(s6Lookup, item, a => a.String6);
                GetValueOrAdd(s7Lookup, item, a => a.String7);
                GetValueOrAdd(s8Lookup, item, a => a.String8);
                GetValueOrAdd(s9Lookup, item, a => a.String9);
            }

            _sw.Stop();
            var gcPostCount = GC.CollectionCount(0) + GC.CollectionCount(1) + GC.CollectionCount(2);
            var memUsed = GC.GetTotalMemory(true) - mem;

            var res = i1Lookup.ContainsKey(1);

            return new TestResult
            {
                Count = list.Count,
                TimeTaken = _sw.Elapsed,
                GCCount = gcPostCount - gcPreCount,
                MemoryUsed = memUsed,
                TestHash = i1Lookup.GetHashCode()
                           ^ s1Lookup.GetHashCode()
                           ^ s2Lookup.GetHashCode()
                           ^ s3Lookup.GetHashCode()
                           ^ s4Lookup.GetHashCode()
                           ^ s5Lookup.GetHashCode()
                           ^ s6Lookup.GetHashCode()
                           ^ s7Lookup.GetHashCode()
                           ^ s8Lookup.GetHashCode()
                           ^ s9Lookup.GetHashCode()
            };
        }

        private static void GetValueOrAdd<TKey>(Dictionary<TKey, List<ClassA>> dic, ClassA item,
            Func<ClassA, TKey> keySelector)
        {
            List<ClassA> list;

            if (!dic.TryGetValue(keySelector(item), out list))
            {
                list = new List<ClassA>(1);
                dic.Add(keySelector(item), list);
            }

            list.Add(item);
        }

        private static void GetValueOrAdd<TKey>(Dictionary<TKey, ClassA[]> dic, ClassA item,
            Func<ClassA, TKey> keySelector)
        {
            ClassA[] list;
            var key = keySelector(item);

            if (!dic.TryGetValue(key, out list))
            {
                list = new[] {item};
                dic.Add(key, list);
                return;
            }

            var newList = new ClassA[list.Length + 1];
            for (var i = 0; i < list.Length; i++)
            {
                var a = list[i];
                if (a.Equals(item)) return; // Already added

                newList[i] = a;
            }
            newList[list.Length] = item;
            dic[key] = newList;
        }

        public class TestResult
        {
            public TimeSpan TimeTaken { get; set; }
            public long MemoryUsed { get; set; }
            public int GCCount { get; set; }
            public int Count { get; set; }
            public int TestHash { get; set; }
        }
    }
}