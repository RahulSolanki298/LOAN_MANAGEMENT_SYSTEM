using System.Web.Mvc;
using System.Web.Routing;

namespace website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               "Customer", "customer", new { controller = "Customer", action = "Index", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               "ActivatedCustomerList", "activated-customers", new { controller = "Customer", action = "ActivatedCustomerList", id = UrlParameter.Optional }
           );

            routes.MapRoute(
               "Employee", "employee", new { controller = "Employee", action = "Index", id = UrlParameter.Optional }
           );


            routes.MapRoute(
                "Dashboard", "home", new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Account", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
