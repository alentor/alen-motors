using System.Web.Optimization;

namespace AlenMotorsWeb {
    public static class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/main.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/scripts/layoutJS").Include("~/Scripts/bootstrap.js", "~/Scripts/main.js"));
        }
    }
}