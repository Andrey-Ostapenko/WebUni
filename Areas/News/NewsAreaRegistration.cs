using System.Web.Mvc;

namespace livemenu.Areas.News
{
    public class NewsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "News";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
             "News_default_Home",
             "News/{controller}/{action}/{id}",
             new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "News" },
             new[] { "livemenu.Areas.News.Controllers" }
         );

            context.MapRoute(
                "News_default",
                "News/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new[] { "livemenu.Areas.News.Controllers" }
            );
        }
    }
}