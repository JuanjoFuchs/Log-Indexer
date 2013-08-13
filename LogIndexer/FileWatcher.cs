using System.Diagnostics.Eventing.Reader;
using System.IO;
using Raven.Client.Document;

namespace LogIndexer {
  public class FileWatcher {
    const string PATH = @"C:\Temp\logs";
    readonly DocumentStore _documentStore;

    public FileWatcher() {
      _documentStore = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "LogIndexer" };
      _documentStore.Initialize();
    }

    public void Watch() {
      var watcher = new FileSystemWatcher { Path = PATH, Filter = "*.log", NotifyFilter = NotifyFilters.LastWrite };
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
          var index = 0;
          while (reader.Peek() >= 0) {
            index++;
            var line = reader.ReadLine();
            if (index > log.LastLineNumber) {
              log.AddLine(new Line { Data = line, Number = index });
            }
          }
        }
      }
    }
  }
}