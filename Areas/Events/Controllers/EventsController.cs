using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Events.Controllers
{
    [LMExecuting(LMExecutingType.Events)]
    public partial class EventsController : AppController
    {
        public EventsController()
        {
            ApplicationID = 32;
        }
	}
}