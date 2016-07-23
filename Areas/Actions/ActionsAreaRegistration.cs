using System.Web.Mvc;

namespace livemenu.Areas.Actions
{
    public class ActionsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Actions";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Actions_default_Home",
             "Actions/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Actions" },
             new[] { "livemenu.Areas.Actions.Controllers" }
         );

            context.MapRoute(
                "Actions_default",
                "Actions/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}