using System;
using System.Configuration;
using System.Linq;

namespace LogIndexer.Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var hostName = Environment.MachineName;
            var path = ConfigurationManager.AppSettings["Path"];
            var dataDirectory = ConfigurationManager.AppSettings["DataDirectory"];
            var logger = new ConsoleLogger();

            logger.Write("Starting Log Indexer on '{0}'...", path);
            var logIndexer = new LogIndexer(path, dataDirectory, logger);
            logger.WriteLine("Done!");

            logger.Write("Indexing existing files");
            logIndexer.IndexExisting();
            logger.WriteLine("Done!");

            logger.Write("Starting file watcher...");
            logIndexer.StartWatching();
            logger.WriteLine("Done!");

            logger.WriteLine("Visit http://{0}:8080 to view the indexed data", hostName);

            System.Console.ReadLine();
            logger.WriteLine("Stopping...");
        }
    }
}