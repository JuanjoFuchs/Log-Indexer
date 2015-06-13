using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using LogIndexer.Analysis.Domain;
using LogIndexer.Core.Data;
using LogIndexer.Core.Domain;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Raven.Abstractions.Data;

namespace LogIndexer.Analysis.Web.Controllers
{
    [RoutePrefix("api/logs/{id}/search")]
    public class SearchController : ApiController
    {
        [HttpGet]
        [Route("byText")]
        public IHttpActionResult NuText(int id, string query)
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
                .Query("Records/ByData", new IndexQuery { Query = query });

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

        [HttpGet]
        [Route("byModel")]
        public IHttpActionResult ByModel(int id, string model, string query)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System;
using System.Linq;
using LogIndexer.Analysis.Domain;

namespace LogIndexer.RoslynCompiler.Template {
    public class LinqQuery { public static object Compile(IQueryable<{0}> query) { return {1}; } }
}
"
            .Replace("{0}", model)
            .Replace("{1}", query));

            var assemblyName = Path.GetRandomFileName();
            MetadataReference[] references = {
                MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(Enumerable).Assembly.Location),
                MetadataReference.CreateFromFile(typeof(WebLog).Assembly.Location),
            };

            var compilation = CSharpCompilation.Create(
                assemblyName,
                syntaxTrees: new[] { syntaxTree },
                references: references,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            Assembly assembly;
            using (var ms = new MemoryStream())
            {
                var result = compilation.Emit(ms);

                if (!result.Success)
                {
                    var failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    var errors = failures.Select(diagnostic => new
                    {
                        diagnostic.Id,
                        Message = diagnostic.GetMessage()
                    }).Select(e => new ArgumentException(e.Message));

                    throw new AggregateException(errors);
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    assembly = Assembly.Load(ms.ToArray());
                }
            }

            using (var session = Store.Instance.OpenSession())
            {

                var type = assembly.GetType("LogIndexer.RoslynCompiler.Template.LinqQuery");
                var obj = Activator.CreateInstance(type);
                var result = type.InvokeMember("Compile",
                    BindingFlags.Default | BindingFlags.InvokeMethod,
                    null,
                    obj,
                    new object[] { session.Query<WebLog>() });

                return Ok(result);
            }
        }
    }
}