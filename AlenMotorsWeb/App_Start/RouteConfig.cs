using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AlenMotorsWeb {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name:"Default",
                            url:"{controller}/{action}/{id}",
                            defaults:new {controller = "Home", action = "Index", id = UrlParameter.Optional});

            routes.MapRoute(name:"Account",
                            url:"Account/{action}/{id}",
                            defaults:new {controller = "Account", action = "Account", id = UrlParameter.Optional});

            //routes.MapRoute(name:"Register",
            //                url:"Account/{action}/{id}",
            //                defaults:new {controller = "Account", action = "Register", id = UrlParameter.Optional});

            //routes.MapRoute(name:"Login",
            //                url:"Account/{action}/{id}",
            //                defaults:new {controller = "Account", action = "Login", id = UrlParameter.Optional});

            routes.MapRoute(name: "EditUser",
                            url: "EditUser/{action}/{id}",
                            defaults: new { controller = "Management", action = "EditUser", id = UrlParameter.Optional });
        }
    }
}