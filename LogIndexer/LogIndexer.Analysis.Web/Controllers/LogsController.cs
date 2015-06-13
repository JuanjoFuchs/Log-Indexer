using System.Linq;
using System.Web.Http;
using LogIndexer.Core.Data;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Domain;

namespace LogIndexer.Analysis.Web.Controllers
{
    [RoutePrefix("api/logs")]
    public class LogsController : ApiController
    {
        [Route("")]
        public IQueryable<Logs_Full.Result> Get()
        {
            using (var session = Store.Instance.OpenSession())
                return session
                    .Query<Log>()
                    .TransformWith<Logs_Full, Logs_Full.Result>();
        }

        [Route("{id}")]
        public Logs_Full.Result Get(int id)
        {
            using (var session = Store.Instance.OpenSession())
                return session.Load<Logs_Full, Logs_Full.Result>($"logs/{id}");
        }
    }

    [RoutePrefix("api/records/totals")]
    public class RecordsTotalsController : ApiController
    {
        [Route("byDataSourceId")]
        [HttpGet]
        public IQueryable<Records_ByDataSourceId_Total.Result> ByDataSourceId()
        {
            using (var session = Store.Instance.OpenSession())
                return session
                    .Query<Records_ByDataSourceId_Total.Result, Records_ByDataSourceId_Total>();
        }
    }
}