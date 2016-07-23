using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.CRM;
using livemenu.Kernel.CRM.Storages;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.WMS.Controllers
{
    public partial class AvailabilityController : WMSController
    {
        private readonly IStorageService _storageService;
        private readonly IStoragePriceListElementService _storagePriceListElementService;
        private readonly IStorageAvailabilityService _storageAvailabilityService;
        private readonly IAddressesExcelImportService _addressesExcelImportService;

        public AvailabilityController(IStorageService storageService,  IStoragePriceListElementService storagePriceListElementService, IStorageAvailabilityService storageAvailabilityService, IAddressesExcelImportService _addressesExcelImportService)
        {
            _storageService = storageService;
            _storagePriceListElementService = storagePriceListElementService;
            _storageAvailabilityService = storageAvailabilityService;
            this._addressesExcelImportService = _addressesExcelImportService;
        }

        public virtual ActionResult Index(long? storageID)
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-wms-fm",
                Name = "Наличие на складах",
                InstructionLink = "http://www.WebUni.ru/platform-wms"
            });
            //return View(storageID.HasValue ? _storageService.Get(storageID.Value) : null);
        }

        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "StorageAvailabilityGridView";
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
                comboBoxProperties.ValueType = typeof(long);

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (ComboBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;

            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "Expr2";
                column.Caption = "Склад";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });



            settings.Columns.Add(column =>
            {
                column.FieldName = "AddressString";
                column.Caption = "Адрес склада";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            

            settings.Columns.Add(column =>
            {
                column.FieldName = "Articul";
                column.Caption = "Артикул";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Наименование";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "PriceListElement.Details";
            //    column.Caption = "Описание";

            //    column.EditFormSettings.Visible = DefaultBoolean.False;

            //    var props = (TextBoxProperties)column.PropertiesEdit;
            //    props.ValidationSettings.Display = Display.None;
            //});

            settings.Columns.Add(column =>
            {
                column.FieldName = "Availability";
                column.Caption = "Наличие";

                column.ColumnType = MVCxGridViewColumnType.CheckBox;

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "Count";
                column.Caption = "Количество";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElementID";
                column.Caption = "ID";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "StorageID";
                column.Caption = "ID склада";
            });

            return settings;
        }



        public virtual ActionResult GridViewPartial(long? storageID)
        {
            return View(storageID);
        }




        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<StorageAvailability, int> updateValues, long? storageID)
        {
            foreach (var storagePriceListElement in updateValues.Insert)
            {
                if (updateValues.IsValid(storagePriceListElement))
                    Insert(storagePriceListElement, updateValues);
            }
            foreach (var storagePriceListElement in updateValues.Update)
            {
                if (updateValues.IsValid(storagePriceListElement))
                    Update(storagePriceListElement, updateValues);
            }
            foreach (var storagePriceListElementID in updateValues.DeleteKeys)
            {
                Delete(storagePriceListElementID, updateValues);
            }
            return PartialView(MVC.WMS.Availability.Views.GridViewPartial, storageID);
        }




        protected void Insert(StorageAvailability storagePriceListElement, MVCxGridViewBatchUpdateValues<StorageAvailability, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _storageAvailabilityService.Create(storagePriceListElement));
            }
            else
            {
                updateValues.SetErrorText(storagePriceListElement, "Ошибка");
            }
        }
        protected void Update(StorageAvailability storagePriceListElement, MVCxGridViewBatchUpdateValues<StorageAvailability, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _storageAvailabilityService.Update(storagePriceListElement));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(storagePriceListElement, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<StorageAvailability, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _storageAvailabilityService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }






        [ValidateInput(false)]
        public virtual ActionResult StoragePriceListElementGridViewUpdatePartial(StoragePriceListElement storagePriceListElement)
        {
            var sID = _storagePriceListElementService.Get(storagePriceListElement.ID).StorageID;
            ViewBag.SID = sID;
            storagePriceListElement.StorageID = sID;
            // if (ModelState.IsValid)
            {
                UpdateWrapper(() => _storagePriceListElementService.Update(storagePriceListElement));
            }
            //else
            //    ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.WMS.Storages.Views.StoragePriceListElementGridViewPartial, sID);
        }

        public virtual ActionResult StoragePriceListElementGridViewDeletePartial(long id)
        {
            var sID = _storagePriceListElementService.Get(id).StorageID;
            ViewBag.SID = sID;


            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _storagePriceListElementService.Delete(id));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.WMS.Storages.Views.StoragePriceListElementGridViewPartial, sID);
        }


        [HttpPost]
        public virtual ActionResult AddPriceListElements(AddStoragePriceListElementsModel data)
        {
            _storagePriceListElementService.AddNew(data.sid, data.ids);
            return Json(true);
        }



        public GridViewSettings GetGridViewExportSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "StoragesExportGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";


            settings.Columns.Add(column =>
            {
                column.FieldName = "Storage.Name";
                column.Caption = "Склад";

            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "StorageID";
                column.Caption = "ID cклада";

            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElement.Brand";
                column.Caption = "Бренд";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElement.Group";
                column.Caption = "Группа";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElement.Articul";
                column.Caption = "Артикул";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElement.Name";
                column.Caption = "Наименование";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElement.Details";
                column.Caption = "Описание";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Count";
                column.Caption = "Количество";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
            });






            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElementID";
                column.Caption = "ID";
            });





            return settings;
        }

        public class AddStoragePriceListElementsModel
        {
            public long sid { get; set; }
            public long[] ids { get; set; }
        }
    }
}