using System.Linq;
using System.Web.Http;
using LogIndexer.Core.Data;
using LogIndexer.Core.Data.Indexes;

namespace LogIndexer.Analysis.Web.Controllers
{
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