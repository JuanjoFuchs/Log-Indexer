using System.Linq;
using System.Web.Http;
using LogIndexer.Core.Domain;
using Raven.Abstractions.Data;
using Raven.Client;
using Raven.Client.Document;

namespace LogIndexer.Analysis.Web.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IDocumentStore _store;

        public SearchController()
        {
            _store = new DocumentStore { Url = "http://localhost:8080", DefaultDatabase = "test" };
            _store.Initialize();
        }

        public IHttpActionResult Get(string query)
        {
            var result = _store.DatabaseCommands.Query("Records/ByData", new IndexQuery {Query = query});

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