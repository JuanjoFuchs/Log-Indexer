using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Data;
using LogIndexer.Core.Data.Indexes;
using LogIndexer.Core.Data.Transforms;
using LogIndexer.Core.Domain;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Connection;
using Raven.Client.Document;
using Raven.Client.Linq;
using Raven.Json.Linq;

namespace LogIndexer.Analysis.Web.Controllers
{
    [RoutePrefix("api/logs/{id}/search")]
    public class SearchController : ApiController
    {
        [HttpGet]
        [Route("byText")]
        public IHttpActionResult ByText(int id, string query)
        {
            return Query(id, query, "Records/ByData", y => new
            {
                Id = y["@metadata"].Value<string>("@id"),
                Data = y["Data"].Value<string>()
            });
        }

        private IHttpActionResult Query(int id, string query, string index, Func<RavenJObject, object> transform, bool indexEntriesOnly = false)
        {
            var log = GetLog(id);
            query = ParseQuery(query, log.DataSourceIds);

            var result = Store.Instance.DatabaseCommands.Query(index, new IndexQuery {Query = query}, indexEntriesOnly: indexEntriesOnly);

            return Ok(ParseResults(result, transform));
        }

        [HttpGet]
        [Route("byModel/{model}")]
        public IHttpActionResult ByModel(int id, string model, string query)
        {
            return Query(id, query, $"Records/By{model}", o => o.Deserialize(typeof(WebLog), new DocumentConvention()), true);

            //var log = GetLog(id);
            //query = ParseQuery(query, log.DataSourceIds);

            //using (var session = Store.Instance.OpenSession())
            //{
            //    var results = session.Advanced.DocumentQuery<Record>($"Records/By{model}")
            //        .Where(query)
            //        .SelectFields<WebLog>();

            //    return Ok(ParseResults(results.QueryResult, o => o.Deserialize(typeof(WebLog), new DocumentConvention())));
            //}
        }

        public IHttpActionResult ByLinq(int id, string query)
        {
            object obj;
            var type = RoslynHelper.Compile(query, out obj);
            using (var session = Store.Instance.OpenSession())
            {
                //session.Advanced.DocumentQuery<Record>("Records/ByWebLog")
                //    .SelectFields<WebLog>()
                //    .Where(query);
                var result = type.InvokeMember("Compile",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    new object[] { session });

                return Ok(result);
            }
        }

        private static object ParseResults(QueryResult result, Func<RavenJObject, object> transform = null)
        {
            var results = transform != null
                ? result.Results.Select(transform)
                : result.Results;

            return new
            {
                Results = results.ToList(),
                result.DurationMilliseconds,
                result.TotalResults
            };
        }

        private static string ParseQuery(string query, IEnumerable<string> sourceIds)
        {
            var dataSourceIds = sourceIds
                .Select(x => $"DataSourceId: {x}");
            query = $"{string.Join(" AND ", dataSourceIds)} AND ({query})";
            return query;
        }

        private static Log GetLog(int id)
        {
            using (var session = Store.Instance.OpenSession())
            {
                var log = session.Load<Log>(id);
                if (log == null)
                    throw new ArgumentException($"Couldn't find a log with Id: {id}", nameof(id));

                return log;
            }
        }
    }
}