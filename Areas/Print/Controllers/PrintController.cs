using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Print.Controllers
{
    [LMExecuting(LMExecutingType.Print)]
    public partial class PrintController : AppController
    {
        public PrintController()
        {
            ApplicationID = 34;
        }
	}
}