using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace livemenu
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{resource}.ashx/{*pathInfo}");

           

            routes.MapRoute(
              name: "FileManager_Default",
              url: "FM/Editor",
              defaults: new { controller = "FileManager", action = "Editor" },
              namespaces: new[] { "livemenu.Controllers" }
          );
            routes.MapRoute(
              name: "FileManager_Thumbs",
              url: "FileManager/Thumbs/{tmb}",
              defaults: new { controller = "FileManager", action = "Thumbs", tmb = UrlParameter.Optional },
              namespaces: new[] { "livemenu.Controllers" }
          );

            routes.MapRoute(
                name: "FileManager_Main",
                url: "FM/{et}",
                defaults: new { controller = "FileManager", action = "Index", et = UrlParameter.Optional },
                namespaces: new[] { "livemenu.Controllers" }
            );



           
            routes.MapRoute(
                name: "Login_Default",
                url: "Login/Index/{returnUrl}",
                defaults: new { controller = "Login", action = "Index", returnUrl = UrlParameter.Optional },
                namespaces: new[] { "livemenu.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "livemenu.Controllers" }
            );
        }
    }
}
