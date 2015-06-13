using System;
using System.Linq;
using System.Web.Http;
using LogIndexer.Core.Domain;
using Raven.Abstractions.Data;

namespace LogIndexer.Analysis.Web.Controllers
{
    [RoutePrefix("api/logs/{id}/search")]
    public class SearchController : ApiController
    {
        [Route("")]
        public IHttpActionResult Get(int id, string query)
        {
            using (var session = Store.Instance.OpenSession())
            {
                //var logId = session.Advanced.DocumentStore.Conventions
                //    .FindFullDocumentKeyFromNonStringIdentifier(id, typeof (Log), false);
                var log = session.Load<Log>(id);
                if (log == null)
                    throw new ArgumentException($"Couldn't find a log with Id: {id}", "id");

                var dataSourceIds = log
                    .DataSourceIds
                    .Select(x => $"DataSourceId: {x}");
                query = $"{string.Join(" AND ", dataSourceIds)} AND ({query})";
            }

            var result = Store.Instance.DatabaseCommands
                .Query("Records/ByData", new IndexQuery {Query = query});

            return Ok(new
            {
                Results = result.Results.Select(x => new
                {
                    Id = x["@metadata"].Value<string>("@id"),
                    Data = x["Data"].Value<string>()
                }).ToList(),
                result.DurationMilliseconds,
                result.TotalResults
            });
        }
    }
}