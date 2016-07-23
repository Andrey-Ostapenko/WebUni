using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Areas.WMS.Controllers;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.CRM.Orders;
using livemenu.Kernel.CRM.Storages;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.WMS.Controllers
{
    public partial class StoragesController : WMSController
    {
        private readonly IStorageService _storageService;
        private readonly IStoragePriceListElementService _storagePriceListElementService;
        private readonly IStoragesExcelImportService _storagesExcelImportService;

        public StoragesController(IStorageService storageService, IStoragePriceListElementService storagePriceListElementService, IStoragesExcelImportService storagesExcelImportService)
        {
            _storageService = storageService;
            _storagePriceListElementService = storagePriceListElementService;
            _storagesExcelImportService = storagesExcelImportService;
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
                //var path = Server.MapPath(file);
                   var d = ServiceLocator.Current.GetInstance<IStoragesExcelImportService>();
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
            var storages = _storagePriceListElementService.RawDataQueryable.Include(x => x.PriceListElement).Include(x => x.Storage).OrderBy(x => x.StorageID).ToList();


            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewExportSettings(), storages, "Склады");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewExportSettings(), storages, "Склады");
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "platform-wms-fm",
                Name = "Склады",
                InstructionLink = "http://www.WebUni.ru/platform-wms"
            });
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
                column.FieldName = "Storage.Address.AddressString";
                column.Caption = "Адрес";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });



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
                column.FieldName = "Count";
                column.Caption = "Количество";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "PriceListElementID";
                column.Caption = "ID Товара";
            });


            return settings;
        }

        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "StoragesGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "Address.CityID";
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
                column.FieldName = "Name";
                column.Caption = "Название";
            });

           

            settings.Columns.Add(column =>
            {
                column.FieldName = "Address.AddressString";
                column.Caption = "Адрес";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "AddressID";
                column.Caption = "ID адреса";
            });


            return settings;
        }



        public virtual ActionResult GridViewPartial()
        {
            return View();
        }




        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<Storage, int> updateValues)
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
            return PartialView(MVC.WMS.Storages.Views.GridViewPartial);
        }




        protected void Insert(Storage storage, MVCxGridViewBatchUpdateValues<Storage, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _storageService.Create(storage));
            }
            else
            {
                updateValues.SetErrorText(storage, "Ошибка");
            }
        }
        protected void Update(Storage storage, MVCxGridViewBatchUpdateValues<Storage, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _storageService.Update(storage));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(storage, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<Storage, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _storageService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }


        public virtual ActionResult StoragePriceListElementGridViewPartial(long id)
        {
            ViewBag.SID = id;
            return View(id);
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
        public virtual ActionResult AddAddresses(AddAddressesModel data)
        {
            _storageService.AddNew(data.sid, data.ids);
            return Json(true);
        }

       
    }

    public class AddAddressesModel
    {
        public long sid { get; set; }
        public long[] ids { get; set; }
    }

    
}