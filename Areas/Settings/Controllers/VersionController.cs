using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.Settings.Models.Version;
using livemenu.Controllers;
using livemenu.Kernel.Configuration;

namespace livemenu.Areas.Settings.Controllers
{

    public partial class VersionController : SettingsController
    {
        private readonly IVersionService _versionService;

        public VersionController(IVersionService versionService)
        {
            _versionService = versionService;
        }

        public virtual ActionResult Index()
        {
            var v = _versionService.GetVersion();

            return View(new VersionModel
            {
                Version = v
            });
        }
    }
}