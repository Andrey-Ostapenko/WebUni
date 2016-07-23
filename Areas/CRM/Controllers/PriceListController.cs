using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;

using livemenu.Common.Entities.Entities;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.CRM;
using livemenu.Kernel.CRM.PriceList;
using livemenu.Kernel.CRM.Properties;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;
using livemenu.Common.Entities;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class PriceListController : CRMController
    {
        private readonly IPriceListElementService _priceListElementService;
        private readonly IPriceListExcelImportService _priceListExcelImportService;
        private readonly IPriceListVersionService _priceListVersionService;
        private readonly IPriceListElementVersionService _priceListElementVersionService;

        public PriceListController(
            IPriceListElementService priceListElementService,
            IPriceListExcelImportService priceListExcelImportService,
            IPriceListVersionService priceListVersionService,
            IPriceListElementVersionService priceListElementVersionService

        )
        {
            _priceListElementService = priceListElementService;
            _priceListExcelImportService = priceListExcelImportService;
            _priceListVersionService = priceListVersionService;
            _priceListElementVersionService = priceListElementVersionService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "crm-pricelist",
                Name = "Прайс-лист",
                InstructionLink = "http://www.WebUni.ru/platform-crm-pricelist"
            });
        }

        public long GetKeyIdByDisplayIndex(int? displayIndex)
        {
            var model = _priceListElementService.RawDataQueryable;
            var rowKey = model.Where(q => q.DisplayIndex == displayIndex).Select(q => q.ID).FirstOrDefault();
            return rowKey;
        }

        public void UpdateDisplayIndex(long? rowKey, int? displayIndex)
        {
            var model = _priceListElementService.RawDataQueryable;
            var product = model.FirstOrDefault(q => q.ID == rowKey);
            product.DisplayIndex = displayIndex ?? 0;
            _priceListElementService.Update(product);
            //_priceListElementService.Commit();
        }

        public virtual ActionResult GridViewPartial(int? focusedRowIndex, int? draggingIndex, int? targetIndex, string command)
        {
            if (command == "DRAGROW")
            {
                var model = _priceListElementService.RawDataQueryable
                    .OrderBy(q => q.DisplayIndex);

                var draggingRowKey = GetKeyIdByDisplayIndex(draggingIndex);
                var targetRowKey = GetKeyIdByDisplayIndex(targetIndex);
                var draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
                for (int rowIndex = 1; rowIndex <= model.Count(); rowIndex++)
                {
                    var rowKey = GetKeyIdByDisplayIndex(rowIndex);
                    if ((rowIndex > Math.Min(targetIndex.Value, draggingIndex.Value)) &&
                        (rowIndex < Math.Max(targetIndex.Value, draggingIndex.Value)))
                    {
                        UpdateDisplayIndex(rowKey, rowIndex + draggingDirection);
                    }
                }
                UpdateDisplayIndex(draggingRowKey, targetIndex);
                UpdateDisplayIndex(targetRowKey, targetIndex + draggingDirection);
            }
            return View(MVC.CRM.PriceList.Views.PriceListElementGridViewPartial);
        }


        public virtual ActionResult SelectorGridViewPartial(bool isSingle = false)
        {
            return View(isSingle);
        }



        public virtual ActionResult ExportTo(long PriceListVersionID)
        {
            string fileName = "Прайс-лист";
            List<PriceListElement> elements = _priceListElementService.GetAll();
            if (PriceListVersionID != 0)
            {
                var priceListVersion = _priceListVersionService.Get(PriceListVersionID);
                if(priceListVersion != null)
                {
                    fileName += "_" + priceListVersion.Name;
                }
                var elementsVersions = _priceListElementVersionService.GetByVersionID(PriceListVersionID);
                foreach(var el in elements)
                {
                    var elVer = elementsVersions.Where(elV => elV.PriceListElementID == el.ID).Single();
                    if (elVer == null) continue;
                    el.Details = elVer.Details;
                    el.Package = elVer.Package;
                    el.Name = elVer.Name;
                    el.UnitPrice = elVer.UnitPrice;
                    el.Price = elVer.Price;
                    el.DiscountPercent = elVer.DiscountPercent;
                    el.Bonus = elVer.Bonus;
                    el.DisplayIndex = elVer.DisplayIndex;
                    el.CurrencyID = elVer.CurrencyID;
                }
            }

            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewSettings(), elements, fileName);
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewSettings(), elements, fileName);
        }

        public GridViewSettings GetGridViewSettings(bool isSelector = false, bool versionMode = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "PriceListGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            //if (RightsManager.IsSuperUser)
            //{
            //  settings.Columns.Add(column =>
            //  {
            //      column.FieldName = "CompanyID";
            //      column.Caption = "Компания";
            //      column.Settings.FilterMode = ColumnFilterMode.DisplayText;

            //      column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            //      column.ColumnType = MVCxGridViewColumnType.ComboBox;
            //      column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
            //      var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
            ////      comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<ICompanyService>().GetAll();
            //      comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            //      comboBoxProperties.AllowUserInput = true;
            //      comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
            //      comboBoxProperties.TextField = "Name";
            //      comboBoxProperties.ValueField = "ID";
            //      comboBoxProperties.ValueType = typeof(long);
            //  });
            //}


            settings.Columns.Add(column =>
            {
                column.Width = new Unit(4, UnitType.Pixel);
                column.FieldName = "DisplayIndex";
                column.Caption = "#";
                column.SortIndex = 1;
                column.SortOrder = ColumnSortOrder.Ascending;
                column.ReadOnly = true;
                if (versionMode) column.CellStyle.BackColor = System.Drawing.Color.LightGray;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Brand";
                column.Caption = "Бренд";
                column.ReadOnly = versionMode;
                if (versionMode) column.CellStyle.BackColor = System.Drawing.Color.LightGray;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "Group";
                column.Caption = "Группа";
                column.ReadOnly = versionMode;
                if (versionMode) column.CellStyle.BackColor = System.Drawing.Color.LightGray;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {

                column.FieldName = "Articul";
                column.Caption = "Артикул";
                column.ReadOnly = versionMode;
                if (versionMode) column.CellStyle.BackColor = System.Drawing.Color.LightGray;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Наименование";


            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "Details";
                column.Caption = "Описание";
            });
            if (!isSelector)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "Package";
                    column.Caption = "Упаковка";
                });

                settings.Columns.Add(column =>
                {
                    column.FieldName = "UnitPrice";
                    column.Caption = "Цена за ед.";
                });
            }
            settings.Columns.Add(column =>
                {
                    column.FieldName = "Price";
                    column.Caption = "Стоимость";

                    //   column.PropertiesEdit.DisplayFormatString = "N2";

                    column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                    var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                    spinEditProperties.DecimalPlaces = 2;
                    spinEditProperties.NumberType = SpinEditNumberType.Float;
                    spinEditProperties.MinValue = 0;

                });
            if (!isSelector)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "CurrencyID";
                    column.Caption = "Валюта";

                    column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                    column.ColumnType = MVCxGridViewColumnType.ComboBox;
                    column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                    var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                    comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<ICurrencyService>().GetAll();
                    comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                    comboBoxProperties.AllowUserInput = true;
                    comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                    comboBoxProperties.TextField = "Name";
                    comboBoxProperties.ValueField = "ID";
                    comboBoxProperties.ValueType = typeof(long);
                });

                settings.Columns.Add(column =>
                {
                    column.FieldName = "DiscountPercent";
                    column.Caption = "Скидка";

                    column.PropertiesEdit.DisplayFormatString = "N2";

                    column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                    var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                    spinEditProperties.NumberType = SpinEditNumberType.Float;
                    spinEditProperties.DecimalPlaces = 2;
                    spinEditProperties.MinValue = 0;

                });

                settings.Columns.Add(column =>
                {
                    column.FieldName = "Bonus";
                    column.Caption = "Бонус";
                    column.PropertiesEdit.DisplayFormatString = "N2";

                    column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                    var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                    spinEditProperties.NumberType = SpinEditNumberType.Float;
                    spinEditProperties.DecimalPlaces = 2;
                    spinEditProperties.MinValue = 0;

                });
            }



            settings.Columns.Add(column =>
            {
                column.FieldName = "ID";
                column.Caption = "ID";
                column.ReadOnly = true;
                column.EditFormSettings.Visible = DefaultBoolean.False;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });





            //settings.Columns.Add(column =>
            //{
            //    column.FieldName = "CreationDate";
            //    column.Caption = "Дата создания";
            //    column.PropertiesEdit.DisplayFormatString = FormatHelpers.CreationDateFormat;
            //    column.Settings.FilterMode = ColumnFilterMode.DisplayText;
            //    column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            //});

            //settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Count, "VehicleBrandModel");

            return settings;
        }


        public virtual ActionResult GridViewAddNewPartial(PriceListElement priceListElement)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _priceListElementService.Create(priceListElement));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.CRM.PriceList.Views.PriceListElementGridViewPartial);
        }

        public virtual ActionResult GridViewUpdatePartial(PriceListElement priceListElement)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _priceListElementService.Update(priceListElement));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.CRM.PriceList.Views.PriceListElementGridViewPartial);
        }

        public virtual ActionResult GridViewDeletePartial(long id)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _priceListElementService.Delete(id));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.CRM.PriceList.Views.PriceListElementGridViewPartial);
        }


        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<PriceListElement, int> updateValues, long PriceListVersionID = 0)
        {
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    InsertProduct(product, updateValues);
            }
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                    UpdateProduct(product, updateValues, PriceListVersionID);
            }
            foreach (var productID in updateValues.DeleteKeys)
            {
                DeleteProduct(productID, updateValues);
            }
            return PartialView(MVC.CRM.PriceList.Views.PriceListElementGridViewPartial);
        }



        protected void InsertProduct(PriceListElement priceListElement, MVCxGridViewBatchUpdateValues<PriceListElement, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => {
                    var newEl = _priceListElementService.Create(priceListElement);

                    foreach(var version in _priceListVersionService.GetAll())
                    {
                        _priceListElementVersionService.Create(new PriceListElementVersion
                        {
                            PriceListElementID = newEl.ID,
                            PriceListVersionID = version.ID,
                            Details = priceListElement.Details,
                            Package = priceListElement.Package,
                            Name = priceListElement.Name,
                            UnitPrice = priceListElement.UnitPrice,
                            Price = priceListElement.Price,
                            DiscountPercent = priceListElement.DiscountPercent,
                            Bonus = priceListElement.Bonus,
                            DisplayIndex = priceListElement.DisplayIndex,
                            CurrencyID = priceListElement.CurrencyID,
                        });
                    }
                });
            }
            else
            {
                updateValues.SetErrorText(priceListElement, "Ошибка");
            }
        }
        protected void UpdateProduct(PriceListElement priceListElement, MVCxGridViewBatchUpdateValues<PriceListElement, int> updateValues, long PriceListVersionID)
        {
            try
            {
                UpdateWrapper(() => {
                    if(PriceListVersionID == 0)
                    {
                        _priceListElementService.Update(priceListElement);
                    }
                    else
                    {
                        var elVer = _priceListElementVersionService.RawDataQueryable.Where(elv => elv.PriceListElementID == priceListElement.ID && elv.PriceListVersionID == PriceListVersionID).FirstOrDefault();
                        elVer.Details = priceListElement.Details;
                        elVer.Package = priceListElement.Package;
                        elVer.Name = priceListElement.Name;
                        elVer.UnitPrice = priceListElement.UnitPrice;
                        elVer.Price = priceListElement.Price;
                        elVer.DiscountPercent = priceListElement.DiscountPercent;
                        elVer.Bonus = priceListElement.Bonus;
                        elVer.DisplayIndex = priceListElement.DisplayIndex;
                        elVer.CurrencyID = priceListElement.CurrencyID;
                        _priceListElementVersionService.Update(elVer);
                    }
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(priceListElement, "Ошибка");
            }
        }
        protected void DeleteProduct(int id, MVCxGridViewBatchUpdateValues<PriceListElement, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => {
                    var elV = _priceListElementVersionService.RawDataQueryable.Where(el => el.PriceListElementID == id).FirstOrDefault();
                    elV.PriceListElement.UniLine.Clear();
                    _priceListElementVersionService.Delete(elV.ID);
                    _priceListElementService.Delete(id);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        [HttpPost]
        public virtual JsonResult Import(string file, long PriceListVersionID = 0)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Импорт запущен"
                });
                var path = Server.MapPath("/Admin/" + file);
                _priceListExcelImportService.Import(path, mes =>
                {
                    _notificationBus.Notificate(new NotificationMessage()
                    {
                        Text = mes
                    });
                }, PriceListVersionID);
                return true;
            }, "Импорт успешно завершен");
            return null;
        }

        [HttpGet]
        public virtual JsonResult GetPriceListVersions()
        {
            var priceListVersionList = new List<PriceListVersionItem>();
            priceListVersionList.Add(new PriceListVersionItem {
                id = 0,
                text = "Источник"
            });
            priceListVersionList.AddRange(_priceListVersionService.GetAll().Select(prv => new PriceListVersionItem
            {
                id = prv.ID,
                text = prv.Name
            }));
            
            return Json(new { items = priceListVersionList , count = priceListVersionList.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetPriceListVersion(long id)
        {
            var items = new List<PriceListVersionItem>();
            if (id == 0)
            {
                items.Add(new PriceListVersionItem {
                    id = 0,
                    text = "Источник"
                });
            }
            else
            {
                var prver = _priceListVersionService.Get(id);
                if (prver == null) return null;
                items.Add(new PriceListVersionItem
                {
                    id = prver.ID,
                    text = prver.Name
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        private class PriceListVersionItem
        {
            public long id { get; set; }
            public string text { get; set; }
        }

        [HttpGet]
        public virtual JsonResult CreatePriceListVersion(string name)
        {
            long? newVersionId = null;
            OperationWrapper(() =>
            {
                if (String.IsNullOrEmpty(name))
                {
                    _notificationBus.Notificate(new NotificationMessage()
                    {
                        Text = "Не задано имя новой версии",
                        
                    }, NotificationType.Warning);
                    return false;
                }

                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Создание версии запущено"
                });

                var version = _priceListVersionService.Create(new PriceListVersion
                {
                    Name = name,
                });
                newVersionId = version.ID;

                // copy all PriceListElements to new version
                var newVersionElements = _priceListElementService.GetAll().Select( pre => new PriceListElementVersion {
                    PriceListVersionID = version.ID,
                    PriceListElementID = pre.ID,
                    Details = pre.Details,
                    Package = pre.Package,
                    Name = pre.Name,
                    UnitPrice = pre.UnitPrice,
                    Price = pre.Price,
                    DiscountPercent = pre.DiscountPercent,
                    Bonus = pre.Bonus,
                    DisplayIndex = pre.DisplayIndex,
                    CurrencyID = pre.CurrencyID
                });

                foreach(var ve in newVersionElements)
                {
                    _priceListElementVersionService.Create(ve);
                }

                return true;
            }, "Создание версии успешно завершено");
            return Json(newVersionId, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public virtual JsonResult DeletePriceListVersion(long id)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Удаление версии запущено"
                });

                var version = _priceListVersionService.Get(id);
                if(version != null)
                {
                    version.UniVersion.Clear();
                    var ids = _priceListElementVersionService.RawDataQueryable.Where(elv => elv.PriceListVersionID == id).Select(x => x.ID).ToList();
                    _priceListElementVersionService.Delete(ids);
                    _priceListVersionService.Delete(id);
                }

                return true;
            }, "Удаление версии успешно завершено");
            return null;
        }

    }


}