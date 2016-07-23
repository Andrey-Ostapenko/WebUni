using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Areas.Team.Controllers;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Positions;
using livemenu.Models;
using livemenu.Models.Auth;
using livemenu.Models.Widgets;

namespace livemenu.Controllers
{
    [CustomAuthorize(AccessMode.Standart, new[]
    {
        SimpleRightValue.AdminMenuView,
        SimpleRightValue.PositionsViewEdit,
    })]
    public partial class PositionController : TeamController
    {
        private readonly IPositionService _positionService;

        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }

     
        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "team-position",
                Name = "Должности",
                InstructionLink = "http://www.WebUni.ru/platform-team-positions"
            });

            //var model = new WidgetsViewModel()
            //{
            //    IsPartial = false,
            //    Widgets = new[]
            //    {
            //        new SimpleWidgetModel
            //        {
            //            Url = MVC.Position.Internal(),
            //            Title = "Должности"
            //        }
            //    }
            //};


            //return View(MVC.CMS.MicroModule.Views.Index, model);

        }

        public virtual ActionResult Internal()
        {
            return View(MVC.Position.Views.Index);
        }


        #region positions
        public virtual ActionResult PositionsGridViewPartial()
        {
            return View(_positionService.GetAll());
        }

        public virtual ActionResult PositionsGridViewAddNewPartial(Position position)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _positionService.Create(position));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.Position.Views.ViewNames.PositionsGridViewPartial, _positionService.GetAll());
        }

        public virtual ActionResult PositionsGridViewUpdatePartial(Position position)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _positionService.Update(position));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.Position.Views.ViewNames.PositionsGridViewPartial, _positionService.GetAll());
        }

        public virtual ActionResult PositionsGridViewDeletePartial(long id)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _positionService.Delete(id));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.Position.Views.ViewNames.PositionsGridViewPartial, _positionService.GetAll());
        }


        #endregion
    }
}