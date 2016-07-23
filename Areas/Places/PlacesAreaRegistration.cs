using System.Web.Mvc;

namespace livemenu.Areas.Places
{
    public class PlacesAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Places";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Places_default_Home",
             "Places/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Places" },
             new[] { "livemenu.Areas.Places.Controllers" }
         );

            context.MapRoute(
                "Places_default",
                "Places/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}