using System.Web.Mvc;

namespace livemenu.Areas.Ads
{
    public class AdsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ads";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Ads_default_Home",
             "Ads/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Ads" },
             new[] { "livemenu.Areas.Ads.Controllers" }
         );

            context.MapRoute(
                "Ads_default",
                "Ads/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}