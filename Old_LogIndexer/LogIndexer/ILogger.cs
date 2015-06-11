namespace LogIndexer {
  public interface ILogger {
    void WriteLine(string info, params object[] args);

    void Write(string info, params object[] args);
  }
}