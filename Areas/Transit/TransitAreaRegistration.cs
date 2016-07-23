using System.Web.Mvc;

namespace livemenu.Areas.Transit
{
    public class TransitAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Transit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "Transit_default_Home",
             "Transit/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Transit" },
             new[] { "livemenu.Areas.Transit.Controllers" }
         );
            context.MapRoute(
                "Transit_default",
                "Transit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}