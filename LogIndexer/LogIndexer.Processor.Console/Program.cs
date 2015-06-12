using System.IO;
using LogIndexer.Core.Domain;
using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using System.Linq;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;

namespace LogIndexer.Processor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            using (IDocumentStore store = new DocumentStore {Url = "http://localhost:8080", DefaultDatabase = "test"})
            {
                store.Initialize();
                //IndexFile(store);
                //SeedData(store);
                CreateIndexes(store);
            }
        }

        private static void CreateIndexes(IDocumentStore store)
        {
            new Records_ByData().Execute(store);
        }

        private static void SeedData(IDocumentStore store)
        {
            using (var session = store.OpenSession())
            {
                session.Store(new Log {Name = "WebApplication log"});
                session.Store(new Log {Name = "Some service log"});
                session.Store(new Log {Name = "Another log"});
                session.Store(new Log {Name = "One more log"});
                session.SaveChanges();
            }
        }

        private static void IndexFile(IDocumentStore store)
        {
            var path = @"C:\dev\stash\noble-implementation\Cignium.CallCenter\Cignium.CallCenter.Web\CUY.log";
            var lastLineNumber = 0;

            System.Console.WriteLine("Starting to read '{0}'", path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                            bulkInsert.Store(new Record {Data = line});
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

    public class Records_ByData : AbstractIndexCreationTask<Record>
    {
        public Records_ByData()
        {
            Map = records => from record in records
                             select new {record.Data};

            Indexes.Add(x => x.Data, FieldIndexing.Analyzed);

            Analyzers.Add(x => x.Data, typeof(SimpleAnalyzer).AssemblyQualifiedName);
        }
    }
}