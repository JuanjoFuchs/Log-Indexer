using System.Configuration;

namespace LogIndexer.Console {
  internal class Program {
    static void Main(string[] args) {
      var settings = new LogIndexerSettings {
        Path = ConfigurationManager.AppSettings["Path"],
        DataDirectory = ConfigurationManager.AppSettings["DataDirectory"],
      };
      System.Console.WriteLine("Starting Log Indexer");
      var logIndexer = new LogIndexer(settings);
      System.Console.Write("Indexing existing files...");
      logIndexer.IndexExisting();
      System.Console.WriteLine("Done");
      logIndexer.StartWatching();
      System.Console.WriteLine("Log watcher started!");
      System.Console.WriteLine("Visit http://localhost:8080 to view the indexed data");
      System.Console.ReadLine();
    }
  }
}