using System.Web.Mvc;

namespace livemenu.Areas.Ideas
{
    public class IdeasAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Ideas";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Ideas_default_Home",
             "Ideas/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Ideas" },
             new[] { "livemenu.Areas.Ideas.Controllers" }
         );

            context.MapRoute(
                "Ideas_default",
                "Ideas/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}