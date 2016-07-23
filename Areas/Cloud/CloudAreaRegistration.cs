using System.Web.Mvc;

namespace livemenu.Areas.Cloud
{
    public class CloudAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Cloud";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
              "Cloud_default_Home",
              "Cloud/{controller}/{action}/{id}",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Cloud" },
              new[] { "livemenu.Areas.Cloud.Controllers" }
          );
            context.MapRoute(
                "Cloud_default",
                "Cloud/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.Cloud.Controllers" }
            );
        }
    }
}