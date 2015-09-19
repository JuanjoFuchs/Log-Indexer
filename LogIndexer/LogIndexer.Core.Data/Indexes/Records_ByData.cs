using System;
using System.Linq;
using LogIndexer.Core.Data.Plugins;
using LogIndexer.Core.Domain;
using Lucene.Net.Analysis;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;
using Raven.Client.Linq;

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

    public class Records_ByWebLog : AbstractIndexCreationTask<Record>
    {
        public Records_ByWebLog()
        {
            Map = records => records
                .Select(record => new
                {
                    record,
                    data = record.Data.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries)
                })
                .Where(record => record.data.Length >= 4 && record.data[0].Length == 14)
                .Select(record => new
                {
                    record,
                    dateTimeOffset = LogIndexerUtils.TryParseExact(record.data[0], "yyyyMMddHHmmss")
                })
                .Where(record => record.dateTimeOffset.HasValue)
                .Select(record => new
                {
                    record.record.record.DataSourceId,
                    Date = record.dateTimeOffset.Value,
                    Level = record.record.data[1],
                    Controller = record.record.data[2],
                    Message = string.Join(System.Environment.NewLine, record.record.data.Except(new[]
                    {
                        record.record.data[0],
                        record.record.data[1],
                        record.record.data[2]
                    }))
                });

            StoreAllFields(FieldStorage.Yes);
        }
    }

    public class Records_ByWebLogError : AbstractIndexCreationTask<Record>
    {
        public Records_ByWebLogError()
        {
            //Map = records => records
            //    .Select(record => TransformWith<Record, WebLog>("WebLog/Transformer", record).FirstOrDefault())
            //    .Where(webLog => webLog != null && webLog.Level == "Error")
            //    .Select(webLog => new { webLog, errors = webLog.Message.Split(new[] { "--->" }, StringSplitOptions.RemoveEmptyEntries) })
            //    .Where(record => record.errors.Length >= 1)
            //    .Select(record => new
            //    {
            //        record.webLog.Date,
            //        record.webLog.Controller,
            //        Errors = LogIndexerUtils.Select(record.errors, error => new
            //        {
            //            Type = Regex.Match(error, @"(?<type>[a-zA-Z\.]+)\:").Groups["type"].Value,
            //            Message = error
            //        })
            //    });
        }
    }
}