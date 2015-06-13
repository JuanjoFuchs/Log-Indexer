using System.Linq;
using LogIndexer.Core.Domain;
using Raven.Client.Indexes;

namespace LogIndexer.Core.Data.Indexes
{
    public class Records_ByLogId_Total : AbstractMultiMapIndexCreationTask<Records_ByLogId_Total.Result>
    {
        public Records_ByLogId_Total()
        {
            AddMap<Record>(records =>
                from record in records
                select new
                {
                    record.DataSourceId,
                    Count = 1,
                    Collection = "Records"
                });

            AddMap<Log>(logs =>
                from log in logs
                from dataSourceId in Recurse(log, l => l.DataSourceIds)
                select new
                {
                    DataSourceId = dataSourceId,
                    Count = 1,
                    Collection = "Logs"
                });

            Reduce = results =>
                from result in results
                where result.Collection == "Records"
                group result by new { result.DataSourceId, result.LogId }
                into g
                select new {LogId = g.Key, Count = g.Count()};
        }

        public class Result
        {
            public string Collection { get; set; }
            public string DataSourceId { get; set; }
            public string LogId { get; set; }
        }
    }
}