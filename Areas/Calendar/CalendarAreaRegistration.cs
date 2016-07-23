using System.Web.Mvc;

namespace livemenu.Areas.Calendar
{
    public class CalendarAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Calendar";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Calendar_default_Home",
             "Calendar/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Calendar" },
             new[] { "livemenu.Areas.Calendar.Controllers" }
         );

            context.MapRoute(
                "Calendar_default",
                "Calendar/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}