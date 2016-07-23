using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Plans.Controllers
{
    [LMExecuting(LMExecutingType.Plans)]
    public partial class PlansController : AppController
    {
        public PlansController()
        {
            ApplicationID = 24;
        }
	}
}