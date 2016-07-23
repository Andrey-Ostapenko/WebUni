using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class HomeController : CRMController
    {
        //
        // GET: /CRM/Home/
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.CRM.Home.Views.Menu);
        }
	}
}