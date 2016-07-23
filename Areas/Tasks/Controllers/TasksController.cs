using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Tasks.Controllers
{
    [LMExecuting(LMExecutingType.Tasks)]
    public partial class TasksController : AppController
    {
        public TasksController()
        {
            ApplicationID = 19;
        }
	}
}