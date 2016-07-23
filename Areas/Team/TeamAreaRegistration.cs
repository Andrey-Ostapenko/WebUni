using System.Web.Mvc;

namespace livemenu.Areas.Team
{
    public class TeamAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Team";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
              "Team_default_Home",
              "Team/{controller}/{action}/{id}",
              new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Team" },
              new[] { "livemenu.Areas.Team.Controllers" }
          );
            context.MapRoute(
                "Team_default",
                "Team/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}