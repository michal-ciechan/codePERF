using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SqlUnionVsJoinBenchmark
{
    public class CpuTimeDiagnoser : IDiagnoser
    {
        private string _output;
        private int _count;
        private readonly StringBuilder _sb = new StringBuilder();

        public void Start(Benchmark benchmark)
        {
            CpuTimeColumn.Results.Add(benchmark, new List<int>());
        }

        public void Stop(Benchmark benchmark, BenchmarkReport report)
        {
        }

        public void ProcessStarted(Process process)
        {
        }

        public void AfterBenchmarkHasRun(Benchmark benchmark, Process process)
        {
            _output = process.StandardError.ReadToEnd();

            var matches = Regex.Matches(_output, @"CPU time = (\d+) ms");

            _sb.AppendLine("Regex.Matches.Count = " + matches.Count);


            if (matches.Count == 0)
            {
                _sb.AppendLine("No matches when parsing ouput:\r\n-------\r\n" + _output + "\r\n-------\r\n");
            }


            if (matches.Count != 25)
                throw new Exception("Regex matches is not 25. It is " + matches.Count);

            foreach (Match match in matches)
            {
                int i;
                var val = match.Groups[1].Value;

                if (!Int32.TryParse(val, out i))
                {
                    _sb.AppendLine("Error parsing: " + val);
                    continue;
                }
                
                CpuTimeColumn.Results[benchmark].Add(i);
            }

            _count = CpuTimeColumn.Results[benchmark].Count;
        }

        public void ProcessStopped(Process process)
        {
        }

        public void DisplayResults(ILogger logger)
        {
            logger.WriteLine("Count: " + _count);
            logger.WriteLine(_output);
            logger.WriteLine("------------------Errors------------------");
            logger.WriteLine(_sb.ToString());
        }
    }
}