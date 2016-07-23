using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Common.Kernel.Settings;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class SettingsController : CRMController
    {
        private readonly ISettingsService _settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            _settingsService = settingsService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

    
    }
}