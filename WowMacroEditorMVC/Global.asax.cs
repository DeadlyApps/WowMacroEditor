using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WowMacroEditorMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Sitemap",
                "sitemap.xml",
                new { controller = "Home", action = "Sitemap" }
                );

            routes.MapRoute(
                "Browse", // Route name
                "Macros/Browse/{Tag}", // URL with parameters
                new { controller = "Macros", action = "Browse", id = UrlParameter.Optional, Tag = UrlParameter.Optional } // Parameter defaults  
                );

            routes.MapRoute(
                "UserRoute", // Route name
                "User/{userIntID}/{profileName}", // URL with parameters
                new { controller = "Profile", action = "Index", userIntID = 0, profileName = "New-User" } // Parameter defaults
                );

            routes.MapRoute(
                "MacroDetails", // Route name
                "{controller}/{action}/{id}/{MacroString}", // URL with parameters
                new { controller = "Macros", action = "Index", MacroString = UrlParameter.Optional } // Parameter defaults
                );

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Macros", action = "Browse", id = UrlParameter.Optional } // Parameter defaults
                );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}