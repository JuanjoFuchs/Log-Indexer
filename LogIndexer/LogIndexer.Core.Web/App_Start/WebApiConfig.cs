using System.Web.Http;
using Newtonsoft.Json;

namespace LogIndexer.Core.Web
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
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
