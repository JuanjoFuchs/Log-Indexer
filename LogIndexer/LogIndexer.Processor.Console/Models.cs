using System;
using System.Globalization;
using System.IO;
using System.Linq;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Domain;
using Raven.Client;

namespace LogIndexer.Processor.Console
{
    static internal class Models
    {
        public static void Create(IDocumentStore store)
        {
            Logger.Write("Creating models...");
            using (var session = store.OpenSession())
            {
                var dataSourceId = "DataSources/1";

                var query = session
                    .Query<Record>("Records/ByData")
                    .Where(x => x.DataSourceId == dataSourceId);
                var results = session.Advanced.Stream(query);
                using (var bulkInsert = store.BulkInsert())
                {
                    while (results.MoveNext())
                    {
                        try
                        {
                            var current = results.Current;
                            var record = current.Document;
                            var transform = Transform(record);
                            if (transform != null)
                                bulkInsert.Store(transform);
                        }
                        catch (EndOfStreamException)
                        {
                            Logger.WriteLine("EndOfStream");
                        }
                    }
                }
            }

            Logger.WriteLine("Done!");
        }

        private static object Transform(Record record)
        {
            var parts = record.Data
                .Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
            DateTime date;
            if (
                !(parts.Length >= 4 && parts[0].Length == 14 &&
                  DateTime.TryParseExact(parts[0], "yyyyMMddHHmmss", CultureInfo.CurrentCulture, DateTimeStyles.None,
                      out date)))
                return null;

            var webLog = new WebLog
            {
                Date = date,
                Level = parts[1],
                Controller = parts[2]
            };

            var partsList = parts.ToList();
            partsList.RemoveRange(0, 3);
            webLog.Message = String.Join("|", partsList);

            if (webLog.Level != "Error" && !webLog.Message.Contains("--->"))
                return webLog;

            var errors = webLog.Message
                .Split(new[] {"--->"}, StringSplitOptions.RemoveEmptyEntries)
                .Select(s =>
                {
                    var indexOf = s.IndexOf(":", StringComparison.Ordinal);
                    return new LoggedException
                    {
                        Type = s.Substring(0, indexOf).Trim(),
                        Message = s.Substring(indexOf + 1).Trim()
                    };
                })
                .ToList();

            var webLogError = new WebLogError
            {
                Date = webLog.Date,
                Controller = webLog.Controller,
                Errors = errors
            };

            return webLogError;
        }
    }
}