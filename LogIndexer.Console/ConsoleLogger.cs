namespace LogIndexer.Console {
  internal class ConsoleLogger : ILogger {
    public void Write(string info, params object[] args) {
      System.Console.Write(info, args);
    }

    public void WriteLine(string info, params object[] args) {
      System.Console.WriteLine(info, args);
    }
  }
}