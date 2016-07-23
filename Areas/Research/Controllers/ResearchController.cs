using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Research.Controllers
{
    [LMExecuting(LMExecutingType.Research)]
    public partial class ResearchController : AppController
    {
        public ResearchController()
        {
            ApplicationID = 36;
        }
	}
}