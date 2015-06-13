using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Data;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Domain;
using Raven.Client;
using Environment = LogIndexer.Core.Domain.Environment;

namespace LogIndexer.Processor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (var store = Store.CreateStore())
            {
                //var sources = SeedData(store);
                //foreach (var source in sources)
                //    IndexFile(store, source);
                //CreateIndexes(store);
                CreateModels(store);
            }
        }

        private static void CreateModels(IDocumentStore store)
        {
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
                            System.Console.WriteLine("EndOfStream?");
                        }
                    }
                }
            }
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
            webLog.Message = string.Join("|", partsList);

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

        private static void CreateIndexes(IDocumentStore store)
        {
            new Records_ByData().Execute(store);
            new Logs_Full().Execute(store);
            new Records_ByDataSourceId_Total().Execute(store);
        }

        private static List<DataSource> SeedData(IDocumentStore store)
        {
            var dataSources = new List<DataSource>();

            using (var session = store.OpenSession())
            {
                var webApp = new Application {Name = "Web Application"};
                var oldService = new Application {Name = "Really old web service"};
                session.Store(webApp);
                session.Store(oldService);

                var dev = new Environment {Name = "Development"};
                var qa = new Environment {Name = "QA"};
                var staging = new Environment {Name = "Staging"};
                var production = new Environment {Name = "Production"};
                session.Store(dev);
                session.Store(qa);
                session.Store(staging);
                session.Store(production);

                var webServer = new Server { Name = "Web Server" };
                var oldServer = new Server {Name = "Old Server"};
                session.Store(webServer);
                session.Store(oldServer);

                var webAppFile = new DataSource
                {
                    ServerId = webServer.Id,
                    Name = "Local WebApplication.log",
                    Path = @"C:\dev\github\personal\Log-Indexer\LogIndexer\LogIndexer.Processor.Console\Logs",
                    File = "WebApplication.log"
                };
                session.Store(webAppFile);
                dataSources.Add(webAppFile);

                var oldServiceFile = new DataSource
                {
                    ServerId = oldServer.Id,
                    Name = "Local OldService.log",
                    Path = @"C:\dev\github\personal\Log-Indexer\LogIndexer\LogIndexer.Processor.Console\Logs",
                    File = "OldService.log"
                };
                session.Store(oldServiceFile);
                dataSources.Add(oldServiceFile);

                session.Store(new Log
                {
                    Name = "WebApplication log",
                    ApplicationId = webApp.Id,
                    EnvironmentId = dev.Id,
                    DataSourceIds = new[] {webAppFile.Id}
                });
                session.Store(new Log
                {
                    Name = "Some service log",
                    ApplicationId = oldService.Id,
                    EnvironmentId = qa.Id,
                    DataSourceIds = new[] {oldServiceFile.Id}
                });
                session.Store(new Log
                {
                    Name = "Another log",
                    ApplicationId = oldService.Id,
                    EnvironmentId = staging.Id
                });
                session.Store(new Log
                {
                    Name = "One more log",
                    ApplicationId = oldService.Id,
                    EnvironmentId = production.Id
                });

                session.SaveChanges();
            }

            return dataSources;
        }

        private static void IndexFile(IDocumentStore store, DataSource dataSource)
        {
            var lastLineNumber = 0;

            System.Console.WriteLine("Checking '{0}'", dataSource.Path);

            var file = Directory
                .GetFiles(dataSource.Path)
                .FirstOrDefault(x => Path.GetFileName(x) == dataSource.File);
            if (file == null)
                return;

            using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            using (var bulkInsert = store.BulkInsert())
            {
                try
                {
                    var index = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        index++;
                        if (index > lastLineNumber)
                        {
                            bulkInsert.Store(new Record {Data = line, DataSourceId = dataSource.Id});
                            System.Console.Write(".");
                        }
                    }
                }
                catch (EndOfStreamException)
                {
                    System.Console.WriteLine("Reached end of file");
                }
            }
        }
    }
}