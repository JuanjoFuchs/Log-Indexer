using System.IO;
using System.Linq;
using System.Threading;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace LogIndexer {
  public class LogIndexer {
    const int RAVEN_DEFAULT_PORT = 8080;
    const bool USE_HTTP_SERVER = true;

    readonly LogIndexerSettings _settings;
    readonly IDocumentStore _documentStore;

    public LogIndexer(LogIndexerSettings settings) {
      _settings = settings;
      NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(RAVEN_DEFAULT_PORT);
      _documentStore = new EmbeddableDocumentStore {
        DataDirectory = _settings.DataDirectory,
        UseEmbeddedHttpServer = USE_HTTP_SERVER
      };
      _documentStore.Initialize();
    }

    public void IndexExisting() {
      var files = Directory.GetFiles(_settings.Path);
      foreach (var file in files) {
        processFile(file);
      }
      waitForStaleIndexes();
    }

    void waitForStaleIndexes() {
      while (_documentStore.DatabaseCommands.GetStatistics().StaleIndexes.Any()) {
        Thread.Sleep(10);
      }
    }

    public void StartWatching() {
      var watcher = new FileSystemWatcher { Path = _settings.Path, NotifyFilter = NotifyFilters.LastWrite };
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