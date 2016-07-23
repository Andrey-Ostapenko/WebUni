using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Lists.Controllers
{
    [LMExecuting(LMExecutingType.Lists)]
    public partial class ListsController : AppController
    {
        public ListsController()
        {
            ApplicationID = 26;
        }
	}
}