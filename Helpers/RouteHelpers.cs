using System.Collections.Generic;
using System.Web.Mvc;
using livemenu.Kernel.Executing;

namespace livemenu.Helpers
{
    public static class RouteHelpers
    {
        public static Dictionary<LMExecutingType, ActionResult> DefaultPaths = new Dictionary<LMExecutingType, ActionResult>()
        {
            {LMExecutingType.LM, MVC.Home.Index()},
            {LMExecutingType.CMS, MVC.CMS.Home.Index()},
            {LMExecutingType.CRM, MVC.CRM.Home.Index()},
            {LMExecutingType.Cloud, MVC.Cloud.Home.Index()},
            {LMExecutingType.Team,  MVC.Team.Home.Index()},
            {LMExecutingType.Market,  MVC.Market.Home.Index()},
            {LMExecutingType.Settings,  MVC.Settings.Home.Index()},
            {LMExecutingType.Support,  MVC.Support.Home.Index()},

            {LMExecutingType.Communications,  MVC.Communications.Home.Index()},
            {LMExecutingType.Projects,  MVC.Projects.Home.Index()},
            {LMExecutingType.WMS,  MVC.WMS.Home.Index()},
            {LMExecutingType.Transit,  MVC.Transit.Home.Index()},
            {LMExecutingType.Partner,  MVC.Partner.Home.Index()},

            {LMExecutingType.Actions,  MVC.Actions.Home.Index()},
            {LMExecutingType.News,  MVC.News.Home.Index()},
            {LMExecutingType.Docs,  MVC.Docs.Home.Index()},
            {LMExecutingType.Tasks,  MVC.Tasks.Home.Index()},
            {LMExecutingType.Postcards,  MVC.Postcards.Home.Index()},
            {LMExecutingType.Vacancies,  MVC.Vacancies.Home.Index()},

            {LMExecutingType.Ideas,  MVC.Ideas.Home.Index()},
            {LMExecutingType.Places,  MVC.Places.Home.Index()},
            {LMExecutingType.Lists,  MVC.Lists.Home.Index()},
            {LMExecutingType.Plans,  MVC.Plans.Home.Index()},
            {LMExecutingType.Calendar,  MVC.Calendar.Home.Index()},
            {LMExecutingType.Events,  MVC.Events.Home.Index()},
            {LMExecutingType.Reserve,  MVC.Reserve.Home.Index()},
            {LMExecutingType.Print,  MVC.Print.Home.Index()},
            {LMExecutingType.Research,  MVC.Research.Home.Index()},
            {LMExecutingType.Ads,  MVC.Ads.Home.Index()},

        };
    }
}