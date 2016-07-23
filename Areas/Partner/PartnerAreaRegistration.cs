using System.Web.Mvc;

namespace livemenu.Areas.Partner
{
    public class PartnerAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Partner";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "Partner_default_Home",
             "Partner/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Partner" },
             new[] { "livemenu.Areas.Partner.Controllers" }
         );
            context.MapRoute(
                "Partner_default",
                "Partner/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}