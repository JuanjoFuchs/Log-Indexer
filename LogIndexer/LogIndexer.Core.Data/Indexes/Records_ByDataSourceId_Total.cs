using System.Linq;
using LogIndexer.Core.Domain;
using Raven.Client.Indexes;

namespace LogIndexer.Core.Data.Indexes
{
    public class Records_ByDataSourceId_Total : AbstractIndexCreationTask<Record, Records_ByDataSourceId_Total.Result>
    {
        public Records_ByDataSourceId_Total()
        {
            Map = records => from record in records
                select new {record.DataSourceId, Count = 1};

            Reduce = results => from result in results
                group result by result.DataSourceId
                into g
                select new {DataSourceId = g.Key, Count = g.Sum(x => x.Count)};
        }

        public class Result
        {
            public string DataSourceId { get; set; }
            public int Count { get; set; }
        }
    }
}