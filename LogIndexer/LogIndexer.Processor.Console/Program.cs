using System.IO;
using LogIndexer.Core.Domain;
using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Processor.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var path = @"C:\dev\stash\noble-implementation\Cignium.CallCenter\Cignium.CallCenter.Web\CUY.log";
            var lastLineNumber = 0;

            System.Console.WriteLine("Starting to read '{0}'", path);

            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (var reader = new StreamReader(stream))
            using (IDocumentStore store = new DocumentStore {Url = "http://localhost:8080", DefaultDatabase = "test"})
            {
                store.Initialize();

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
    }
}
