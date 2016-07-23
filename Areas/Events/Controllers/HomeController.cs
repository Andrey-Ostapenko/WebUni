using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models.Catalog;
using livemenu.Kernel.Catalog;

namespace livemenu.Areas.Events.Controllers
{
    public partial class HomeController : EventsController
    {
        private readonly ICatalogItemService _catalogItemService;

        public HomeController(ICatalogItemService catalogItemService)
        {
            _catalogItemService = catalogItemService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Events.Home.Views.Menu);
        }

        public virtual ActionResult Catalog()
        {
            var catalogItem = _catalogItemService.Get("WebUni-events");
            return View(MVC.CMS.Catalog.Views.Index, new CatalogMainModel
            {
                CanHaveChildren = true,
                RootID = catalogItem.RootID,
                CatalogItemCode = catalogItem.Code,
                CatalogItemID = catalogItem.ID
            });
        }
    }
}