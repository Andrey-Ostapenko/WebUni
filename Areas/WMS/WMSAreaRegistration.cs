using System.Web.Mvc;

namespace livemenu.Areas.WMS
{
    public class WMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "WMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              "WMS_default_Home",
              "WMS/{controller}/{action}/{id}",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "WMS" },
              new[] { "livemenu.Areas.WMS.Controllers" }
            );

            context.MapRoute(
                "WMS_default",
                "WMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}