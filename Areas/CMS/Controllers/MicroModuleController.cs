using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models;
using livemenu.Controllers;
using livemenu.Kernel.MicroModules;
using livemenu.Models.Widgets;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class MicroModuleController : CMSController
    {
        private readonly IMicroModuleFactory _microModuleFactory;

        public MicroModuleController(IMicroModuleFactory microModuleFactory)
        {
            this._microModuleFactory = microModuleFactory;
        }

        public virtual ActionResult Index(IEnumerable<string> codes = null, bool isPartial = false)
        {
            IEnumerable<IMicroModuleBase> mms = _microModuleFactory.GetAllByCode(codes);
            
            var model = new WidgetsViewModel()
            {
                Widgets = mms.Select(mm =>
                {
                    mm.WidgetSettings.IsPartial = isPartial;
                    return new MicroModuleWidget(mm, mm.WidgetSettings);
                }),
                IsPartial = isPartial

            };
            return View(MVC.CMS.MicroModule.Views.ViewNames.Index, model);
        }

        public virtual ActionResult Open(string code)
        {
            return Index(new List<string> { code }, true);
        }

        public virtual ActionResult Kind(IEnumerable<string> kinds = null)
        {
            IEnumerable<IMicroModuleBase> mms = _microModuleFactory.GetAllByKind(kinds);

            var model = new WidgetsViewModel()
            {
                Widgets = mms.Select(mm => new MicroModuleWidget(mm, mm.WidgetSettings))
            };

            return View(MVC.CMS.MicroModule.Views.ViewNames.Index, model);
        }


        public virtual ActionResult CatalogItem(string code)
        {
            IEnumerable<IMicroModuleBase> mms = _microModuleFactory.GetAllByCatalogItemCode(code);

            var model = new WidgetsViewModel()
            {
                Widgets = mms.Select(mm => new MicroModuleWidget(mm, mm.WidgetSettings))
            };
            return View(MVC.CMS.MicroModule.Views.ViewNames.Index, model);
        }
    }
}