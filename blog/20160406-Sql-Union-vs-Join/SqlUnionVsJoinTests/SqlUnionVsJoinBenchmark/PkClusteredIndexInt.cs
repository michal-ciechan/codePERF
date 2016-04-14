using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using BenchmarkDotNet.Attributes;

namespace SqlUnionVsJoinBenchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class PkClusteredIndexIntBenchmark
    {

        public static List<int> Query1CpuTime = new List<int>();

        [Benchmark(Description = "1. Union All")]
        public int UnionAll()
        {
            var sql = @"
	SELECT BusinessEntityID, ModifiedDate, rowguid FROM Person.Person
	UNION ALL
	SELECT BusinessEntityID, ModifiedDate, rowguid FROM Person.Person
";
            
            var res = GetStats(sql);

            WriteOutput(res);
            return 0;
        }

        [Benchmark(Description = "2. Union")]
        public int Union()
        {
            var sql = @"
	SELECT BusinessEntityID, ModifiedDate, rowguid FROM Person.Person
	UNION
	SELECT BusinessEntityID, ModifiedDate, rowguid FROM Person.Person
";

            var res = GetStats(sql);

            WriteOutput(res);
            return 0;
        }

        [Benchmark(Description = "3. Inner Join")]
        public int InnerJoin()
        {
            var sql = @"
	SELECT p1.BusinessEntityID, p1.ModifiedDate, p1.rowguid
		,  p2.BusinessEntityID, p2.ModifiedDate, p2.rowguid 
	FROM Person.Person p1 
	INNER JOIN Person.Person p2 
		ON p2.BusinessEntityID = p1.BusinessEntityID
";
            var res = GetStats(sql);

            WriteOutput(res);
            return 0;
        }

        [Benchmark(Description = "4. Left Join")]
        public int LeftJoin()
        {
            var sql = @"
	SELECT p1.BusinessEntityID, p1.ModifiedDate, p1.rowguid
		,  p2.BusinessEntityID, p2.ModifiedDate, p2.rowguid 
	FROM Person.Person p1 
	LEFT JOIN Person.Person p2 
		ON p2.BusinessEntityID = p1.BusinessEntityID
";
            var res = GetStats(sql);

            WriteOutput(res);
            return 0;
        }

        [Benchmark(Description = "5. Right Join")]
        public int RightJoin()
        {
            var sql = @"
	SELECT p1.BusinessEntityID, p1.ModifiedDate, p1.rowguid
		,  p2.BusinessEntityID, p2.ModifiedDate, p2.rowguid 
	FROM Person.Person p1 
	RIGHT JOIN Person.Person p2 
		ON p2.BusinessEntityID = p1.BusinessEntityID
";
            var res = GetStats(sql);

            WriteOutput(res);
            return 0;
        }

        [Benchmark(Description = "6. Full Outer Join")]
        public int FullOuterJoin()
        {
            var sql = @"
	SELECT p1.BusinessEntityID, p1.ModifiedDate, p1.rowguid
		,  p2.BusinessEntityID, p2.ModifiedDate, p2.rowguid 
	FROM Person.Person p1 
	FULL OUTER JOIN Person.Person p2 
		ON p2.BusinessEntityID = p1.BusinessEntityID
";
            var res = GetStats(sql);

            WriteOutput(res);
            return 0;
        }

        private static string GetStats(string sql)
        {
            var msg = new StringBuilder();
            var sqlPre = "SET STATISTICS TIME ON;";

            using (
                var con =
                    new SqlConnection(
                        @"Data Source=MC-PC\SQLEXPRESS;Initial Catalog=AdventureWorks2014;Integrated Security=True"))
            using (var cmd = new SqlCommand(sqlPre + " ; " + sql, con))
            {
                con.InfoMessage += (sender, args) =>
                    msg.AppendLine(args.Message);
                con.Open();
                cmd.ExecuteNonQuery();
            }

            var res = msg.ToString();
            return res;
        }

        private void WriteOutput(string output)
        {
            Console.Error.WriteLine("Test-" + output);
        }
    }
}