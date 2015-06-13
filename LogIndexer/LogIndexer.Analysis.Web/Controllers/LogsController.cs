using System.Linq;
using System.Web.Http;
using LogIndexer.Core.Domain;
using LogIndexer.Processor.Data.Indexes;

namespace LogIndexer.Analysis.Web.Controllers
{
    [RoutePrefix("api/logs")]
    public class LogsController : ApiController
    {
        public IQueryable<Logs_Full.Result> Get()
        {
            using (var session = Store.Instance.OpenSession())
                return session.Query<Log>()
                    .TransformWith<Logs_Full, Logs_Full.Result>();
        }

        [Route("{id}")]
        public Logs_Full.Result Get(int id)
        {
            using (var session = Store.Instance.OpenSession())
                return session.Load<Logs_Full, Logs_Full.Result>($"logs/{id}");
        }
    }
}