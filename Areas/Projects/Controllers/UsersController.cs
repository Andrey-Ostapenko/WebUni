using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Areas.Cloud.Controllers;
using livemenu.Areas.CMS.Controllers;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Cloud;
using livemenu.Kernel.CRM;
using livemenu.Kernel.Projects;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.Projects.Controllers
{
    public partial class UsersController : ProjectsController
    {
        private readonly IClientRoleService _clientRoleService;
        private readonly IClientRoleStatusService _clientRoleStatusService;

        public UsersController(IClientRoleService clientRoleService, IClientRoleStatusService clientRoleStatusService)
        {
            _clientRoleService = clientRoleService;
            _clientRoleStatusService = clientRoleStatusService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "projects-roles",
                Name = "Роли",
                InstructionLink = "http://www.WebUni.ru/platform-projects-roles"
            });
        }


        [HttpGet]
        public virtual JsonResult SearchByClientRole(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var sections = _clientRoleService.RawDataQueryable.Where(x => x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page - 1) * pagelimit).Take(pagelimit);
            var count = _clientRoleService.RawDataQueryable.Count(x => x.Name.ToLower().Contains(lid));
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetByClientRole(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var sections = _clientRoleService.RawDataQueryable;


            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var tag = sections.FirstOrDefault(m => m.ID == idInt);
                    if (tag == null)
                        continue;
                    items.Add(new SelectItem
                    {
                        id = (int)tag.ID,
                        text = tag.Name
                    });
                }
            }


           

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult SearchByClientRoleStatus(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var sections = _clientRoleStatusService.RawDataQueryable.Where(x => x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page - 1) * pagelimit).Take(pagelimit);
            var count = _clientRoleStatusService.RawDataQueryable.Count(x => x.Name.ToLower().Contains(lid));
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetByClientRoleStatus(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var sections = _clientRoleStatusService.RawDataQueryable;

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var tag = sections.FirstOrDefault(m => m.ID == idInt);
                    if (tag == null)
                        continue;
                    items.Add(new SelectItem
                    {
                        id = (int)tag.ID,
                        text = tag.Name
                    });
                }
            }


            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #region roles

        public virtual ActionResult Roles()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "projects-roles",
                Name = "Роли",
                InstructionLink = "http://www.WebUni.ru/platform-projects-roles"
            });
        }

        public virtual ActionResult RolesGridViewPartial()
        {
            return View();
        }

        public GridViewSettings GetRolesGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "RoleGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "Group";
                column.Caption = "Группа";
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
            });

            return settings;
        }


        [ValidateInput(false)]
        public virtual ActionResult RolesGridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    RolesInsert(product, updateValues);
            }
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                    RolesUpdate(product, updateValues);
            }
            foreach (var productID in updateValues.DeleteKeys)
            {
                RolesDelete(productID, updateValues);
            }
            return PartialView(MVC.Projects.Users.Views.RolesGridViewPartial);
        }



        protected void RolesInsert(ClientRole clientRole, MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    _clientRoleService.Create(clientRole);
                });
            }
            else
            {
                updateValues.SetErrorText(clientRole, "Ошибка");
            }
        }
        protected void RolesUpdate(ClientRole clientRole, MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _clientRoleService.Update(clientRole);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(clientRole, "Ошибка");
            }
        }
        protected void RolesDelete(int id, MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _clientRoleService.Delete(id);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        #endregion


        #region statuses

        public virtual ActionResult Statuses()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "projects-statuses",
                Name = "Статусы",
                InstructionLink = "http://www.WebUni.ru/platform-projects-statuses"
            });
        }

        public virtual ActionResult StatusesGridViewPartial()
        {
            return View();
        }

        public GridViewSettings GetStatusesGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "StatusGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "Group";
                column.Caption = "Группа";


            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";


            });

            return settings;
        }


        [ValidateInput(false)]
        public virtual ActionResult StatusesGridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<ClientRoleStatus, int> updateValues)
        {
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    StatusesInsert(product, updateValues);
            }
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                    StatusesUpdate(product, updateValues);
            }
            foreach (var productID in updateValues.DeleteKeys)
            {
                StatusesDelete(productID, updateValues);
            }
            return PartialView(MVC.Projects.Users.Views.StatusesGridViewPartial);
        }



        protected void StatusesInsert(ClientRoleStatus clientRoleStatus, MVCxGridViewBatchUpdateValues<ClientRoleStatus, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    _clientRoleStatusService.Create(clientRoleStatus);
                });
            }
            else
            {
                updateValues.SetErrorText(clientRoleStatus, "Ошибка");
            }
        }
        protected void StatusesUpdate(ClientRoleStatus clientRoleStatus, MVCxGridViewBatchUpdateValues<ClientRoleStatus, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _clientRoleStatusService.Update(clientRoleStatus);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(clientRoleStatus, "Ошибка");
            }
        }
        protected void StatusesDelete(int id, MVCxGridViewBatchUpdateValues<ClientRoleStatus, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _clientRoleStatusService.Delete(id);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        #endregion


    }
}