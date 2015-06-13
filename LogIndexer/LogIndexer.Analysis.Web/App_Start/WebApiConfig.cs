using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using LogIndexer.Core.Domain;
using LogIndexer.Processor.Data.Indexes;
using Microsoft.OData.Edm;
using Newtonsoft.Json;

namespace LogIndexer.Analysis.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var jsonSettings = JsonConvert.DefaultSettings();
            config.Formatters.JsonFormatter.SerializerSettings = jsonSettings;
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.MapODataServiceRoute("oData", "odata", GetEdmModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
            config.EnsureInitialized();
        }

        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder {Namespace = typeof (Log).Namespace};
            builder.EntitySet<Log>("logs");
            builder.EntitySet<Logs_Full.Result>("logs_full");
            builder.EntitySet<Application>("applications");
            builder.EntitySet<Environment>("environments");
            builder.EntitySet<DataSource>("dataSources");
            builder.EnableLowerCamelCase();
            var edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
