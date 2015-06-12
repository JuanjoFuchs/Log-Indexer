using System.Web.Mvc;
using System.Web.Routing;

namespace LogIndexer.Analysis.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "App",
                url: "",
                defaults: new { controller = "App", action = "Index" });
        }
    }
}
