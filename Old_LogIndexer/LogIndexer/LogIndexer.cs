using System.IO;
using System.Linq;
using System.Threading;
using Raven.Client;
using Raven.Client.Embedded;
using Raven.Database.Server;

namespace LogIndexer
{
    public class LogIndexer
    {
        const int RAVEN_DEFAULT_PORT = 8080;
        const bool USE_HTTP_SERVER = true;

        IDocumentStore _documentStore;
        readonly string _path;
        readonly ILogger _logger;

        public LogIndexer(string path, string dataDirectory, ILogger logger)
        {
            _path = path;
            _logger = logger;
            startRaven(dataDirectory);
        }

        void startRaven(string dataDirectory)
        {
            NonAdminHttp.EnsureCanListenToWhenInNonAdminContext(RAVEN_DEFAULT_PORT);
            _documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = dataDirectory,
                UseEmbeddedHttpServer = USE_HTTP_SERVER
            };
            _documentStore.Initialize();
        }

        public void IndexExisting()
        {
            var files = Directory.GetFiles(_path);
            foreach (var file in files)
            {
                _logger.Write(".");
                processFile(file);
            }
            waitForStaleIndexes();
        }

        void waitForStaleIndexes()
        {
            while (_documentStore.DatabaseCommands.GetStatistics().StaleIndexes.Any())
            {
                Thread.Sleep(10);
            }
        }

        public void StartWatching()
        {
            var watcher = new FileSystemWatcher { Path = _path, NotifyFilter = NotifyFilters.LastWrite };
            watcher.Changed += watcherOnChanged;
            watcher.EnableRaisingEvents = true;
        }

        void watcherOnChanged(object sender, FileSystemEventArgs fileSystemEventArgs)
        {
            _logger.WriteLine("Change detected on {0}", fileSystemEventArgs.FullPath);
            processFile(fileSystemEventArgs.FullPath);
        }

        void processFile(string path)
        {
            using (var session = _documentStore.OpenSession())
            {
                var log = session.Load<Log>(path.GetHashCode());
                if (log == null)
                {
                    log = new Log(path);
                    session.Store(log);
                }
                processLog(log);
                session.SaveChanges();
            }
        }

        void processLog(Log log)
        {
            using (var stream = new FileStream(log.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    processLines(log, reader);
                }
            }
        }

        void processLines(Log log, StreamReader reader)
        {
            using (var bulk = _documentStore.BulkInsert())
            {
                var index = 0;
                while (reader.Peek() >= 0)
                {
                    index++;
                    var line = reader.ReadLine();
                    if (index > log.LastLineNumber)
                    {
                        bulk.Store(log.CreateLine(line, index));
                    }
                }
            }
        }
    }
}