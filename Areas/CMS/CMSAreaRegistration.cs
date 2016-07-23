using System.Web.Mvc;

namespace livemenu.Areas.CMS
{
    public class CMSAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CMS";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

       //     context.Routes.IgnoreRoute("BWP/{*pathinfo}");
          

            //context.MapRoute(
            //    "BWP_default_Home",
            //    "BWP/{controller}/{action}/{id}",
            //    new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "BWP" },
            //    new[] { "livemenu.Areas.BWP.Controllers" }
            //);

            context.MapRoute(
                "CMS_Catalog_default",
                "CMS/Catalog/Index/{code}",
                new { controller="Catalog", action = "Index", code = UrlParameter.Optional },
                new[] { "livemenu.Areas.CMS.Controllers" }
            );

            context.MapRoute(
                "CMS_default",
                "CMS/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.CMS.Controllers" }
            );
        }
    }
}