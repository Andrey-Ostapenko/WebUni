using System.Web.Mvc;

namespace livemenu.Areas.Projects
{
    public class BlogAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Projects";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "Blog_default_Home",
             "Projects/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Projects" },
             new[] { "livemenu.Areas.Projects.Controllers" }
         );
            context.MapRoute(
                "Blog_default",
                "Projects/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}