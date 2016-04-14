using System.Linq;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Helpers;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;

namespace SqlUnionVsJoinBenchmark
{
    public class WordPressMarkdownExporter : ExporterBase
    {
        protected override string FileExtension => "md";
        protected override string FileNameSuffix => $"-{Dialect.ToLower()}";

        private string Dialect { get; set; }

        private string prefix = string.Empty;
        private bool useCodeBlocks = false;
        private string codeBlocksSyntax = string.Empty;
        private bool startOfGroupInBold = false;

        public WordPressMarkdownExporter()
        {
            Dialect = "WordPress";
            prefix = "|";
            startOfGroupInBold = true;
        }

        public override void ExportToLog(Summary summary, ILogger logger)
        {
            if (useCodeBlocks)
                logger.WriteLine($"```{codeBlocksSyntax}");
            logger = GetRightLogger(logger);
            logger.WriteLine();
            foreach (var infoLine in EnvironmentHelper.GetCurrentInfo().ToFormattedString("Host"))
            {
                logger.WriteLineInfo(infoLine);
            }
            logger.WriteLine();

            PrintTable(summary.Table, logger);

            // TODO: move this logic to an analyser
            var benchmarksWithTroubles = summary.Reports.Values.Where(r => !BenchmarkReportExtensions.GetResultRuns(r).Any()).Select(r => r.Benchmark).ToList();
            if (benchmarksWithTroubles.Count > 0)
            {
                logger.WriteLine();
                logger.WriteLineError("Benchmarks with issues:");
                foreach (var benchmarkWithTroubles in benchmarksWithTroubles)
                    logger.WriteLineError("  " + benchmarkWithTroubles.ShortInfo);
            }
        }

        private ILogger GetRightLogger(ILogger logger)
        {
            if (string.IsNullOrEmpty(prefix)) // most common scenario!! we don't need expensive LoggerWithPrefix
            {
                return logger;
            }

            return new LoggerWithPrefix(logger, prefix);
        }

        private void PrintTable(SummaryTable table, ILogger logger)
        {
            if (table.FullContent.Length == 0)
            {
                logger.WriteLineError("There are no benchmarks found ");
                logger.WriteLine();
                return;
            }

            table.PrintCommonColumns(logger);
            logger.WriteLine();

            if (useCodeBlocks)
            {
                logger.Write("```");
                logger.WriteLine();
            }

            table.PrintLine(table.FullHeader, logger, string.Empty, " |");
            logger.WriteLineStatistic(string.Join("", table.Columns.Where(c => c.NeedToShow).Select(c => new string('-', c.Width) + " |")));
            var rowCounter = 0;
            var highlightRow = false;
            foreach (var line in table.FullContent)
            {
                // Each time we hit the start of a new group, alternative the colour (in the console) or display bold in Markdown
                if (table.FullContentStartOfGroup[rowCounter])
                {
                    highlightRow = !highlightRow;
                }

                table.PrintLine(line, logger, string.Empty, " |", highlightRow, table.FullContentStartOfGroup[rowCounter], startOfGroupInBold);
                rowCounter++;
            }
        }
    }
}