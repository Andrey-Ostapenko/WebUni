using System.Collections.Generic;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using DevExpress.Data.Linq;

using livemenu.Common;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Settings;
using livemenu.Models;
using livemenu.Models.Widgets;

namespace livemenu.Areas.Settings.Controllers
{
    public partial class HomeController : SettingsController
    {
        private readonly ISettingsService _settingsService;
        private readonly IApplicationService _applicationService;

        public HomeController(ISettingsService settingsService, IApplicationService applicationService)
        {
            _settingsService = settingsService;
            _applicationService = applicationService;
        }

        public virtual ActionResult Index()
        {
            return RedirectToAction(MVC.Settings.Version.Index());
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Settings.Home.Views.Menu);
        }


        public virtual ActionResult LoginPage()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-settings-login",
                Name = "Страница входа",
                InstructionLink = "http://www.WebUni.ru/platform-settings"
            });

        }

        public virtual ActionResult Applications()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-settings-applications",
                Name = "Приложения",
                InstructionLink = "http://www.WebUni.ru/platform-settings-applications"
            });

        }

        [HttpPost]
        public virtual JsonResult ApplicationStateChanged(ApplicationStateModel model)
        {
            _applicationService.ChangeState(model);
            _notificationBus.Notificate(new NotificationMessage() { Text = "Изменения сохранены" });
            return Json(true);
        }


        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult LoginPageSave(ApplicationSettings applicationSettings)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _settingsService.SaveApplicationSettings(applicationSettings);
                    });

                return View(MVC.Settings.Home.Views.LoginPageInternalEdit, _settingsService.GetApplicationSettings());
            }
            else
            {
                return View(MVC.Settings.Home.Views.LoginPageInternalEdit, _settingsService);
            }

        }
      
        public virtual ActionResult Internal()
        {
            return View(MVC.Settings.Home.Views.LoginPageInternal, _settingsService.GetApplicationSettings());
        }



        #region catalogsettings

        public virtual ActionResult Catalog()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-settings-site",
                Name = "настройки сайта",
                InstructionLink = "http://www.WebUni.ru/platform-settings"
            });
        }


        public virtual ActionResult CatalogSettings()
        {
            return View(_settingsService.GetCatalogSettings());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SaveCatalogSettings(CatalogSettings settings)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _settingsService.SaveCatalogSettings(settings);
                    });

                return View(MVC.Settings.Home.Views.CatalogSettingsInternalEdit, _settingsService.GetCatalogSettings());
            }
            else
            {
                return View(MVC.Settings.Home.Views.CatalogSettingsInternalEdit, _settingsService);
            }

        }



        #endregion

    }
}