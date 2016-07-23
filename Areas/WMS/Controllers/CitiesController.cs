using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Areas.CMS.Controllers;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.CRM.Storages;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;


namespace livemenu.Areas.WMS.Controllers
{
    public partial class CitiesController : WMSController
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-wms-fm",
                Name = "Города",
                InstructionLink = "http://www.WebUni.ru/platform-wms"
            });
        }


        public GridViewSettings GetGridViewSettings(bool isExcel = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "CityGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "CountryID";
                column.Caption = "Страна";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<ICountryService>().GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Name";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
            });

            if (isExcel)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "CountryID";
                    column.Caption = "ID страны";
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });
            }

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            if (isExcel)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "ID";
                    column.Caption = "ID города";
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });
            }

            return settings;
        }

        [HttpPost]
        public virtual JsonResult Import(string file)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Импорт запущен"
                });
                var path = Server.MapPath("/Admin/" + file);

                var d = ServiceLocator.Current.GetInstance<ICityExcelImportService>();
                d.Import(path, mes =>
                {
                    _notificationBus.Notificate(new NotificationMessage()
                    {
                        Text = mes
                    });
                });
                return true;
            }, "Импорт успешно завершен");
            return null;
        }

        public virtual ActionResult ExportTo()
        {
            var addresses = _cityService.RawDataQueryable.ToList();

            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewSettings(true), addresses, "Города");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewSettings(true), addresses, "Города");
        }

        public virtual ActionResult GridViewPartial()
        {
            return View(_cityService.GetAll());
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<City, int> updateValues)
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
            return PartialView(MVC.WMS.Cities.Views.GridViewPartial, _cityService.GetAll());
        }

        protected void Insert(City city, MVCxGridViewBatchUpdateValues<City, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _cityService.Create(city));
            }
            else
            {
                updateValues.SetErrorText(city, "Ошибка");
            }
        }
        protected void Update(City city, MVCxGridViewBatchUpdateValues<City, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _cityService.Update(city));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(city, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<City, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _cityService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        [HttpGet]
        public virtual JsonResult SearchCity(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var sections = _cityService.RawDataQueryable.Where(x => x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page - 1) * pagelimit).Take(pagelimit);
            var count = _cityService.RawDataQueryable.Count(x => x.Name.ToLower().Contains(lid));
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetCity(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _cityService.RawDataQueryable;
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