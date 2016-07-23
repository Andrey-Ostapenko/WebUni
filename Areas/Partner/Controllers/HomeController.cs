using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Models;

namespace livemenu.Areas.Partner.Controllers
{
    public partial class HomeController : PartnerController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Partner.Home.Views.Menu);
        }

        public virtual ActionResult List()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "partners-list",
                OnlyInstruction = true,
                IsIFrame = true,
                Name = "Управление",
                InstructionLink = "http://www.WebUni.ru/platform-partners"
            });
        }

        public virtual ActionResult Add()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "transit-add",
                OnlyInstruction = true,
                IsIFrame = true,
                Name = "Стать партнером",
                InstructionLink = "http://www.WebUni.ru/platform-partners"
            });
        }

        public virtual ActionResult Search()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "transit-search",
                OnlyInstruction = true,
                IsIFrame = true,
                Name = "Найти партнеров",
                InstructionLink = "http://www.WebUni.ru/platform-partners"
            });
        }
    }
}