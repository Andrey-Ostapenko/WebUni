using System.Web.Mvc;

namespace livemenu.Areas.Plans
{
    public class PlansAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Plans";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Plans_default_Home",
             "Plans/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Plans" },
             new[] { "livemenu.Areas.Plans.Controllers" }
         );

            context.MapRoute(
                "Plans_default",
                "Plans/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}