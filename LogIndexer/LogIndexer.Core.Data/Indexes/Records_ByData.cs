using System.Linq;
using LogIndexer.Core.Domain;
using Lucene.Net.Analysis;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace LogIndexer.Core.Data.Indexes
{
    public class Records_ByData : AbstractIndexCreationTask<Record>
    {
        public Records_ByData()
        {
            Map = records => from record in records
                select new {record.DataSourceId, record.Data};

            Indexes.Add(x => x.Data, FieldIndexing.Analyzed);

            Analyzers.Add(x => x.Data, typeof(SimpleAnalyzer).AssemblyQualifiedName);
        }
    }
}