using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Models;

namespace livemenu.Areas.Cloud.Controllers
{
    public partial class HomeController : CloudController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Cloud.Home.Views.Menu);
        }

        public virtual ActionResult ControlPanel()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cloud-WebUni",
                WithoutInstruction = true,
                IsIFrame = true,
                Name = "Платформа WebUni",
                InstructionLink = "http://www.WebUni.ru/platform-cloud"
            });
        }

        public virtual ActionResult Server()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cloud-server",
                WithoutInstruction = true,
                IsIFrame = true,
                Name = "Сервер",
                InstructionLink = "http://www.WebUni.ru/platform-cloud"
            });
        }

        public virtual ActionResult Backup()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cloud-backup",
                WithoutInstruction = true,
                IsIFrame = true,
                Name = "Архив",
                InstructionLink = "http://www.WebUni.ru/platform-cloud"
            });
        }
    }
}