using System.Web.Mvc;

namespace livemenu.Areas.Vacancies
{
    public class VacanciesAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Vacancies";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
           "Vacancies_default_Home",
           "Vacancies/{controller}/{action}/{id}",
           new { controller = "Home", action = "Index", id = UrlParameter.Optional, area = "Vacancies" },
           new[] { "livemenu.Areas.Vacancies.Controllers" }
       );

            context.MapRoute(
                "Vacancies_default",
                "Vacancies/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                    new[] { "livemenu.Areas.Vacancies.Controllers" }
            );
        }
    }
}