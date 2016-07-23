using System.Web.Mvc;

namespace livemenu.Areas.Print
{
    public class PrintAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Print";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Print_default_Home",
             "Print/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Print" },
             new[] { "livemenu.Areas.Print.Controllers" }
         );

            context.MapRoute(
                "Print_default",
                "Print/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}