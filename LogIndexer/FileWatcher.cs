using System.IO;
using Raven.Client.Document;

namespace LogIndexer {
  public class FileWatcher {
    //const string PATH = @"C:\Temp\logs";
    const string PATH = @"C:\dev\github\tzt\Host\Tzt.WinService.Host\bin\Debug";
    readonly DocumentStore _documentStore;

    public FileWatcher() {
      _documentStore = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "LogIndexer" };
      _documentStore.Initialize();
    }

    public void Watch() {
      var watcher = new FileSystemWatcher { Path = PATH, Filter = "*.txt", NotifyFilter = NotifyFilters.LastWrite };
      watcher.Changed += watcherOnChanged;
      watcher.EnableRaisingEvents = true;
    }

    void watcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs) {
      var path = fileSystemEventArgs.FullPath;
      processLog(path);
    }

    void processLog(string path) {
      using (var session = _documentStore.OpenSession()) {
        var log = session.Load<Log>(path.GetHashCode());
        if (log == null) {
          log = new Log(path);
          session.Store(log);
        }
        processFile(log);
        session.SaveChanges();
      }
    }

    void processFile(Log log) {
      using (var stream = new FileStream(log.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
        using (var reader = new StreamReader(stream)) {
          processLines(log, reader);
        }
      }
    }

    void processLines(Log log, StreamReader reader) {
      using (var bulk = _documentStore.BulkInsert()) {
        var index = 0;
        while (reader.Peek() >= 0) {
          index++;
          var line = reader.ReadLine();
          if (index > log.LastLineNumber) {
            bulk.Store(log.CreateLine(line, index));
          }
        }
      }
    }
  }
}