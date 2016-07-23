using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Calendar.Controllers
{
    [LMExecuting(LMExecutingType.Calendar)]
    public partial class CalendarController : AppController
    {
        public CalendarController()
        {
            ApplicationID = 30;
        }
	}
}