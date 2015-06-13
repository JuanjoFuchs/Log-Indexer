using System.Web.Optimization;

namespace LogIndexer.Analysis.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/generated.js"));

            bundles.Add(new ScriptBundle("~/bundles/vendor").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/respond.js",
                "~/Scripts/q.js",
                "~/Scripts/angular.js",
                "~/Scripts/angular-animate.js",
                "~/Scripts/angular-aria.js",
                 "~/Scripts/angular-material.js",
                "~/Scripts/angular-route.js",
                "~/Scripts/angular-resource.js",
                "~/Scripts/json-formatter.js"
                //"~/Scripts/rx.js",
                //"~/Scripts/rx.lite.js",
                //"~/Scripts/rx.aggregates.js",
                //"~/Scripts/rx.async.js",
                //"~/Scripts/rx.angular.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                 "~/Content/angular-material/angular-material.css",
                 "~/Content/json-formatter.css",
                "~/Content/site.css"));
        }
    }
}
