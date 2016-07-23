using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Actions.Controllers
{
    [LMExecuting(LMExecutingType.Actions)]
    public partial class ActionsController : AppController
    {
        public ActionsController()
        {
            ApplicationID = 17;
        }
	}
}