using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;

namespace SqlUnionVsJoinBenchmark
{
    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(Job.Dry.WithTargetCount(25 - 1 - 1 - 1));     // Launch, Warmup, ?

            Add(new CpuTimeColumn());
            Add(new CpuTimeDiagnoser());
            Add(new WordPressMarkdownExporter());
        }
    }
}