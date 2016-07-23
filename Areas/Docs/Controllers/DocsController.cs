using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Docs.Controllers
{
    [LMExecuting(LMExecutingType.Docs)]
    public partial class DocsController : AppController
    {
        public DocsController()
        {
            ApplicationID = 20;
        }
	}
}