using System.Web.Mvc;

namespace livemenu.Areas.Docs
{
    public class DocsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Docs";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "Docs_default_Home",
             "Docs/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Docs" },
             new[] { "livemenu.Areas.Docs.Controllers" }
         );

            context.MapRoute(
              "Docs_default",
              "Docs/{controller}/{action}/{id}",
              new { action = "Index", id = UrlParameter.Optional },
              new[] { "livemenu.Areas.Docs.Controllers" }
          );
        }
    }
}