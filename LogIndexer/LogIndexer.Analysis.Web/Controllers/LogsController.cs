using System.Linq;
using System.Web.Http;
using LogIndexer.Core.Domain;
using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Analysis.Web.Controllers
{
    public class LogsController : ApiController
    {
        [Queryable]
        public IQueryable<Log> Get()
        {
            using (IDocumentStore store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "test" })
            using (var session = store.OpenSession())
            {
                return session.Query<Log>();
            }
        }
    }
}