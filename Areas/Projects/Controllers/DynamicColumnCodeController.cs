using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Controllers;
using livemenu.Filters;
using livemenu.Kernel.Executing;
using livemenu.Models;
using DevExpress.Web.Mvc;
using System.Web.UI.WebControls;
using livemenu.Common.Kernel.DynamicTables;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.BWP.DynamicTables;
using DevExpress.Web;
using Microsoft.Practices.ServiceLocation;
using System.Web.UI;
using livemenu.Areas.Cloud.Controllers;
using livemenu.Areas.Projects.Models;
using livemenu.Kernel.CMS;
using livemenu.Kernel.Catalog;
using livemenu.Helpers;
using DevExpress.Data;
using BWP.Kernel.Notification;
using livemenu.Excel.Projects;

namespace livemenu.Areas.Projects.Controllers
{
    public partial class DynamicColumnCodeController : ProjectsController
    {
        private IDynamicColumnService _dynamicColumnService;
        private IDynamicColumnCodeService _dynamicColumnCodeService;
        private ICatalogItemService _catalogItemService;
        private readonly ICatalogItemDynamicEntityService _catalogItemDynamicEntityService;
        private readonly IDynamicColumnCodeExcelImportService _dynamicColumnCodeExcelImportService;

        public DynamicColumnCodeController(
            IDynamicColumnService dynamicColumnService,
            IDynamicColumnCodeService dynamicColumnCodeService,
            ICatalogItemService catalogItemService,
            ICatalogItemDynamicEntityService catalogItemDynamicEntityService,
            IDynamicColumnCodeExcelImportService dynamicColumnCodeExcelImportService
            )
        {
            _dynamicColumnService = dynamicColumnService;
            _dynamicColumnCodeService = dynamicColumnCodeService;
            _catalogItemService = catalogItemService;
            _catalogItemDynamicEntityService = catalogItemDynamicEntityService;
            _dynamicColumnCodeExcelImportService = dynamicColumnCodeExcelImportService;
        }

        public virtual ActionResult Index(long? appID = null)
        {
            return View(new DynamicColumnCodeResponsiveLayoutModel
            {
                CustomID = "projects-columncodes",
                Name = "Коды",
                InstructionLink = "http://www.WebUni.ru/platform-projects-database",

                ApplicationID = appID
            });
        }

        public virtual ActionResult GridViewPartial(long? appID = null, int? draggingIndex =null, int? targetIndex = null, string command = null)
        {
            if (command == "DRAGROW" && draggingIndex != null && targetIndex != null && targetIndex != draggingIndex && draggingIndex > 0)
            {
                if (targetIndex < 0) targetIndex = 1;
                _catalogItemService.UpdateDisplayIndex((long)draggingIndex, (long)targetIndex, "DynamicColumnCode");
            }
            return View(MVC.Projects.DynamicColumnCode.Views.GridViewPartial, new DynamicColumnCodeGridViewModel
            {
                ApplicationID = appID
            });
        }

        public GridViewSettings GetGridViewSettings(GridViewSettings settings)
        {
            settings.Settings.ShowFooter = true;

            settings.Name = "DynamicColumnCodeGridView";
            settings.Width = new System.Web.UI.WebControls.Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column => {
                column.FieldName = "DynamicColumnID";
                column.Caption = "Название";
            
                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = _dynamicColumnService.RawDataQueryable.Where(col => col.DynamicColumnCode.Count == 0).Select(col => new { ID = col.ID, DisplayName = col.DynamicTable.DisplayName + ": " + col.DisplayName + " - " + col.DynamicType.Name}).ToList();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "DisplayName";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
            });

            settings.Columns.Add(column => {
                column.FieldName = "Code";
                column.Caption = "Код данных";
            });

            return settings;
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<DynamicColumnCodeList, int> updateValues, long? appID = null)
        {
            foreach (var dynamicColumnCode in updateValues.Insert)
            {
                Insert(dynamicColumnCode, updateValues, appID);
            }
            foreach (var dynamicColumnCode in updateValues.Update)
            {
                Update(dynamicColumnCode, updateValues);
            }
            foreach (var dynamicColumnCodeID in updateValues.DeleteKeys)
            {
                Delete(dynamicColumnCodeID, updateValues);
            }
            return PartialView(MVC.Projects.DynamicColumnCode.Views.GridViewPartial, new DynamicColumnCodeGridViewModel
            {
                ApplicationID = appID
            });
        }

        protected void Insert(DynamicColumnCodeList property, MVCxGridViewBatchUpdateValues<DynamicColumnCodeList, int> updateValues, long? appID = null)
        {
            if (ModelState.IsValid && !string.IsNullOrEmpty(property.Code))
            {
                UpdateWrapper(() =>
                {
                    _catalogItemDynamicEntityService.ClearPageCacheByDynamicColumn(property.DynamicColumnID);
                    _dynamicColumnCodeService.Create(new DynamicColumnCode
                    {
                        ID = property.ID,
                        DynamicColumnID = property.DynamicColumnID,
                        Code = property.Code,
                        DisplayIndex = property.DisplayIndex
                    }, appID);
                });
            }
            else
            {
            //    updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Update(DynamicColumnCodeList property, MVCxGridViewBatchUpdateValues<DynamicColumnCodeList, int> updateValues)
        {
            if (!string.IsNullOrEmpty(property.Code))
            {
                try
                {
                    UpdateWrapper(() =>
                    {
                        _dynamicColumnCodeService.Update(new DynamicColumnCode
                        {
                            ID = property.ID,
                            DynamicColumnID = property.DynamicColumnID,
                            Code = property.Code,
                            DisplayIndex = property.DisplayIndex
                        });
                        _catalogItemDynamicEntityService.ClearPageCacheByDynamicColumn(property.DynamicColumnID);
                    });
                }
                catch (Exception e)
                {
                    updateValues.SetErrorText(property, "Ошибка");
                }
            }
            else updateValues.SetErrorText(property, "Ошибка");
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<DynamicColumnCodeList, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _catalogItemDynamicEntityService.ClearPageCacheByDynamicColumnCode(id);
                    _dynamicColumnCodeService.Delete(id);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        public virtual ActionResult ExportTo(long? applicationID)
        {
            var settings = new GridViewSettings();

            settings.Name = "DynamicColumnCodeGridView";
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.Width = new System.Web.UI.WebControls.Unit(4, UnitType.Pixel);
                column.Name = "DisplayIndex";
                column.FieldName = "DisplayIndex";
                column.Caption = "№";
                column.SortIndex = 1;
                column.SortOrder = ColumnSortOrder.Ascending;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "DynamicColumn.DynamicTable.Code";
                column.Caption = "Код таблицы";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "DynamicColumn.DisplayName";
                column.Caption = "Название";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Code";
                column.Caption = "Код данных";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ID";
                column.Caption = "ID";
            });

          //  var items = applicationID.HasValue  ? _dynamicColumnCodeService.RawDataQueryable().Where(x => x.ApplicationID == applicationID) : _dynamicColumnCodeService.GetAll();
            
            string fileName = "Коды_база данных";
            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(settings, _dynamicColumnCodeService.GetAll(), fileName);
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(settings, _dynamicColumnCodeService.GetAll(), fileName);
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
                _dynamicColumnCodeExcelImportService.Import(
                    path,
                    (mes, type) =>
                    {
                        _notificationBus.Notificate(new NotificationMessage()
                        {
                            Text = mes
                        }, type);
                    },
                    (progress) =>
                    {
                        _notificationBus.NotificateAboutProgress(progress);
                    });
                return true;
            }, "Импорт успешно завершен");
            return null;
        }
    }
}