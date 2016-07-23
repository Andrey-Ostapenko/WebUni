using System.Web.Mvc;
using livemenu.Areas.Support.Controllers;
using livemenu.Common.Kernel.Settings;
using livemenu.Models.Widgets;

namespace livemenu.Areas.WMS.Controllers
{
    public partial class HomeController : WMSController
    {

        public HomeController()
        {
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.WMS.Home.Views.Menu);
        }

         
    }
}