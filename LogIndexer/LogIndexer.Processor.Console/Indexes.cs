using LogIndexer.Core.Data.Indexes;
using Raven.Client;

namespace LogIndexer.Processor.Console
{
    static internal class Indexes
    {
        public static void Create(IDocumentStore store)
        {
            Logger.Write("Creating indexes...");
            new Records_ByData().Execute(store);
            new Logs_Full().Execute(store);
            new Records_ByDataSourceId_Total().Execute(store);
            new Records_ByWebLog().Execute(store);
            Logger.WriteLine("Done!");
        }
    }
}