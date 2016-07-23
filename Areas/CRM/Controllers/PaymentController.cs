using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Common.Kernel.Settings;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class PaymentController : CRMController
    {
        private readonly ISettingsService _settingsService;

        public PaymentController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        #region Robokassa
     
        public virtual ActionResult Robokassa()
        {
            return View();
        }


        public virtual ActionResult RobokassaSettings()
        {
            return View(_settingsService.GetRobokassaSettings());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SaveRobokassaSettings(RobokassaSettings settings)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _settingsService.SaveRobokassaSettings(settings);
                    });

                return View(MVC.CRM.Payment.Views.RobokassaSettingsInternalEdit, _settingsService.GetRobokassaSettings());
            }
            else
            {
                return View(MVC.CRM.Payment.Views.RobokassaSettingsInternalEdit, _settingsService);
            }

        }



        #endregion
    }
}