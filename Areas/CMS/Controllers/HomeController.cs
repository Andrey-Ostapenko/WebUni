using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.CRM.Services;
using livemenu.Common.Kernel.Catalog;
using livemenu.Common.Kernel.Order;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.BWP;
using livemenu.Kernel.Menu;
using livemenu.Models.Menu;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class
        HomeController : CMSController
    {
        private readonly IKernelStateInfo _kernelStateInfo;
        private readonly IDynamicMenuItemsProvider _dynamicMenuItemsProvider;

        public HomeController(IKernelStateInfo kernelStateInfo, IDynamicMenuItemsProvider dynamicMenuItemsProvider)
        {
            _kernelStateInfo = kernelStateInfo;
            _dynamicMenuItemsProvider = dynamicMenuItemsProvider;
        }
        public virtual ActionResult Index()
        {
            var _catalogStorageManager = ServiceLocator.Current.GetInstance<ICatalogStorageManager>();

            // var watch = Stopwatch.StartNew();


            // var k = d.DeepLoadAllChildCatalogItems("headerpage");
            
            //watch.Stop();
            // var elapsedMs = watch.ElapsedMilliseconds;


            // watch = Stopwatch.StartNew();

            // k = d.DeepLoadAllChildCatalogItems2("headerpage");


            // watch.Stop();
            // var elapsedMs2 = watch.ElapsedMilliseconds;


         //   var p = "p-mobil";
          //  var s = "1";
            //var k = _catalogStorageManager.SiteGroupFullTextSearch(p, s);
            //var d = k;

            //var childrenCodes = _catalogStorageManager.GetAllChildCatalogItems("section_9").OrderBy(x => x.Priority).Select(x => new 
            //{
            //    Code = x.Code,
            //    Name = x.Name
            //}).ToList();


      //      var red = childrenCodes;


            //var act = ServiceLocator.Current.GetInstance<IActualizator>();
            //try
            //{
            //    act.CustomScript2();
            //}
            //catch (Exception e)
            //{
            //    var d = e;


            //    var asdfasdf = d;
            //}
            //act.ActializeFL("SectionSettings", new List<string>
            //{
            //    "WideBackground",
            //});

            //var test = ServiceLocator.Current.GetInstance<IOrderMakerService>();
            //test.IncomeRecived(8,315, OrderIncomeRecivedVerificationMode.OnlyFullPrice  );



            //var childrenCodes = _catalogStorageManager.GetAllChildCatalogItems("section_9").OrderBy(x => x.Priority).Select(x => new
            //{
            //    Code = x.Code,
            //    Name = x.Name
            //}).ToList();

            //var act = ServiceLocator.Current.GetInstance<IActualizator>();
            //try
            //{
            //    act.CustomScript1();
            //}
            //catch (Exception e)
            //{
            //    var d = e;


            //    var asdfasdf = d;
            //}


            //var res =  _catalogStorageManager.DeepLoadAllChildCatalogItemsFastWithSearchIDs("link-motornie-masla", new List<long>()
            //{
            //    390,391
            //});


          //  new EmailSenderService().OrderCreated(28);

            return View();
        }

        public virtual ActionResult Menu()
        {
            var items = _dynamicMenuItemsProvider.GetMenuItems();
            return View(MVC.CMS.Home.Views.Menu, new MenuModel() { AccessMode = _kernelStateInfo.AccessMode, MicroModuleMenuItemsModel = items });
        }
    }
}