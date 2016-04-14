using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SqlUnionVsJoinBenchmark
{
    public class CpuTimeColumn : IColumn
    {
        public static Dictionary<Benchmark, List<int>> Results = new Dictionary<Benchmark, List<int>>();

        public string GetValue(Summary summary, Benchmark benchmark)
        {
            if (!Results.ContainsKey(benchmark))
                return "Never Run";

            var list = Results[benchmark];


            if (list.Count == 0)
                return "Empty";

            var min = list.Min();
            var max = list.Max();
            var sum = list.Sum();
            var count = list.Count;

            var avg = (sum - min - max) / (count - 2);

            return avg.ToString("N0");
        }

        public bool IsAvailable(Summary summary)
        {
            return true;
        }

        public string ColumnName => "CPUTime(ms)";
        public bool AlwaysShow => true;
    }
}