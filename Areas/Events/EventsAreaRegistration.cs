using System.Web.Mvc;

namespace livemenu.Areas.Events
{
    public class EventsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Events";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Events_default_Home",
             "Events/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Events" },
             new[] { "livemenu.Areas.Events.Controllers" }
         );

            context.MapRoute(
                "Events_default",
                "Events/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}