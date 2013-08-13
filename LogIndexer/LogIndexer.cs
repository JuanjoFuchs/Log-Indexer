using System.IO;
using Raven.Client.Document;

namespace LogIndexer {
  public class LogIndexer {
    const string PATH = @"C:\Temp\logs";
    //const string PATH = @"C:\dev\github\tzt\Host\Tzt.WinService.Host\bin\Debug";
    readonly DocumentStore _documentStore;

    public LogIndexer() {
      _documentStore = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "LogIndexer" };
      _documentStore.Initialize();
    }

    public void IndexExisting() {
      var files = Directory.GetFiles(PATH);
      foreach (var file in files) {
        processFile(file);
      }
    }

    public void StartWatching() {
      var watcher = new FileSystemWatcher { Path = PATH, NotifyFilter = NotifyFilters.LastWrite };
      watcher.Changed += watcherOnChanged;
      watcher.EnableRaisingEvents = true;
    }

    void watcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs) {
      processFile(fileSystemEventArgs.FullPath);
    }

    void processFile(string path) {
      using (var session = _documentStore.OpenSession()) {
        var log = session.Load<Log>(path.GetHashCode());
        if (log == null) {
          log = new Log(path);
          session.Store(log);
        }
        processLog(log);
        session.SaveChanges();
      }
    }

    void processLog(Log log) {
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