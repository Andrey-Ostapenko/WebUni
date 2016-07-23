using System.Web.Mvc;

namespace livemenu.Areas.Settings
{
    public class SettingsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Settings";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Settings_default_Home",
                "Settings/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Settings" },
                new[] { "livemenu.Areas.Settings.Controllers" }
            );
            context.MapRoute(
                "Settings_default",
                "Settings/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}