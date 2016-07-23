using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models.CatalogItemSelector;
using livemenu.Controllers;
using livemenu.Kernel.CatalogItemSelector;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class CatalogItemSelectorController : CMSController
    {
        private readonly ICatalogItemSelectorService _catalogItemSelectorService;

        public CatalogItemSelectorController(ICatalogItemSelectorService catalogItemSelectorService)
        {
            _catalogItemSelectorService = catalogItemSelectorService;
        }

        public virtual ActionResult CatalogItemSelectorGridViewPartial(long ciID)
        {
        //    var items = _catalogItemSelectorService.GetLinksByCatalogItemID(ciID);
            return View(MVC.CMS.CatalogItemSelector.Views._CatalogItemSelectorGridViewPartial, new CatalogItemSelectorModel()
            {
                CIID = ciID,
             //   CatalogItems = items
            });
        }
    }
}