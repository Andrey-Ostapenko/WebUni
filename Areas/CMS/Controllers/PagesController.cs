using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Areas.CMS.Models;
using livemenu.Kernel.BWP.Pages;
using livemenu.Common.Kernel.CatalogItemCache;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.CMS;
using livemenu.Models;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class PagesController : CMSController
    {
        private readonly IPageService _pageService;
        private readonly ICatalogItemCacheService _catalogItemCacheService;
        private readonly ISettingsService _settingsService;

        public PagesController(IPageService pageService, ICatalogItemCacheService catalogItemCacheService, ISettingsService settingsService)
        {
            _pageService = pageService;
            _catalogItemCacheService = catalogItemCacheService;
            _settingsService = settingsService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-speed",
                Name = "Страницы",
                InstructionLink = "http://www.WebUni.ru/platform-cms-speed"
            });
        }

        public virtual ActionResult Internal()
        {
            return View(new PagesModel
            {
                ClearCacheOnSave = _settingsService.GetCatalogSettings().ClearCacheOnSave
            });
        }


        [HttpPost]
        public virtual JsonResult ClearCacheOnSaveChanged(bool value)
        {
            OperationWrapper(() =>
            {
                _settingsService.SaveCatalogSettings(new CatalogSettings
                {
                    ClearCacheOnSave = value
                });

                return true;
            }, "Изменения успешно сохранены");
            return new JsonResult();
        }

        public virtual ActionResult PagesCachingGridViewPartial()
        {
            return View();
        }



        public virtual JsonResult ClearAllCache()
        {
            OperationWrapper(() =>
            {
                _pageService.ClearAllCache();
                _catalogItemCacheService.ClearAllCache();
                return true;
            },"Кэш успешно очищен");
            
            return Json(true);
        }

        public virtual JsonResult ClearPageCache(long id, string fullCode)
        {
            OperationWrapper(() =>
            {
                _pageService.ClearPageCache(id, fullCode);
                return true;
            }, "Кэш успешно очищен");

           return Json(true);
        }

        public virtual JsonResult RegenerateAbsent()
        {
            OperationWrapper(() =>
            {
                _pageService.RegenerateAbsent((mes) =>
                {
                    _notificationBus.Notificate(new NotificationMessage
                    {
                        Text = mes
                    });
                });
                return true;
            }, "Генерация завершена");

            return Json(true);
        }

        public virtual JsonResult RegenerateAll()
        {
            OperationWrapper(() =>
            {
                _pageService.RegenerateAll((mes) =>
                {
                    _notificationBus.Notificate(new NotificationMessage
                    {
                        Text =mes
                    });
                });
                return true;
            }, "Генерация завершена");
            
            return Json(true);
        }

        public virtual JsonResult RegeneratePage(long id, string fullCode)
        {
            OperationWrapper(() =>
            {
                _pageService.RegeneratePage(id, fullCode);
                return true;
            }, "Генерация завершена");
            
            return Json(true);
        }

        
	}
}