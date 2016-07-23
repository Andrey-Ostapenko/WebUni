using System.Web.Mvc;

namespace livemenu.Areas.Postcards
{
    public class PostcardsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Postcards";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
         "Postcards_default_Home",
         "Postcards/{controller}/{action}/{id}",
         new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Postcards" },
         new[] { "livemenu.Areas.Postcards.Controllers" }
     );

            context.MapRoute(
                "Postcards_default",
                "Postcards/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                    new[] { "livemenu.Areas.News.Controllers" }
            );
        }
    }
}