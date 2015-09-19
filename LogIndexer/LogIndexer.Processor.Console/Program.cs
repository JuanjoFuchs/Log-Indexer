using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CommandLine;
using LogIndexer.Core.Data;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Data.Transforms;
using LogIndexer.Core.Domain;
using Newtonsoft.Json;
using Raven.Client;
using RestSharp;
using Environment = LogIndexer.Core.Domain.Environment;

namespace LogIndexer.Processor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parserResult = Parser.Default.ParseArguments<Options>(args);
            if (parserResult.Errors.Any())
                throw new AggregateException(parserResult.Errors.Select(e => new ArgumentException(e.ToString())));

            var options = parserResult.Value;
            using (var store = Store.CreateStore())
            {
                if (options.Seed)
                    Seed(store);

                if (options.Index)
                    Indexes.Create(store);

                if (options.ProcessLog != 0)
                    Logs.Process(store, options.ProcessLog);

                if (options.Models)
                    Models.Create(store);

                //new WebLog_Transformer().Execute(store);
                //new WebLogError_Transformer().Execute(store);
            }
        }

        private static void Seed(IDocumentStore store)
        {
            Logger.Write("Seeding data...");
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

                var webServer = new Server {Name = "Web Server"};
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

                var oldServiceFile = new DataSource
                {
                    ServerId = oldServer.Id,
                    Name = "Local OldService.log",
                    Path = @"C:\dev\github\personal\Log-Indexer\LogIndexer\LogIndexer.Processor.Console\Logs",
                    File = "OldService.log"
                };
                session.Store(oldServiceFile);

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

            Logger.WriteLine("Done!");
        }
    }
}