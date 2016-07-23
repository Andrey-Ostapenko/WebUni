using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models.Catalog;
using livemenu.Kernel.Catalog;

namespace livemenu.Areas.Tasks.Controllers
{
    public partial class HomeController : TasksController
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
            return View(MVC.Tasks.Home.Views.Menu);
        }

        public virtual ActionResult Catalog()
        {
            var catalogItem = _catalogItemService.Get("WebUni-tasks");
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