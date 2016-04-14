using BenchmarkDotNet.Running;

namespace SqlUnionVsJoinBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<PkClusteredIndexIntBenchmark>();
        }
    }
}

