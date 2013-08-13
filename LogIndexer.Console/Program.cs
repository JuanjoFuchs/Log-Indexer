namespace LogIndexer.Console {
  internal class Program {
    static void Main(string[] args) {
      var logIndexer = new LogIndexer();
      System.Console.Write("Indexing existing files...");
      logIndexer.IndexExisting();
      System.Console.WriteLine("Done");
      logIndexer.StartWatching();
      System.Console.WriteLine("Log watcher started!");
      System.Console.ReadLine();
    }
  }
}