using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Postcards.Controllers
{
    [LMExecuting(LMExecutingType.Postcards)]
    public partial class PostcardsController : AppController
    {
        public PostcardsController()
        {
            ApplicationID = 22;
        }
	}
}