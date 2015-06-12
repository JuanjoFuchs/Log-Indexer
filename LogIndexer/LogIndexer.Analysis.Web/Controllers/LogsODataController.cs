using System.Linq;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using LogIndexer.Core.Domain;
using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Analysis.Web.Controllers
{
    [ODataRoutePrefix("logs")]
    [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False, EnableConstantParameterization = false)]
    public class LogsODataController : ODataController
    {
        private readonly IDocumentStore _store;

        public LogsODataController()
        {
            _store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "test" };
            _store.Initialize();
        }

        [ODataRoute]
        public IQueryable<Log> Get()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<Log>();
            }
        }
    }
}