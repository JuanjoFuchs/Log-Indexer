using System.Linq;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using LogIndexer.Core.Data;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Domain;

namespace LogIndexer.Analysis.Web.Controllers
{
    [ODataRoutePrefix("logs")]
    [EnableQuery(HandleNullPropagation = HandleNullPropagationOption.False, EnableConstantParameterization = false)]
    public class LogsODataController : ODataController
    {
        [ODataRoute]
        public IQueryable<Logs_Full.Result> Get()
        {
            using (var session = Store.Instance.OpenSession())
            {
                return session
                    .Query<Log>()
                    .TransformWith<Logs_Full, Logs_Full.Result>();
            }
        }
    }
}