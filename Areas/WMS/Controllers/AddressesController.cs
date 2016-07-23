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
    public partial class AddressesController : WMSController
    {
        private readonly IAddressService _addressService;
        public AddressesController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-wms-fm",
                Name = "Адреса",
                InstructionLink = "http://www.WebUni.ru/platform-wms"
            });
        }


        public GridViewSettings GetGridViewSettings(bool isExport = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "AddressesGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

       

            settings.Columns.Add(column =>
            {
                column.FieldName = "CityID";
                column.Caption = "Город";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<ICityService>().GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Name";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof (long);
            });

            if (isExport)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "CityID";
                    column.Caption = "ID города";
                });
            }

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "AddressString";
                column.Caption = "Адрес";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Latitude";
                column.Caption = "Широта";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Longitude";
                column.Caption = "Долгота";
            });

            if (isExport)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "ID";
                    column.Caption = "ID";
                });
            }


            return settings;
        }

        public virtual ActionResult SelectorGridViewPartial()
        {
            return View();
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
               
                var d = ServiceLocator.Current.GetInstance<IAddressesExcelImportService>();
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
            var addresses = _addressService.RawDataQueryable.ToList();

            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewSettings(true), addresses, "Адреса");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewSettings(true), addresses, "Адреса");
        }


        public virtual ActionResult GridViewPartial()
        {
            return View();
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<Address, int> updateValues)
        {
            foreach (var storage in updateValues.Insert)
            {
                if (updateValues.IsValid(storage))
                    Insert(storage, updateValues);
            }
            foreach (var storage in updateValues.Update)
            {
                if (updateValues.IsValid(storage))
                    Update(storage, updateValues);
            }
            foreach (var storageID in updateValues.DeleteKeys)
            {
                Delete(storageID, updateValues);
            }
            return PartialView(MVC.WMS.Addresses.Views.GridViewPartial);
        }

        protected void Insert(Address address, MVCxGridViewBatchUpdateValues<Address, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _addressService.Create(address));
            }
            else
            {
                updateValues.SetErrorText(address, "Ошибка");
            }
        }

        protected void Update(Address address, MVCxGridViewBatchUpdateValues<Address, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _addressService.Update(address));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(address, "Ошибка");
            }
        }

        protected void Delete(int id, MVCxGridViewBatchUpdateValues<Address, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _addressService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }
        [HttpGet]
        public virtual JsonResult SearchAddress(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var sections = _addressService.RawDataQueryable.Where(x=>x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page - 1) * pagelimit).Take(pagelimit);
            var count = _addressService.RawDataQueryable.Count(x => x.Name.ToLower().Contains(lid));
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetAddress(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _addressService.RawDataQueryable;
            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var tag = sections.FirstOrDefault(m => m.ID == idInt);

                    items.Add(new SelectItem
                    {
                        id = (int) tag.ID,
                        text = tag.Name
                    });
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult UpdateAddressCoords(int? mode = null, long? id = null)
        {

            _addressService.Geocode(false);
            return Json("", JsonRequestBehavior.AllowGet);

        }

    }



}