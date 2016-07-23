using System.Web.Mvc;

namespace livemenu.Areas.CRM
{
    public class CRMAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CRM";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "CRM_default_Home",
             "CRM/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "CRM" },
             new[] { "livemenu.Areas.CRM.Controllers" }
         );
            context.MapRoute(
                "CRM_default",
                "CRM/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CRM.Controllers" }
            );
        }
    }
}