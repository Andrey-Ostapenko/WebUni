using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Areas.CMS.Controllers;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.CMS;
using livemenu.Kernel.CRM.Storages;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;


namespace livemenu.Areas.CMS.Controllers
{
    public partial class TagsGroupController : CMSController
    {
        private readonly ITagGroupService _tagGroupService;

        public TagsGroupController(ITagGroupService tagGroupService)
        {
            _tagGroupService = tagGroupService;
        }


        [HttpGet]
        public virtual JsonResult SearchTagGroup(string id)
        {
            var groups = new List<SelectItem>();

            var lid = id.ToLower();
            var grouped = _tagGroupService.GetAll().Where(x => x.Name.ToLower().Contains(lid));

            foreach (var group in grouped)
            {
                groups.Add(new SelectItem
                {
                    id = (int)group.ID,
                    text = group.Name
                });
            }

            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetTagGroup(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var tagGroups = _tagGroupService.GetAll();

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var tagGroup = tagGroups.FirstOrDefault(m => m.ID == idInt);
                    if (tagGroup == null)
                        continue;
                    items.Add(new SelectItem
                    {
                        id = (int)tagGroup.ID,
                        text = tagGroup.Name
                    });
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }

}