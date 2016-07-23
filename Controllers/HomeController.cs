using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Kernel.Authentication;
using livemenu.Models.Auth;
using livemenu.Models.Home;

namespace livemenu.Controllers
{
    public class LMHomeModel
    {
        public string Version { get; set; }
    }
    [CustomAuthorize]
    public partial class HomeController : LMController
    {
        private readonly IAccountManager _accountManager;


        public HomeController(IAccountManager accountManager)
        {
            _accountManager = accountManager;
        }

        public virtual ActionResult Index()
        {
            var v = System.Reflection.Assembly.GetExecutingAssembly()
                                              .GetName()
                                              .Version
                                              .ToString();
            return View(new LMHomeModel
            {
                Version = v
            });
        }

        public virtual ActionResult Header()
        {
            var account = _accountManager.GetCurrentUser();
            var model = new HeaderModel
            {
                Account = account
            };
            return View(MVC.Shared.Views._Header, model);
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Home.Views.Menu);
        }
    }
}