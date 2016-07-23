using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;

namespace livemenu.Areas.Ads.Controllers
{
    [LMExecuting(LMExecutingType.Ads)]
    public partial class AdsController : AppController
    {
        public AdsController()
        {
            ApplicationID = 37;
        }
	}
}