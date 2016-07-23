using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.BWP.Pages.WebCounters;
using livemenu.Models;
using Unit = System.Web.UI.WebControls.Unit;


namespace livemenu.Areas.CMS.Controllers
{
    public partial class WebCountersController : CMSController
    {
        private readonly IWebCounterService _webCounterService;

        public WebCountersController(IWebCounterService webCounterService)
        {
            _webCounterService = webCounterService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-statistics",
                Name = "Счетчики",
                InstructionLink = "http://www.WebUni.ru/platform-cms-statistics"
            });
        }

        public virtual ActionResult WebCountersGridViewPartial()
        {
            return View();
        }

        public GridViewSettings GetWebCountersGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "WebCountersGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";


            settings.Columns.Add(column =>
            {
                column.FieldName = "IsEnabled";
                column.Caption = "Активность";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                column.Width = new Unit(50, UnitType.Pixel);
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


            return settings;
        }

        public virtual ActionResult Delete(long id)
        {
            try
            {
                _webCounterService.Delete(id);
                _notificationBus.Notificate(new NotificationMessage() { Text = "Счетчик успешно удален" });
            }
            catch (Exception e)
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Счетчик не был удален. " + e.Message }, NotificationType.Warning);
            }
            return null;
        }

        public virtual ActionResult WebCountersCard(long? id)
        {
            if (id.HasValue)
            {
                var item = _webCounterService.Get(id.Value);
                return View(MVC.CMS.WebCounters.Views.WebCountersCardLayout, item);
            }
            else
            {
                var item = _webCounterService.PrepareNew();
                return View(MVC.CMS.WebCounters.Views.WebCountersCardLayout, item);
            }
        }
        [ValidateInput(false)]
        public virtual ActionResult WebCountersCardSave(WebCounter item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var saved = _webCounterService.CreateOrUpdate(item);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Изменения успешно сохранены" });
                    ModelState.Clear();
                    return View(MVC.CMS.WebCounters.Views.WebCountersCard, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Изменения успешно сохранены. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CMS.WebCounters.Views.WebCountersCard, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен." }, NotificationType.Warning);
                return View(MVC.CMS.WebCounters.Views.WebCountersCard, item);
            }
        }
    }
}