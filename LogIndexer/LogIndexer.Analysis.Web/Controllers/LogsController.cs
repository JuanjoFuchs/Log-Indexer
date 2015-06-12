using System.Web.Http;
using LogIndexer.Core.Domain;
using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Analysis.Web.Controllers
{
    public class LogsController : ApiController
    {
        private readonly IDocumentStore _store;

        public LogsController()
        {
            _store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "test" };
            _store.Initialize();
        }

        public Log Get(int id)
        {
            using (var session = _store.OpenSession())
            {
                return session.Load<Log>(id);
            }
        }
    }
}