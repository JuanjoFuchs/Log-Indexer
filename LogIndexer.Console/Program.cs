namespace LogIndexer.Console {
  internal class Program {
    static void Main(string[] args) {
      var fileWatcher = new FileWatcher();
      fileWatcher.Watch();
      System.Console.ReadLine();
    }
  }
}