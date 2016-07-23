using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using livemenu.Kernel.Authentication;
using livemenu.Kernel.Executing;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Controllers
{
    public partial class CommonController : BaseController
    {
        private readonly ILMExecutingContext _executingContext;

        public CommonController(ILMExecutingContext executingContext)
        {
            _executingContext = executingContext;
        }

        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult Menu()
        {
            // Разное меню в зависимости от режима
            var type = _executingContext.ExecutingType;

            switch (type)
            {
                case LMExecutingType.CMS:
                {
                    return ServiceLocator.Current.GetInstance<Areas.CMS.Controllers.HomeController>().Menu();
                    break;
                }
                case LMExecutingType.CRM:
                {
                    return ServiceLocator.Current.GetInstance<Areas.CRM.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Cloud:
                {
                    return  ServiceLocator.Current.GetInstance<Areas.Cloud.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.LM:
                {
                    return ServiceLocator.Current.GetInstance<HomeController>().Menu();
                }
                case LMExecutingType.Team:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Team.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Market:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Market.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Settings:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Settings.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Support:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Support.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Communications:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Communications.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Projects:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Projects.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.WMS:
                {
                    return ServiceLocator.Current.GetInstance<Areas.WMS.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Transit:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Transit.Controllers.HomeController>().Menu();
                }
                case LMExecutingType.Partner:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Partner.Controllers.HomeController>().Menu();
                }

                case LMExecutingType.Actions:
                {
                    return ServiceLocator.Current.GetInstance<Areas.Actions.Controllers.HomeController>().Menu();
                }

                case LMExecutingType.Vacancies:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Vacancies.Controllers.HomeController>().Menu();
                    }

                case LMExecutingType.Tasks:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Tasks.Controllers.HomeController>().Menu();
                    }

                case LMExecutingType.Postcards:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Postcards.Controllers.HomeController>().Menu();
                    }

                case LMExecutingType.Docs:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Docs.Controllers.HomeController>().Menu();
                    }

                case LMExecutingType.Ideas:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Ideas.Controllers.HomeController>().Menu();
                    }

                case LMExecutingType.Plans:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Plans.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Lists:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Lists.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Places:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Places.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Calendar:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Calendar.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Events:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Events.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Reserve:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Reserve.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Print:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Print.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Research:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Research.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.Ads:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.Ads.Controllers.HomeController>().Menu();
                    }
                case LMExecutingType.News:
                    {
                        return ServiceLocator.Current.GetInstance<Areas.News.Controllers.HomeController>().Menu();
                    }
            }
            return null;
        }
	}
}