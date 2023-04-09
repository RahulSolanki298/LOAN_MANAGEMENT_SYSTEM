using System.Web.Mvc;
using System.Web.Routing;

namespace website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            

            //routes.MapRoute(
            //    name: "Customer",
            //    url: "{Customer}/{Index}/{id}",
            //    defaults: new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
            //);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
