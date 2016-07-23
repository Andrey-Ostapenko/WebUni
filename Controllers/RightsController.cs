using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.Team.Controllers;
using livemenu.Common.Entities;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Rights;
using livemenu.Models.Auth;
using livemenu.Models.Rights;
using livemenu.Models.Rights.Catalogs;
using livemenu.Models.Widgets;

namespace livemenu.Controllers
{
    [CustomAuthorize(AccessMode.Standart, new[]
    {
        SimpleRightValue.RightsViewEdit,
    })]
    public partial class RightsController : TeamController
    {
        private readonly IRightsManager _rightsManager;

        public RightsController(IRightsManager rightsManager)
        {
            _rightsManager = rightsManager;
        }

      
        public virtual ActionResult Index(long id)
        {
            var model = new WidgetsViewModel()
            {
                IsPartial = false,
                Widgets = new[]
                {
                    new SimpleWidgetModel
                    {
                        Url = MVC.Rights.Internal(id),
                        Title = "Редактирование прав"
                    }
                }
            };


            return View(MVC.CMS.MicroModule.Views.Index, model);

        }

        public virtual ActionResult Internal(long id)
        {

            return View(MVC.Rights.Views.Index, id);
        }



        public virtual ActionResult RightsDetail(long id)
        {
            return View(id);
        }

        [HttpPost]
        public virtual ActionResult RightsCatalogChanged(long cid, long rsid, int type, bool value)
        {
            _rightsManager.RightsCatalogChanged(cid, rsid, (RightValueType) type, value);
            return null;
        }

        public virtual ActionResult RightsCatalog()
        {


            return View();
        }

        #region UsersList

        [ValidateInput(false)]
        public virtual ActionResult RightsMenuGridViewPartial(long id)
        {
            ViewBag.RightSubjectID = id;

            var items = new List<RightMenuItem>
            {
                new RightMenuItem
                {
                    ID = 0,
                    Name = "Общие права",
                    Url = Url.Action(MVC.Rights.SimpleRightsGridViewPartial(id))
                }
            };

            items.AddRange(_rightsManager.GetAllCatalogs(id).Select(x => new RightMenuItem()
            {
                Name = x.Name,
                Url = Url.Action(MVC.Rights.RightsCatalogTreeListPartial(-x.ID, id, true))
            }));
            return PartialView("_RightsMenuGridViewPartial", items);
        }

        #endregion

        #region SimpleRights

        [ValidateInput(false)]
        public virtual ActionResult SimpleRightsGridViewPartial(long id)
        {
            ViewBag.RightSubjectID = id;
            return PartialView("_SimpleRightsGridViewPartial", _rightsManager.LoadSimpleRights(id));
        }

        [ValidateInput(false)]
        public virtual ActionResult SimpleRightsChanged(long rsid, int sid, bool value)
        {
            _rightsManager.SimpleRightsChangedChanged(rsid, (SimpleRightValue) sid, value);
            ViewBag.RightSubjectID = rsid;
            return PartialView("_SimpleRightsGridViewPartial", _rightsManager.LoadSimpleRights(rsid));
        }

        #endregion

        [ValidateInput(false)]
        //public virtual ActionResult RightsCatalogTreeListPartial(long cid, long rsid)
        public virtual ActionResult RightsCatalogTreeListPartial(long cid, long rsid, bool isRoot = false)
        {
            return PartialView("_RightsCatalogTreeListPartial", new RightsCatalogModel
            {
                CatalogItemID = cid,
                RightSubjectID = rsid,
                IsRoot = isRoot
            });
        }
    }
}