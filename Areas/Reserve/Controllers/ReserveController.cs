using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Reserve.Controllers
{
    [LMExecuting(LMExecutingType.Reserve)]
    public partial class ReserveController : AppController
    {
        public ReserveController()
        {
            ApplicationID = 33;
        }
	}
}