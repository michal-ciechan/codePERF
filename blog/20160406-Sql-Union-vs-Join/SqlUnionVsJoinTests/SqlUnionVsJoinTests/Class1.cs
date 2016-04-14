
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace SqlUnionVsJoinTests
{
    [TestFixture]
    public class SqlUnionVsJoinTests
    {
        [Test]
        public void TestMethod()
        {
            BenchmarkRunner.Run<SqlUnionVsJoinBenchmark.PkClusteredIndexIntBenchmark>();
        }}
    
}
