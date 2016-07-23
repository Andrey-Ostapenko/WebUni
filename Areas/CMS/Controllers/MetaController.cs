using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Kernel.DynamicExecution;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class MetaController : Controller
    {
        private readonly IDynamicExecutor _dynamicExecutor;

        public MetaController(IDynamicExecutor dynamicExecutor)
        {
            _dynamicExecutor = dynamicExecutor;
        }

        public virtual ActionResult Index()
        {
            var code = @"

            var items = Catalogs.Get(ci => ci.TypeCode == ""Line"").ToArray();

            foreach(var item in items)
            {
                item.Code = ""line_"" + item.ID;

                Catalogs.Save(item);
            }


            ";

            _dynamicExecutor.Execute(code);
            return View();
        }
	}
}