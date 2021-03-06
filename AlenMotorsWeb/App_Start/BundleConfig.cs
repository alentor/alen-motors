﻿using System.Web.Optimization;

namespace AlenMotorsWeb {
    public static class BundleConfig {
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/bootstrap-datetimepicker.css", "~/Content/jquery-ui.css" ,"~/Content/main.css"));
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js", "~/Scripts/jquery-ui-{version}.js", "~/Scripts/jquery.validate*"/*, "~/Script/jquery/unobtrusive*"*/));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js", "~/Scripts/respond.js", "~/Scripts/bootstrap-datetimepicker.js", "~/Scripts/bootstrap-select.js"));
            bundles.Add(new ScriptBundle("~/Scripts/moment").Include("~/Scripts/moment.js", "~/Scripts/moment-with-locales.js" ));
            bundles.Add(new ScriptBundle("~/Scripts/main").Include("~/Scripts/main.js"));
        }
    }
}
