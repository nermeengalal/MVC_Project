using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Day7
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //routes.MapRoute(
            //        name: "Default1",
            //        url: "{controller}/{action}/{name}",
            //        defaults: new { controller = "Profile", action = "GetUser", name = UrlParameter.Optional }
            //    );
            //routes.MapRoute(
            //        name: "Default2",
            //        url: "{controller}/{action}/{id}/{Quantity}",
            //        defaults: new { controller = "Home", action = "card", id = UrlParameter.Optional , Quantity = UrlParameter.Optional }
            //    );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
 
        }
    }
}
