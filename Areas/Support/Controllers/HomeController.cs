using System.Web.Mvc;
using livemenu.Areas.Support.Controllers;
using livemenu.Common.Kernel.Settings;
using livemenu.Models.Widgets;

namespace livemenu.Areas.Support.Controllers
{
    public partial class HomeController : SupportController
    {
        private readonly ISettingsService _settingsService;

        public HomeController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Support.Home.Views.Menu);
        }

         
    }
}