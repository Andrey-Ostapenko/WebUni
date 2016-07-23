using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Notification;
using livemenu.Models;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.Communications.Controllers
{
    public partial class NotificationController : CommunicationsController
    {
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly ISettingsService _settingsService;

        public NotificationController(IEmailNotificationService emailNotificationService, ISettingsService settingsService)
        {
            _emailNotificationService = emailNotificationService;
            _settingsService = settingsService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "communication-notifications",
                Name = "Получатели",
                InstructionLink = "http://www.WebUni.ru/platform-communication-notifications"
            });
        }


        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "EmailNotificationGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";



            settings.Columns.Add(column =>
            {
                column.FieldName = "Email";
                column.Caption = "E-mail";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });



            return settings;
        }



        public virtual ActionResult GridViewPartial()
        {
            return View(_emailNotificationService.GetAll());
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<EmailNotification, int> updateValues)
        {
            foreach (var emailNotification in updateValues.Insert)
            {
                if (updateValues.IsValid(emailNotification))
                    Insert(emailNotification, updateValues);
            }
            foreach (var emailNotification in updateValues.Update)
            {
                if (updateValues.IsValid(emailNotification))
                    Update(emailNotification, updateValues);
            }
            foreach (var id in updateValues.DeleteKeys)
            {
                Delete(id, updateValues);
            }
            return PartialView(MVC.Communications.Notification.Views.GridViewPartial, _emailNotificationService.GetAll());
        }

        protected void Insert(EmailNotification emailNotification, MVCxGridViewBatchUpdateValues<EmailNotification, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _emailNotificationService.Create(emailNotification));
            }
            else
            {
                updateValues.SetErrorText(emailNotification, "Ошибка");
            }
        }
        protected void Update(EmailNotification emailNotification, MVCxGridViewBatchUpdateValues<EmailNotification, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _emailNotificationService.Update(emailNotification));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(emailNotification, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<EmailNotification, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _emailNotificationService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        #region EmailSenderServer

        public virtual ActionResult EmailSenderServer()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "communication-notifications-settings",
                Name = "SMTP сервер отправки E-mail",
                InstructionLink = "http://www.WebUni.ru/platform-communication-notifications-settings"
            });
        }


        public virtual ActionResult EmailSenderServerSettings()
        {
            return View(_settingsService.GetEmailSenderServerSettings());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SaveEmailSenderServerSettings(EmailSenderServerSettings settings)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _settingsService.SaveEmailSenderServerSettings(settings);
                    });

                return View(MVC.Communications.Notification.Views.EmailSenderServerSettingsInternalEdit, _settingsService.GetEmailSenderServerSettings());
            }
            else
            {
                return View(MVC.Communications.Notification.Views.EmailSenderServerSettings, _settingsService);
            }

        }



        #endregion
    }


}