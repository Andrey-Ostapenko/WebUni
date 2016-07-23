using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Areas.Team.Controllers;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Management;
using livemenu.Kernel.Users;
using livemenu.Models;
using livemenu.Models.Auth;
using livemenu.Models.Widgets;

namespace livemenu.Controllers
{
    [CustomAuthorize(AccessMode.Standart, new[]
    {
        SimpleRightValue.UsersViewEdit,
        SimpleRightValue.AdminMenuView,
    })]
    public partial class UsersController : TeamController
    {
        private readonly IUserService _userService;
        private readonly ILicenseInfoService _licenseInfoService;
        private readonly INotificationBus _notificationBus;

        public UsersController(IUserService userService, ILicenseInfoService licenseInfoService, INotificationBus notificationBus)
        {
            _userService = userService;
            _licenseInfoService = licenseInfoService;
            _notificationBus = notificationBus;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "team-users",
                Name = "Персонал",
                InstructionLink = "http://www.WebUni.ru/platform-team-users"
            });
          
        }

        public virtual ActionResult Internal()
        {
            var model = _userService.GetAll().ToList();

            return View(MVC.Users.Views.Index, model);
        }

        public virtual ActionResult UsersGridViewPartial()
        {
            return View(_userService.GetAll());
        }

        public virtual ActionResult Edit(long id)
        {
            var user = _userService.Get(id);
            user.NewLogin = user.Login;
            return View(user);
        }

        [HttpPost]
        public virtual ActionResult Save(User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var saved = _userService.CreateOrUpdate(user);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Пользователь успешно сохранен." });
                    saved.NewLogin = saved.Login;
                    return View(MVC.Users.Views.InternalEdit, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Пользователь не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.Users.Views.InternalEdit, user);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Пользователь не был сохранен." }, NotificationType.Warning);
                return View(MVC.Users.Views.InternalEdit, user);
            }

        }

        public virtual ActionResult Delete(long id)
        {
            _userService.Delete(id);
            _notificationBus.Notificate(new NotificationMessage() { Text = "Пользователь успешно удален." });

            return View(MVC.Users.Views.UsersGridViewPartial, _userService.GetAll());
        }

        public virtual ActionResult Create()
        {
            var maxCount = _licenseInfoService.GetLicense().MaxUserCount;
            if (maxCount.HasValue && _userService.GetCount() + 1 >= maxCount.Value)
            {
                return View(MVC.Users.Views.NoAccess, new User()
                {
                    RightSubject = new RightSubject()
                });

            }
            else
            {
                return View(MVC.Users.Views.Edit, new User()
                {
                    RightSubject = new RightSubject()
                });
            }
            
        }

    }
}