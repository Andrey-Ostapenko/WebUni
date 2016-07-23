using System.Web.Mvc;

namespace livemenu.Areas.Support
{
    public class SupportAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Support";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
               "Support_default_Home",
               "Support/{controller}/{action}/{id}",
               new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Support" },
               new[] { "livemenu.Areas.Support.Controllers" }
           );
            context.MapRoute(
                "Support_default",
                "Support/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}