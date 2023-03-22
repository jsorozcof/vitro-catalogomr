using System.Web;
using System.Web.Optimization;

namespace Vitro
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));
            bundles.Add(new ScriptBundle("~/bundles/metro").Include("~/Scripts/metro.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include("~/Scripts/angular.min.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/metro-all.min.css","~/Content/metro-icons.min.css","~/Content/site.css"));
        }
    }
}
