using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models.Catalog;
using livemenu.Kernel.Catalog;
using livemenu.Models;

namespace livemenu.Areas.Projects.Controllers
{
    public partial class HomeController : ProjectsController
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
            return View(MVC.Projects.Home.Views.Menu);
        }
        public virtual ActionResult LaunchProject()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "projects-new-project",
                IsIFrame = true,
                Name = "Запустить проект",
                InstructionLink = "http://www.WebUni.ru/platform-projects"
            });
        }

        public virtual ActionResult Catalog()
        {
            var catalogItem = _catalogItemService.Get("WebUni-projects");
            return View(MVC.CMS.Catalog.Views.Index, new CatalogMainModel
            {
                CanHaveChildren = true,
                RootID = catalogItem.RootID,
                CatalogItemCode = catalogItem.Code,
                CatalogItemID = catalogItem.ID
            });
        }

        public virtual ActionResult Projects()
        {
            var catalogItem = _catalogItemService.Get("NewsList"); //todo
            return View(MVC.CMS.Catalog.Views.Index, new CatalogMainModel
            {
                CanHaveChildren = true,
                RootID = catalogItem.RootID,
                CatalogItemCode = catalogItem.Code,
                CatalogItemID = catalogItem.ID
            });
        }

        public virtual ActionResult Action()
        {
            var catalogItem = _catalogItemService.Get("ActionsList");
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