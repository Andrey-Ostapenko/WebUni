using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Models;

namespace livemenu.Areas.Transit.Controllers
{
    public partial class HomeController : TransitController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Transit.Home.Views.Menu);
        }


        public virtual ActionResult Module()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "transit-module",
                OnlyInstruction = true,
                IsIFrame = true,
                Name = "Подключить модуль",
                InstructionLink = "http://www.WebUni.ru/platform-transit"
            });
        }
    }
}