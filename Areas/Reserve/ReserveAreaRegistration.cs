using System.Web.Mvc;

namespace livemenu.Areas.Reserve
{
    public class ReserveAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Reserve";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Reserve_default_Home",
             "Reserve/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Reserve" },
             new[] { "livemenu.Areas.Reserve.Controllers" }
         );

            context.MapRoute(
                "Reserve_default",
                "Reserve/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}