using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogIndexer.Core.Domain;
using LogIndexer.Processor.Data.Indexes;
using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Processor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (IDocumentStore store = new DocumentStore {Url = "http://localhost:8080", DefaultDatabase = "test"})
            {
                store.Initialize();

                //var sources = SeedData(store);
                //foreach (var source in sources)
                //    IndexFile(store, source);
                CreateIndexes(store);
            }
        }

        private static void CreateIndexes(IDocumentStore store)
        {
            new Records_ByData().Execute(store);
            new Logs_Full().Execute(store);
        }

        private static Dictionary<string, DataSource> SeedData(IDocumentStore store)
        {
            var webAppFile = new DataSource
            {
                Name = "Local WebApplication.log",
                Path = @"C:\dev\github\personal\Log-Indexer\LogIndexer\LogIndexer.Processor.Console\Logs",
                File = "WebApplication.log"
            };
            var oldServiceFile = new DataSource
            {
                Name = "Local OldService.log",
                Path = @"C:\dev\github\personal\Log-Indexer\LogIndexer\LogIndexer.Processor.Console\Logs",
                File = "OldService.log"
            };

            string webAppFileId;
            string oldServiceFileId;
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

                session.Store(webAppFile);
                session.Store(oldServiceFile);

                webAppFileId = session.Advanced.GetDocumentId(webAppFile);
                session.Store(new Log
                {
                    Name = "WebApplication log",
                    ApplicationId = session.Advanced.GetDocumentId(webApp),
                    EnvironmentId = session.Advanced.GetDocumentId(dev),
                    DataSourceIds = new[] { webAppFileId }
                });
                var oldServiceId = session.Advanced.GetDocumentId(oldService);
                oldServiceFileId = session.Advanced.GetDocumentId(oldServiceFile);
                session.Store(new Log
                {
                    Name = "Some service log",
                    ApplicationId = oldServiceId,
                    EnvironmentId = session.Advanced.GetDocumentId(qa),
                    DataSourceIds = new[] { oldServiceFileId}
                });
                session.Store(new Log
                {
                    Name = "Another log",
                    ApplicationId = oldServiceId,
                    EnvironmentId = session.Advanced.GetDocumentId(staging)
                });
                session.Store(new Log
                {
                    Name = "One more log",
                    ApplicationId = oldServiceId,
                    EnvironmentId = session.Advanced.GetDocumentId(production)
                });

                session.SaveChanges();
            }

            return new Dictionary<string, DataSource>
            {
                { webAppFileId, webAppFile},
                { oldServiceFileId, oldServiceFile}
            };
        }

        private static void IndexFile(IDocumentStore store, KeyValuePair<string, DataSource> dataSourceWithId)
        {
            var lastLineNumber = 0;

            var dataSource = dataSourceWithId.Value;
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
                            bulkInsert.Store(new Record {Data = line, DataSourceId = dataSourceWithId.Key});
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