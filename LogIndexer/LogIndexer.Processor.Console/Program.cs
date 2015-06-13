using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Domain;
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

                var sources = SeedData(store);
                foreach (var source in sources)
                    IndexFile(store, source);
                CreateIndexes(store);
            }
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