using BWP.Kernel.Notification;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Fonts;
using livemenu.Models.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.CMS.Controllers;
using livemenu.Models;

namespace livemenu.Controllers
{
    //[CustomAuthorize(AccessMode.Standart, new[]
    //{
    //    SimpleRightValue.UsersViewEdit,
    //    SimpleRightValue.AdminMenuView,
    //})]
    public partial class UserFontsController : CMSController
    {
        private readonly IFontService _fontService;
        private readonly INotificationBus _notificationBus;

        public UserFontsController(IFontService fontService, INotificationBus notificationBus)
        {
            _fontService = fontService;
            _notificationBus = notificationBus;
        }


        public virtual ActionResult Index()
        {
            return View(MVC.UserFonts.Views.Index, new ResponsiveLayoutModel
            {
                CustomID = "cms-fonts",
                Name = "Шрифты",
                InstructionLink = "http://www.WebUni.ru/platform-cms-fonts"
            });
        }

        public virtual ActionResult Internal()
        {
            var model = _fontService.GetAll().ToList();

            return View(MVC.UserFonts.Views.Index, model);
        }

        public virtual ActionResult FontsGridViewPartial()
        {
            return View(_fontService.GetAll());
        }

        public virtual ActionResult Delete(long id)
        {
            _fontService.Delete(id);
            _notificationBus.Notificate(new NotificationMessage() { Text = "Шрифт успешно удален." });

            return View(MVC.UserFonts.Views.FontsGridViewPartial, _fontService.GetAll());
        }

        public virtual ActionResult Edit(long id)
        {
            var font = _fontService.Get(id);
            return View(font);
        }

        public virtual ActionResult Create()
        {
            return View(MVC.UserFonts.Views.Edit, new Font()
            {
                
            });
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult Save(Font font)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var saved = _fontService.CreateOrUpdate(font);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Шрифт успешно сохранен." });
                    return View(MVC.UserFonts.Views.InternalEdit, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Шрифт не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.UserFonts.Views.InternalEdit, font);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Шрифт не был сохранен." }, NotificationType.Warning);
                return View(MVC.UserFonts.Views.InternalEdit, font);
            }

        }
    }
}