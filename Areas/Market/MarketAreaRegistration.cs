using System.Web.Mvc;

namespace livemenu.Areas.Market
{
    public class MarketAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Market";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Market_default_Home",
                "Market/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Market" },
                new[] { "livemenu.Areas.Market.Controllers" }
            );
            context.MapRoute(
                "Market_default",
                "Market/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}