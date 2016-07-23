using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models.Catalog;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.Catalog.Custom;

namespace livemenu.Areas.CRM.Controllers 
{
    public partial class PropertiesController : CRMController
    {
        private readonly ICatalogItemService _catalogItemService;

        public PropertiesController(ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService;
        }

        public virtual ActionResult Index()
        {
            return RedirectToAction(MVC.CMS.Catalog.Index(CustomCatalogCodes.PriceListElementProperties));
        }

        public virtual ActionResult Selector()
        {
            var catalogItem = _catalogItemService.Get(CustomCatalogCodes.PriceListElementProperties);
            return View(new CatalogMainModel
            {
                RootID = catalogItem.RootID,
                CatalogItemCode = catalogItem.Code,
                CatalogItemID = catalogItem.ID
            });
            return View();
        }
	}
}