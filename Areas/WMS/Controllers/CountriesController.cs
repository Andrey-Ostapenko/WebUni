using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web.Mvc;
using livemenu.Areas.CMS.Controllers;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.CRM.Storages;
using livemenu.Models;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.WMS.Controllers
{
    public partial class CountriesController : WMSController
    {
        private readonly ICountryService _countryService;

        public CountriesController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-wms-fm",
                Name = "Страны",
                InstructionLink = "http://www.WebUni.ru/platform-wms"
            });
        }


        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "CountryGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";



            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });



            return settings;
        }



        public virtual ActionResult GridViewPartial()
        {
            return View(_countryService.GetAll());
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<Country, int> updateValues)
        {
            foreach (var city in updateValues.Insert)
            {
                if (updateValues.IsValid(city))
                    Insert(city, updateValues);
            }
            foreach (var city in updateValues.Update)
            {
                if (updateValues.IsValid(city))
                    Update(city, updateValues);
            }
            foreach (var cityID in updateValues.DeleteKeys)
            {
                Delete(cityID, updateValues);
            }
            return PartialView(MVC.WMS.Countries.Views.GridViewPartial, _countryService.GetAll());
        }

        protected void Insert(Country city, MVCxGridViewBatchUpdateValues<Country, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _countryService.Create(city));
            }
            else
            {
                updateValues.SetErrorText(city, "Ошибка");
            }
        }
        protected void Update(Country city, MVCxGridViewBatchUpdateValues<Country, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _countryService.Update(city));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(city, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<Country, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _countryService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        [HttpGet]
        public virtual JsonResult SearchCountry(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var sections = _countryService.RawDataQueryable.Where(x => x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page - 1) * pagelimit).Take(pagelimit);
            var count = _countryService.RawDataQueryable.Count(x => x.Name.ToLower().Contains(lid));
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetCountry(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _countryService.RawDataQueryable;
            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var tag = sections.FirstOrDefault(m => m.ID == idInt);

                    items.Add(new SelectItem
                    {
                        id = (int)tag.ID,
                        text = tag.Name
                    });
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }
    }
}