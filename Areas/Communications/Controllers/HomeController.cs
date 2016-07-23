using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Models;

namespace livemenu.Areas.Communications.Controllers
{
    public partial class HomeController : CommunicationsController
    {
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Communications.Home.Views.Menu);
        }


        public virtual ActionResult Email()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "communication-notifications-mail",
                OnlyInstruction = true,
                IsIFrame = true,
                InstructionLink = "http://www.WebUni.ru/platform-communication-mail"
            });
        }

        public virtual ActionResult Phone()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "communication-notifications-phone",
                OnlyInstruction = true,
                IsIFrame = true,
                InstructionLink = "http://www.WebUni.ru/platform-communication-phone"
            });
        }

        public virtual ActionResult Chat()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "communication-notifications-chat",
                OnlyInstruction = true,
                IsIFrame = true,
                InstructionLink = "http://www.WebUni.ru/platform-communication-chat"
            });
        }

        public virtual ActionResult Social()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "communication-notifications-social",
                OnlyInstruction = true,
                IsIFrame = true,
                InstructionLink = "http://www.WebUni.ru/platform-communication-social"
            });
        }
    }
}