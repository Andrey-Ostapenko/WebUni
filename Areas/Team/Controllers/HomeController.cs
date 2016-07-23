using System.Web.Mvc;
using livemenu.Areas.BWP.Models.Catalog;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.Executing;
using livemenu.Models.Widgets;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Areas.Team.Controllers
{
    public partial class HomeController : TeamController
    {
        private readonly ICatalogItemService _catalogItemService;

        public HomeController(ICatalogItemService catalogItemService )
        {
            _catalogItemService = catalogItemService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Team.Home.Views.Menu);
        }


        public virtual ActionResult Vacancies()
        {
            var context = ServiceLocator.Current.GetInstance<ILMExecutingContext>();
            context.SetExecutingType(LMExecutingType.Team, true);

            var catalogItem = _catalogItemService.Get("WebUni-vacancies");
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