using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Areas.Projects.Models;
using livemenu.Filters;
using livemenu.Models.Auth;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Controllers
{

    [CustomAuthorize]
    public abstract partial class BaseController : Controller
    {
        protected readonly INotificationBus _notificationBus;

        public BaseController()
        {
            _notificationBus = ServiceLocator.Current.GetInstance<INotificationBus>();
        }

        protected void UpdateWrapper(Action internalSave)
        {
            try
            {
                internalSave();
                _notificationBus.Notificate(new NotificationMessage() {Text = "Изменения успешно сохранены"});
            }
            catch (Exception e)
            {
                _notificationBus.Notificate(
                    new NotificationMessage()
                    {
                        Text = "Изменения не были сохранены. Произошла ошибка : " + e.ToString() + e.StackTrace
                    },
                    NotificationType.Error);
            }

        }

        protected bool OperationWrapper(Func<bool> action, string successMessage = null, string errorMessage = null, bool isPermanent =false)
        {
            successMessage = successMessage ?? "Изменения успешно сохранены";
            errorMessage = errorMessage ?? "Изменения не были сохранены. Произошла ошибка : ";

            try
            {
                var result = action();
                if (result)
                    _notificationBus.Notificate(new NotificationMessage() {Text = successMessage}, isPermanent: isPermanent);
                return result;
            }
            catch (Exception e)
            {
                _notificationBus.Notificate(
                    new NotificationMessage() {Text = errorMessage + e.ToString() + e.StackTrace},
                    NotificationType.Error);

            }

            return false;

        }


    }

    public abstract partial class AppController : BaseController
    {

        protected long? ApplicationID { get; set; }

        public virtual ActionResult DataBase()
        {
            return View(MVC.Projects.DynamicTables.Views.Index, new DynamicTableResponsiveLayoutModel
            {
                CustomID = "projects-dynamictables",
                Name = "База данных",
                InstructionLink = "http://www.WebUni.ru/platform-projects-database",

                ApplicationID = ApplicationID
            });
        }

        public virtual ActionResult Codes()
        {
            return View(MVC.Projects.DynamicColumnCode.Views.Index, new DynamicColumnCodeResponsiveLayoutModel
            {
                CustomID = "projects-columncodes",
                Name = "Коды",
                InstructionLink = "http://www.WebUni.ru/platform-projects-database",

                ApplicationID = ApplicationID
            });
        }
    }

}