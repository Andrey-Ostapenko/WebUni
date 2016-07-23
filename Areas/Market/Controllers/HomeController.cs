using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Models;

namespace livemenu.Areas.Market.Controllers
{
    public partial class HomeController : MarketController
    {
        //
        // GET: /CRM/Home/
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Menu()
        {
            return View(MVC.Market.Home.Views.Menu);
        }

        public virtual ActionResult WebUni()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "market-WebUni",
                WithoutInstruction = true,
                IsIFrame = true,
                Name = "WebUni",
                InstructionLink = null
            });
        }

        public virtual ActionResult Shop()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "market-shop",
                WithoutInstruction = true,
                IsIFrame = true,
                Name = "Управление",
                InstructionLink = null
            });
        }
    }
}