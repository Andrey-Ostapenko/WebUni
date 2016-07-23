using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Cache;
using livemenu.Common.Kernel.ShortLink;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using livemenu.Models;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class ShortLinkController : CMSController
    {
        private readonly IShortLinkService _shortLinkService;

        public ShortLinkController(IShortLinkService shortLinkService)
        {
            _shortLinkService = shortLinkService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-short-links",
                Name = "Короткие ссылки",
                InstructionLink = "http://www.WebUni.ru/platform-cms-shortlinks"
            });
        }

        public virtual ActionResult GridViewPartial()
        {
            return View();
        }

        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "ShortLinkGridView";
            settings.Width = new System.Web.UI.WebControls.Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

         

            settings.Columns.Add(column =>
            {
                column.FieldName = "LinkUrl";
                column.Caption = "Код страницы с параметрами";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ShortLinkUrl";
                column.Caption = "Короткая ссылка";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ShortLinkUrlHash";
                column.Caption = "ShortLinkUrlHash";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "LinkUrlHash";
                column.Caption = "LinkUrlHash";
            });

            return settings;
        }



        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<ShortLink, int> updateValues)
        {
            foreach (var orderStatus in updateValues.Insert)
            {
                if (updateValues.IsValid(orderStatus))
                    Insert(orderStatus, updateValues);
            }
            foreach (var orderStatus in updateValues.Update)
            {
                if (updateValues.IsValid(orderStatus))
                    Update(orderStatus, updateValues);
            }
            foreach (var orderStatusID in updateValues.DeleteKeys)
            {
                Delete(orderStatusID, updateValues);
            }
            return PartialView(MVC.CMS.ShortLink.Views.GridViewPartial);
        }

        protected void Insert(ShortLink property, MVCxGridViewBatchUpdateValues<ShortLink, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _shortLinkService.Create(property));
            }
            else
            {
                updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Update(ShortLink property, MVCxGridViewBatchUpdateValues<ShortLink, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _shortLinkService.Update(property));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<ShortLink, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _shortLinkService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }
    }
}