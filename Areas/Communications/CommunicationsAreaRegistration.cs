using System.Web.Mvc;

namespace livemenu.Areas.Mail
{
    public class CommunicationsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Communications";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              "Mail_default_Home",
              "Communications/{controller}/{action}/{id}",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Communications" },
              new[] { "livemenu.Areas.Communications.Controllers" }
          );
            context.MapRoute(
                "Mail_default",
                "Communications/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}