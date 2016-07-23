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
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;
using livemenu.Kernel.Fonts;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class FontController : CMSController
    {
        private readonly IFontService _fontService;

        public FontController(IFontService fontService)
        {
            _fontService = fontService;
        }

        [HttpGet]
        public virtual JsonResult SearchFont(string id)
        {
            var lid = id.ToLower();
            var fonts = _fontService.GetAll().Where(x => x.Name.ToLower().Contains(lid)).Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            });

            return Json(fonts, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetFont(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();

            long idInt;
            if (long.TryParse(id, out idInt))
            {
                var font = _fontService.Get(idInt);
                if(font != null)
                {
                    items.Add(new SelectItem
                    {
                        id = (int)idInt,
                        text = font.Name
                    });
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }


}