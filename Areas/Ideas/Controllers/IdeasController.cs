using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Ideas.Controllers
{
    [LMExecuting(LMExecutingType.Ideas)]
    public partial class IdeasController : AppController
    {
        public IdeasController()
        {
            ApplicationID = 23;
        }
	}
}