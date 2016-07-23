using System.Web.Mvc;

namespace livemenu.Areas.Research
{
    public class ResearchAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Research";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
             "Research_default_Home",
             "Research/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Research" },
             new[] { "livemenu.Areas.Research.Controllers" }
         );

            context.MapRoute(
                "Research_default",
                "Research/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}