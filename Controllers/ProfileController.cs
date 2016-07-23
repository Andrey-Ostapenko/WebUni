using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Models;

namespace livemenu.Controllers
{
    public partial class ProfileController : LMController
    {
        // GET: Profile
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Account()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "profile-account",
                IsIFrame = true,
                Name = "Аккаунт",
                InstructionLink = "http://www.WebUni.ru/platform-profile-settings"
            });
        }

        public virtual ActionResult License()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "profile-license",
                IsIFrame = true,
                Name = "Лицензия",
                InstructionLink = "http://www.WebUni.ru/platform-profile-license"
            });
        }

        public virtual ActionResult Security()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "profile-security",
                IsIFrame = true,
                Name = "Безопасность",
                InstructionLink = "http://www.WebUni.ru/platform-profile-security"
            });
        }

        public virtual ActionResult Purchases()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "profile-purchases",
                IsIFrame = true,
                Name = "Покупки",
                InstructionLink = "http://www.WebUni.ru/platform-profile-purchases"
            });
        }

        public virtual ActionResult Password()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "profile-password",
                IsIFrame = true,
                Name = "Пароль",
                InstructionLink = "http://www.WebUni.ru/platform-profile-password"
            });
        }
    }
}