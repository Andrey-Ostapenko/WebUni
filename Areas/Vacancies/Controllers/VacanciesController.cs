using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Vacancies.Controllers
{
    [LMExecuting(LMExecutingType.Vacancies)]
    public partial class VacanciesController : AppController
    {
        public VacanciesController()
        {
            ApplicationID = 18;
        }
	}
}