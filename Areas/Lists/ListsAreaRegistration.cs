using System.Web.Mvc;

namespace livemenu.Areas.Lists
{
    public class ListsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Lists";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Lists_default_Home",
             "Lists/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Lists" },
             new[] { "livemenu.Areas.Lists.Controllers" }
         );

            context.MapRoute(
                "Lists_default",
                "Lists/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}