using System.IO;
using System.Linq;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Domain;
using Newtonsoft.Json;
using Raven.Client;
using RestSharp;

static internal class Logs
{
    public static void Process(IDocumentStore store, int logId)
    {
        Logger.WriteLine("Processing logs...");
        Logger.WriteLine($"Getting log information for {logId}...");
        var client = new RestClient("http://localhost:2385");
        var request = new RestRequest("api/logs/{id}", Method.GET);
        request.AddUrlSegment("id", logId.ToString());
        var response = client.Execute(request);

        var log = JsonConvert.DeserializeObject<Logs_Full.Result>(response.Content);
        Logger.WriteLine($"Got log information for {log.Name}");
        foreach (var dataSource in log.DataSources)
            IndexFile(store, dataSource);
    }

    private static void IndexFile(IDocumentStore store, DataSource dataSource)
    {
        var lastLineNumber = 0;

        Logger.WriteLine($"Looking for files in '{dataSource.Path}'...");

        var file = Directory
            .GetFiles(dataSource.Path)
            .FirstOrDefault(x => Path.GetFileName(x) == dataSource.File);
        if (file == null)
            return;

        using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var reader = new StreamReader(stream))
            try
            {
                using (var bulkInsert = store.BulkInsert())
                {
                    Logger.Write($"Processing '{file}'");
                    var index = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        index++;
                        if (index > lastLineNumber)
                        {
                            bulkInsert.Store(new Record {Data = line, DataSourceId = dataSource.Id});
                            Logger.Write(".");
                        }
                    }
                }

                Logger.Write("Done!");
            }
            catch (EndOfStreamException)
            {
                Logger.WriteLine("Reached end of file");
            }
    }
}