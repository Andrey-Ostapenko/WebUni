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
using livemenu.Areas.CMS.Controllers;
using BWP.Kernel.Notification;
using livemenu.Common.Entities;
using System.ComponentModel.DataAnnotations;
using DynamicClassGenerator;
using System.ComponentModel;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.CMS;
using DevExpress.Data;
using System.Web.UI;
using livemenu.Areas.Cloud.Controllers;
using livemenu.Areas.Projects.Models;
using livemenu.Helpers;
using System.Data.Entity;
using livemenu.Excel.Projects;
using System.Reflection;
using WebUni.Bus.Admin;

namespace livemenu.Areas.Projects.Controllers
{
    public partial class DynamicTablesController : ProjectsController
    {
        private IDynamicTableService _dynamicTableService;
        private IDynamicTableVersionService _dynamicTableVersionService;
        private IDynamicColumnService _dynamicColumnService;
        private IDynamicTypeService _dynamicTypeService;
        private IDynamicTablesService _dynamicTablesService;
        private ICatalogItemService _catalogItemService;
        private IDynamicTablesExcelImportService _dynamicTablesExcelImportService;
        private readonly ICatalogItemDynamicEntityService _catalogItemDynamicEntityService;
        private readonly IAdminClientBus _adminClientBus;

        public DynamicTablesController(
            IDynamicTableService dynamicTableService,
            IDynamicTableVersionService dynamicTableVersionService,
            IDynamicColumnService dynamicColumnService,
            IDynamicTypeService dynamicTypeService,
            IDynamicTablesService dynamicTablesService,
            ICatalogItemService catalogItemService,
            IDynamicTablesExcelImportService dynamicTablesExcelImportService,
            ICatalogItemDynamicEntityService catalogItemDynamicEntityService,
            IAdminClientBus adminClientBus
            )
        {
            _dynamicTableService = dynamicTableService;
            _dynamicTableVersionService = dynamicTableVersionService;
            _dynamicColumnService = dynamicColumnService;
            _dynamicTypeService = dynamicTypeService;
            _dynamicTablesService = dynamicTablesService;
            _catalogItemService = catalogItemService;
            _dynamicTablesExcelImportService = dynamicTablesExcelImportService;
            _catalogItemDynamicEntityService = catalogItemDynamicEntityService;
            _adminClientBus = adminClientBus;
        }

        public virtual ActionResult Index(long? appID)
        {
            return View(new DynamicTableResponsiveLayoutModel
            {
                CustomID = "projects-dynamictables",
                Name = "База данных",
                InstructionLink = "http://www.WebUni.ru/platform-projects-database",

                ApplicationID = appID
            });
        }

        private long? GetDisplayIndexOfDynamicEntity(dynamic entity)
        {
            return entity.DisplayIndex;
            //return (long?)entity.GetType().GetProperty("DisplayIndex").GetValue(entity);
        }

        public virtual ActionResult GridViewPartial(long? appID = null, long? dynamicTableID = null, bool? editstructure = null, int? focusedRowIndex = null, int? draggingIndex = null, int? targetIndex = null, string command = null)
        {
            // dynamic tables
            var qw = _catalogItemService.RawDataQueryable;
            if (dynamicTableID == null) {
                if (command == "DRAGROW" && draggingIndex != null && targetIndex != null && targetIndex != draggingIndex)
                {
                    _catalogItemService.UpdateDisplayIndex((long)draggingIndex, (long)targetIndex, "DynamicTable");
                    foreach(var table in _dynamicTableService.GetAll())
                    {
                        _dynamicTableService.Reload(table);
                    }
                }
                return View(MVC.Projects.DynamicTables.Views.GridViewPartial, new DynamicTableGridViewModel
                {
                    ApplicationID = appID
                });
            }

            // dynamic columns
            var model = _dynamicTableService.Get((long)dynamicTableID);
            if(editstructure != null && (bool)editstructure)
            {
                if (command == "DRAGROW" && draggingIndex != null && targetIndex != null && targetIndex != draggingIndex)
                {
                    var draggingColumn = _dynamicColumnService.RawDataQueryable.Where(q => q.DynamicTableID == dynamicTableID && q.DisplayIndex == draggingIndex).FirstOrDefault();
                    List<DynamicColumn> editedColumns;
                    if (draggingIndex < targetIndex)
                    {
                        editedColumns = _dynamicColumnService.RawDataQueryable.Where(q => q.DynamicTableID == dynamicTableID && q.DisplayIndex > draggingIndex && q.DisplayIndex <= targetIndex).ToList();
                    } else
                    {
                        editedColumns = _dynamicColumnService.RawDataQueryable.Where(q => q.DynamicTableID == dynamicTableID && q.DisplayIndex >= targetIndex && q.DisplayIndex < draggingIndex).ToList();
                    }
                    foreach (var column in editedColumns)
                    {
                        column.DisplayIndex = column.DisplayIndex + (draggingIndex < targetIndex ? -1 : 1);
                        _dynamicColumnService.Update(column);
                    }
                    draggingColumn.DisplayIndex = targetIndex;
                    _dynamicColumnService.Update(draggingColumn);
                }

                return View(MVC.Projects.DynamicTables.Views.GridViewEditTableStructurePartial, new DynamicTableStructureGridViewModel
                {
                    ApplicationID = appID,
                    Table = model
                });
            }

            // data of dynamic table
            if (command == "DRAGROW" && draggingIndex != null && targetIndex != null && targetIndex != draggingIndex)
            {
                var tableName = _dynamicTablesService.GetTableName((long)dynamicTableID);
                _catalogItemService.UpdateDisplayIndex((long)draggingIndex, (long)targetIndex, tableName);
            }
            return View(MVC.Projects.DynamicTables.Views.GridViewEditTablePartial, new DynamicTableStructureGridViewModel
            {
                ApplicationID = appID,
                Table = model
            });
        }

        public GridViewSettings GetGridViewSettings(bool isSelector = false, bool showID = false) // settings of gridview of tables
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;

            settings.Name = "DynamicTablesGridView";
            settings.Width = new System.Web.UI.WebControls.Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.Width = new System.Web.UI.WebControls.Unit(4, UnitType.Pixel);
                column.FieldName = "DisplayIndex";
                column.Caption = "№";
                column.SortIndex = 1;
                column.SortOrder = ColumnSortOrder.Ascending;
                column.ReadOnly = true;
            });

            settings.Columns.Add(column => {
                column.FieldName = "Group";
                column.Caption = "Группа";
            });

            settings.Columns.Add(column => {
                column.FieldName = "DisplayName";
                column.Caption = "Название";
            });

            settings.Columns.Add(column => {
                column.FieldName = "Description";
                column.Caption = "Описание";
            });

            return settings;
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues < DynamicTable, int> updateValues, long? appID = null) // devexpress table of tables
        {
            foreach (var dynamicTable in updateValues.Insert)
            {
                if (updateValues.IsValid(dynamicTable))
                    Insert(dynamicTable, updateValues, appID);
            }
            foreach (var dynamicTable in updateValues.Update)
            {
                if (updateValues.IsValid(dynamicTable))
                    Update(dynamicTable, updateValues);
            }
            foreach (var dynamicTableID in updateValues.DeleteKeys)
            {
                Delete(dynamicTableID, updateValues);
            }
            return PartialView(MVC.Projects.DynamicTables.Views.GridViewPartial, new DynamicTableGridViewModel
            {
                ApplicationID = appID
            });
        }

        protected void Insert(DynamicTable property, MVCxGridViewBatchUpdateValues<DynamicTable, int> updateValues, long? appID = null) // create table
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                   var id = _dynamicTableService.Create(property, appID).ID;
                    _dynamicTablesService.CreateTable(id);
                    _dynamicTablesService.CreateTableVersion(id);
                   
                    _adminClientBus.ClearDynamicTables();
                });
            }
            else
            {
            //    updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Update(DynamicTable property, MVCxGridViewBatchUpdateValues<DynamicTable, int> updateValues, long? appID = null) // update table
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    _dynamicTableService.Update(property, appID);
                    _adminClientBus.ClearDynamicTables();
                });
            }
            else
            {
              //  updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<DynamicTable, int> updateValues) // delete table
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _catalogItemDynamicEntityService.DeleteCatalogItemDynamicEntity(id);
                    _dynamicTableService.Delete(id);
                    _adminClientBus.ClearDynamicTables();
                });
            }
            catch (Exception e)
            {
              //  updateValues.SetErrorText(id, e.Message);
            }
        }


        public GridViewSettings GetGridViewEditTableStructureSettings() // settings of gridview of table structure
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;

            settings.Name = "DynamicTablesGridView";
            settings.Width = new System.Web.UI.WebControls.Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.Width = new System.Web.UI.WebControls.Unit(4, UnitType.Pixel);
                column.FieldName = "DisplayIndex";
                column.Name = "DisplayIndex";
                column.Caption = "№";
                column.SortIndex = 1;
                column.SortOrder = ColumnSortOrder.Ascending;
                column.ReadOnly = true;
                column.CellStyle.CssClass = "draggable-dynamicColumns";
            });

            settings.Columns.Add(column => {
                column.FieldName = "DisplayName";
                column.Caption = "Название";
            });

            settings.Columns.Add(column => {
                column.FieldName = "DynamicTypeID";
                column.Caption = "Тип";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = _dynamicTypeService.GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Name";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
            });

            settings.Columns.Add(column => {
                column.FieldName = "IsNotNullable";
                column.Caption = "Обязательное поле";

                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            return settings;
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewEditTableStructureBatchUpdatePartial(long? appID, long dynamicTableID, MVCxGridViewBatchUpdateValues<DynamicColumn, int> updateValues) // devexpress table of columns
        {
            foreach (var dynamicColumn in updateValues.Insert)
            {
                dynamicColumn.DynamicTableID = dynamicTableID;
                if (updateValues.IsValid(dynamicColumn))
                    InsertColumn(dynamicColumn, updateValues);
            }
            foreach (var dynamicColumn in updateValues.Update)
            {
                dynamicColumn.DynamicTableID = dynamicTableID;
                if (updateValues.IsValid(dynamicColumn))
                    UpdateColumn(dynamicColumn, updateValues);
            }
            foreach (var dynamicColumnID in updateValues.DeleteKeys)
            {
                DeleteColumn(dynamicColumnID, updateValues);
            }

            var model = _dynamicTableService.Get((long)dynamicTableID);
            return View(MVC.Projects.DynamicTables.Views.GridViewEditTableStructurePartial, new DynamicTableStructureGridViewModel
            {
                ApplicationID = appID,
                Table = model
            });
        }

        protected void InsertColumn(DynamicColumn property, MVCxGridViewBatchUpdateValues<DynamicColumn, int> updateValues) // create column
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    _dynamicColumnService.Create(property);
                });
            }
            else
            {
                //updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void UpdateColumn(DynamicColumn property, MVCxGridViewBatchUpdateValues<DynamicColumn, int> updateValues) // update column
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    _dynamicColumnService.Update(property);
                });
            }
            else
            {
                //updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void DeleteColumn(int id, MVCxGridViewBatchUpdateValues<DynamicColumn, int> updateValues) // delete column
        {
            try
            {
                UpdateWrapper(() =>
                {
                    _catalogItemDynamicEntityService.ClearPageCacheByDynamicColumn(id);
                    _dynamicColumnService.Delete(id);
                });
            }
            catch (Exception e)
            {
                //updateValues.SetErrorText(id, e.Message);
            }
        }


        public GridViewSettings GetGridViewEditTableSettings(DynamicTable table)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;

            settings.Name = "DynamicTablesGridView";
            settings.Width = new System.Web.UI.WebControls.Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";
            
            settings.Columns.Add(column =>
            {
                column.Width = new System.Web.UI.WebControls.Unit(4, UnitType.Pixel);
                column.FieldName = "DisplayIndex";
                column.Name = "DisplayIndex";
                column.Caption = "№";
                column.SortIndex = 1;
                column.SortOrder = ColumnSortOrder.Ascending;
                column.ReadOnly = true;
                column.CellStyle.CssClass = "draggable-dynamicEntities";
            });

            foreach (var dynCol in table.DynamicColumn.OrderBy(x => x.DisplayIndex))
            {
                settings.Columns.Add(column => {
                    column.FieldName = dynCol.Name;
                    column.Caption = dynCol.DisplayName;
                    column.EditCellStyle.CssClass += " type-" + dynCol.DynamicTypeID+" ";
                    switch (dynCol.DynamicTypeID)
                    {
                        case (long)DynamicTypeValue.Decimal:
                            column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                            (column.PropertiesEdit as SpinEditProperties).DecimalPlaces = 4;
                            (column.PropertiesEdit as SpinEditProperties).NumberType = SpinEditNumberType.Float;
                            break;
                        case (long)DynamicTypeValue.Int:
                            column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                            (column.PropertiesEdit as SpinEditProperties).NumberType = SpinEditNumberType.Integer;
                            break;
                        case (long)DynamicTypeValue.Formula:
                        {
                                column.CellStyle.CssClass = "formula-cell";
                                column.ColumnType = MVCxGridViewColumnType.Memo;

                                ((MemoProperties)column.PropertiesEdit).Rows = 4;
                                break;
                            }
                        case (long)DynamicTypeValue.Bool:
                            column.ColumnType = MVCxGridViewColumnType.CheckBox;
                           // column.CellStyle.CssClass = "bool-cell";
                            ((CheckBoxProperties)column.PropertiesEdit).ValueUnchecked = false;


                            //column.ColumnType = MVCxGridViewColumnType.ComboBox;
                            //column.Settings.FilterMode = ColumnFilterMode.DisplayText;

                            //var combo = column.PropertiesEdit as ComboBoxProperties;
                            //combo.Items.Add("Истина", true);
                            //combo.Items.Add("Ложь", false);
                            //combo.Items.Add("Не определено", null);
                            break;
                        case (long)DynamicTypeValue.Time:
                            column.ColumnType = MVCxGridViewColumnType.TimeEdit;
                            ((TimeEditProperties)column.PropertiesEdit).DisplayFormatString = "hh:mm";
                            ((TimeEditProperties)column.PropertiesEdit).EditFormat = EditFormat.Time;
                            
                            break;
                        case (long)DynamicTypeValue.Date:
                            column.ColumnType = MVCxGridViewColumnType.DateEdit;
                            
                            ((DateEditProperties) column.PropertiesEdit).UseMaskBehavior = true;
                            ((DateEditProperties) column.PropertiesEdit).EditFormat = EditFormat.Date;
                            ((DateEditProperties) column.PropertiesEdit).EditFormatString = "dd.MM.yyyy";
                            ((DateEditProperties)column.PropertiesEdit).TimeSectionProperties.Visible = false;
                            ;
                            break;
                        case (long)DynamicTypeValue.File:
                        {
                                column.ColumnType = MVCxGridViewColumnType.Memo;

                                break;
                        }
                        case (long)DynamicTypeValue.Image:
                            {
                                column.ColumnType = MVCxGridViewColumnType.Memo;

                                break;
                            }
                       
                        case (long)DynamicTypeValue.String:
                            column.ColumnType = MVCxGridViewColumnType.Memo;
                          
                            ((MemoProperties)column.PropertiesEdit).Rows = 4;
                            break;
                    }
                });
            }
            settings.Columns.Add(column =>
            {
                column.Width = new System.Web.UI.WebControls.Unit(4, UnitType.Pixel);
              //  column.FieldName = "ID";
                column.Name = "ID";
                column.Caption = "ID";
              //  column.EditFormSettings.
                column.ReadOnly = true;
              
            });
            return settings;
        }


        [ValidateInput(false)]
        public virtual ActionResult GridViewEditTableBatchUpdatePartial(long dynamicTableID, long? dynamicTableVersionID, [ModelBinder(typeof(DynamicTypeModelBinder))] MVCxGridViewBatchUpdateValues<BaseDynamicEntity, int> updateValues, long? appID = null)
        {
            foreach (var obj in updateValues.Insert)
            {
                //if (updateValues.IsValid(obj))
                    InsertObj(obj, updateValues, dynamicTableVersionID);
            }
            foreach (var obj in updateValues.Update)
            {
                //if (updateValues.IsValid(obj))
                    UpdateObj(obj, updateValues, dynamicTableVersionID);
            }
            foreach (var objID in updateValues.DeleteKeys)
            {
                DeleteObj(objID, dynamicTableID, updateValues, dynamicTableVersionID);
            }

            var model = _dynamicTableService.Get((long)dynamicTableID);

            return View(MVC.Projects.DynamicTables.Views.GridViewEditTablePartial, new DynamicTableStructureGridViewModel
            {
                ApplicationID = appID,
                Table = model
            });
        }

        protected void InsertObj(BaseDynamicEntity property, MVCxGridViewBatchUpdateValues<BaseDynamicEntity, int> updateValues, long? dynamicTableVersionID)
        {
            if(ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    property = _dynamicTablesService.Create(property);
                    // add to versions
                    foreach(var version in _dynamicTableVersionService.GetVersionsByTableID((long)_dynamicTablesService.GetTableIDByType(property.GetType())))
                    {
                        _dynamicTablesService.CreateEntityVersion(version.ID, property);
                    }

                    _adminClientBus.ClearDynamicTables();
                });
            }
            else
            {
                //updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void UpdateObj(BaseDynamicEntity property, MVCxGridViewBatchUpdateValues<BaseDynamicEntity, int> updateValues, long? dynamicTableVersionID)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                    if(dynamicTableVersionID == null || dynamicTableVersionID == 0)
                    {
                        _dynamicTablesService.Update(property);

                        var typeTable = property.GetType();
                        var tableId = _dynamicTablesService.GetTableIDByType(typeTable);
                        var bde = property;
                        if (tableId.HasValue && bde != null)
                        {
                            _catalogItemDynamicEntityService.ClearPageCacheByDynamicEntity(tableId.Value, bde.ID);
                        }
                       
                    }
                    else
                    {
                        var entityVersion = _dynamicTablesService.UpdateEntityVersion((long)dynamicTableVersionID, property);

                        var typeTable = property.GetType();
                        var tableId = _dynamicTablesService.GetTableIDByType(typeTable);
                        var bde = property;
                        if (tableId.HasValue && bde != null)
                        {
                            _catalogItemDynamicEntityService.ClearPageCacheByDynamicEntity(tableId.Value, entityVersion.DynamicTableEntityID, (long)dynamicTableVersionID);
                        }
                    }

                    _adminClientBus.ClearDynamicTables();

                });
            }
            else
            {
                //updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void DeleteObj(int id, long dynamicTableID, MVCxGridViewBatchUpdateValues<BaseDynamicEntity, int> updateValues, long? dynamicTableVersionID)
        {
            try
            {
                if(dynamicTableVersionID == null || dynamicTableVersionID == 0)
                {
                    _catalogItemDynamicEntityService.DeleteCatalogItemDynamicEntity(dynamicTableID, id);
                    _dynamicTablesService.Delete(dynamicTableID, id);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Изменения успешно сохранены" });
                }
                else
                {
                    var deletedEntityID = _dynamicTablesService.DeleteByVersionEntityID(dynamicTableID, id);
                    if(deletedEntityID.HasValue)
                    {
                        _catalogItemDynamicEntityService.DeleteCatalogItemDynamicEntity(dynamicTableID, (long)deletedEntityID);
                    }
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Изменения успешно сохранены" });
                }

                _adminClientBus.ClearDynamicTables();
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

        public class DynamicTypeModelBinder : DefaultModelBinder
        {
            public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
            {
                long tableID = 0;
                long.TryParse(controllerContext.HttpContext.Request.Params["dynamicTableID"], out tableID);
                var tableType = ServiceLocator.Current.GetInstance<IDynamicTablesService>().GetTableType(tableID);
                var modelType = (typeof(MVCxGridViewBatchUpdateValues<,>)).MakeGenericType(new Type[] { tableType, typeof(int) });

                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, modelType);
                var binder = new DevExpress.Web.Mvc.DevExpressEditorsBinder();
                dynamic resultValues = binder.BindModel(controllerContext, bindingContext);

                var result = new MVCxGridViewBatchUpdateValues<BaseDynamicEntity, int>();
                result.Insert.AddRange(resultValues.Insert);
                result.Update.AddRange(resultValues.Update);
                result.DeleteKeys.AddRange(resultValues.DeleteKeys);

                // validation
                var columns = ServiceLocator.Current.GetInstance<IDynamicTablesService>().GetColumnsByType(tableType);
                foreach(var column in columns.Where(c => c.IsNotNullable))
                {
                    var i = 0;
                    foreach(var entity in result.Insert)
                    {
                        var dynamicEntityValue = entity.GetType().GetProperty(column.Name).GetValue(entity);
                        if (dynamicEntityValue == null || (dynamicEntityValue.GetType().FullName == "System.String" && String.IsNullOrEmpty((string)dynamicEntityValue)))
                        {
                            bindingContext.ModelState.AddModelError("Insert["+i.ToString()+"]." + column.Name, "Поле обязательно для заполнения");
                        }
                        i++;
                    }
                    i = 0;
                    foreach (var entity in result.Update)
                    {
                        var dynamicEntityValue = entity.GetType().GetProperty(column.Name).GetValue(entity);
                        if (dynamicEntityValue == null || (dynamicEntityValue.GetType().FullName == "System.String" && String.IsNullOrEmpty((string)dynamicEntityValue)))
                        {
                            bindingContext.ModelState.AddModelError("Update[" + i.ToString() + "]." + column.Name, "Поле обязательно для заполнения");
                        }
                        i++;
                    }
                }


                return result;
            }
        }

        [HttpGet]
        public virtual JsonResult GetTablesList()
        {
            var result = _dynamicTableService.RawDataQueryable.OrderBy(t => t.DisplayIndex).Select(t => new
            {
                ID = t.ID,
                DisplayName = t.DisplayName
            }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult DynamicEntitySelectorGridViewPartial(long dynamicTableID, long? dynamicTableVersionID, bool isSingle = false)
        {
            var model = _dynamicTableService.Get((long)dynamicTableID);
            ViewBag.IsSingle = isSingle;
            return View(model);
        }

        public virtual ActionResult DynamicColumnSelectorGridViewPartial(long dynamicTableID)
        {
            var model = _dynamicTableService.Get((long)dynamicTableID);
            return View(model);
        }


        [HttpGet]
        public virtual JsonResult GetDynamicEntityDescription(string ids)
        {
            var result = new List<SelectItem>();

            string[] idList = ids.Split(new char[] { ',' });

            foreach (var s in idList)
            {
                if(string.IsNullOrEmpty(s))
                    continue;
                var splt = s.Split('_');
                var tableID = long.Parse(splt[0]);
                var id = long.Parse(splt[1]);
                long versionID = 0;
                if (splt.Length > 2)
                {
                    long.TryParse(splt[2], out versionID);
                }
              

                var versionName = string.Empty;
                if(versionID != 0)
                {
                    versionName = " (" + _dynamicTableVersionService.Get(versionID).Name + ")";
                }


                dynamic entity = versionID == 0 ? _dynamicTablesService.Get(tableID, id) : _dynamicTablesService.Get(tableID, id, versionID);
                string description = string.Empty;
                var overview = string.Empty;
                if (entity != null)
                {
                    description = _dynamicTablesService.GetTableDisplayName(tableID) + versionName + ": ";
                    var columns = _dynamicTablesService.GetColumnsByTableID(tableID);
                    foreach (var column in columns)
                    {
                        var value = entity.GetType().GetProperty(column.Name).GetValue(entity);
                        description += (value != null ? value.ToString() : "") + "; ";
                        var code = (column.DynamicColumnCode != null ? column.DynamicColumnCode.FirstOrDefault() : null);
                        var codeName = code != null ? code.Code : null;
                        overview += "<div class=\"form-group WebUni-form-group WebUni-form-group-input WebUni-form-group-ple-info\"><div class=\"col-sm-12 \"><label><span class=\"WebUni-form-group-ple-info-name\">" + column.DisplayName + ": </span><span class=\"WebUni-form-group-ple-info-value\">" + (value != null ? value.ToString() : "") + " </span><span class=\"WebUni-form-group-ple-info-value\">" + (codeName != null ? "{{" + codeName + "}}" : "") + "</span></label></div></div>";
                    }

                    description += ("#" + id);
                }

                result.Add(new SelectItem
                {
                    id = s,
                    text = description,
                    overview = overview
                });

            }

            
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDynamicEntityWithColumnDescription(string ids, long columnID)
        {
            var result = new List<SelectItem>();

            string[] idList = ids.Split(new char[] { ',' });

            foreach (var s in idList)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                var splt = s.Split('_');
                var tableID = long.Parse(splt[0]);
                var id = long.Parse(splt[1]);


                dynamic entity = _dynamicTablesService.Get(tableID, id);
                string description = string.Empty;
                var overview = string.Empty;
                if (entity != null)
                {
                    description = _dynamicTablesService.GetTableDisplayName(tableID) + ": ";
                    var columns = _dynamicTablesService.GetColumnsByTableID(tableID);
                    foreach (var column in columns.Where(x=>x.ID == columnID))
                    {
                        var value = entity.GetType().GetProperty(column.Name).GetValue(entity);
                        description += (value != null ? value.ToString() : "") + "";
                     //   var code = (column.DynamicColumnCode != null ? column.DynamicColumnCode.FirstOrDefault() : null);
                    //    var codeName = code != null ? code.Code : null;
                    //    overview += "<div class=\"form-group WebUni-form-group WebUni-form-group-input WebUni-form-group-ple-info\"><div class=\"col-sm-12 \"><label><span class=\"WebUni-form-group-ple-info-name\">" + column.DisplayName + ": </span><span class=\"WebUni-form-group-ple-info-value\">" + (value != null ? value.ToString() : "") + " </span><span class=\"WebUni-form-group-ple-info-value\">" + (codeName != null ? "{{" + codeName + "}}" : "") + "</span></label></div></div>";
                    }

                    description += ("#" + id);
                }

                result.Add(new SelectItem
                {
                    id = s,
                    text = description,
                    overview = overview
                });

            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDynamicEntityFullDescription(string ids)
        {
            var result = new List<SelectItem>();

            string[] idList = ids.Split(new char[] { ',' });

            foreach (var s in idList)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
                var splt = s.Split('_');
                var tableID = long.Parse(splt[0]);
                var colid = long.Parse(splt[2]);
                var id = long.Parse(splt[1]);
              


                dynamic entity = _dynamicTablesService.Get(tableID, id);
                string description = string.Empty;
                var overview = string.Empty;
                if (entity != null)
                {
                    description = _dynamicTablesService.GetTableDisplayName(tableID) + ": ";
                    var columns = _dynamicTablesService.GetColumnsByTableID(tableID);
                    foreach (var column in columns.Where(x => x.ID == colid))
                    {
                        var value = entity.GetType().GetProperty(column.Name).GetValue(entity);
                        description += (value != null ? value.ToString() : "") + "";
                        //   var code = (column.DynamicColumnCode != null ? column.DynamicColumnCode.FirstOrDefault() : null);
                        //    var codeName = code != null ? code.Code : null;
                        //    overview += "<div class=\"form-group WebUni-form-group WebUni-form-group-input WebUni-form-group-ple-info\"><div class=\"col-sm-12 \"><label><span class=\"WebUni-form-group-ple-info-name\">" + column.DisplayName + ": </span><span class=\"WebUni-form-group-ple-info-value\">" + (value != null ? value.ToString() : "") + " </span><span class=\"WebUni-form-group-ple-info-value\">" + (codeName != null ? "{{" + codeName + "}}" : "") + "</span></label></div></div>";
                    }

                    description += ("#" + id);
                }

                result.Add(new SelectItem
                {
                    id = s,
                    text = description,
                    overview = overview
                });

            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDynamicColumnDescription(string ids)
        {
            var result = new List<SelectItem>();

            string[] idList = ids.Split(new char[] { ',' });

            foreach (var s in idList)
            {
                if (string.IsNullOrEmpty(s))
                    continue;
          
                var id = long.Parse(s);


                DynamicColumn column = _dynamicColumnService.Get(id);
                string description = string.Empty;
                var overview = string.Empty;
                if (column != null)
                {
                    description = _dynamicTablesService.GetTableDisplayName(column.DynamicTableID) + ": ";
                  //  var columns = _dynamicTablesService.GetColumnsByTableID(tableID);
                   
                     //   var value = entity.GetType().GetProperty(column.Name).GetValue(entity);
                   //     description += (value != null ? value.ToString() : "") + "; ";
                   //     var code = (column.DynamicColumnCode != null ? column.DynamicColumnCode.FirstOrDefault() : null);
                  //      var codeName = code != null ? code.Code : null;
                      //  overview += "<div class=\"form-group WebUni-form-group WebUni-form-group-input WebUni-form-group-ple-info\"><div class=\"col-sm-12 \"><label><span class=\"WebUni-form-group-ple-info-name\">" + column.DisplayName + ": </span><span class=\"WebUni-form-group-ple-info-value\"> </span><span class=\"WebUni-form-group-ple-info-value\"></span></label></div></div>";
                    

                    description += column.DisplayName;
                }

                result.Add(new SelectItem
                {
                    id = s,
                    text = description,
                    overview = overview
                });

            }



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GetDynamicEntitiesOverview(string ids)
        {
            var result = new List<DynamicEntityTableRecordModel>();

            string[] idList = ids.Split(new char[] { ',' });



            foreach (var s in idList)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                var splt = s.Split('_');
                int index = 0;
                var tableID = long.Parse(splt[index]);

                index = 1;
                var id = long.Parse(splt[index]);

                long versionID = 0;
                if (splt.Length > 2)
                {
                    long.TryParse(splt[2], out versionID);
                }

                var versionName = string.Empty;
                if (versionID != 0)
                {
                    versionName = _dynamicTableVersionService.Get(versionID).Name;
                }

                dynamic entity = versionID == 0 ? _dynamicTablesService.Get(tableID, id) : _dynamicTablesService.Get(tableID, id, versionID);
                //   string description = string.Empty;
                var overview = string.Empty;
                if (entity != null)
                {
                    // description = _dynamicTablesService.GetTableDisplayName(tableID) + ": ";
                    var columns = _dynamicTablesService.GetColumnsByTableID(tableID);

                    var record = new DynamicEntityTableRecordModel()
                    {
                        EntityID = id, // because version Entity have another ID
                        Entity = entity,
                        TableDisplayName = _dynamicTablesService.GetTableDisplayName(tableID),
                        TableVersionName = versionName,
                        Columns = columns.ToList()
                    };
                    result.Add(record);
                    //  foreach (var column in columns)
                    //  {


                    //      var value = entity.GetType().GetProperty(column.Name).GetValue(entity);
                    ////      description += (value != null ? value.ToString() : "") + "; ";
                    //      var code = (column.DynamicColumnCode != null ? column.DynamicColumnCode.FirstOrDefault() : null);
                    //      var codeName = code != null ? code.Code : null;
                    //      overview += "<div class=\"form-group WebUni-form-group WebUni-form-group-input WebUni-form-group-ple-info\"><div class=\"col-sm-12 \"><label><span class=\"WebUni-form-group-ple-info-name\">" + column.DisplayName + ": </span><span class=\"WebUni-form-group-ple-info-value\">" + (value != null ? value.ToString() : "") + " </span><span class=\"WebUni-form-group-ple-info-value\">" + (codeName != null ? "{{" + codeName + "}}" : "") + "</span></label></div></div>";

                    //      record.Columns.ad
                    //  }

                    //      description += ("#" + id);
                }



            }

            return View(result);
        }



        public class SelectItem
        {
            public string id { get; set; }
            public string text { get; set; }
            public string overview { get; set; }
        }


        public virtual ActionResult ExportTo(long EditingDynamicTableID, long? DynamicTableVersionID)
        {
            string fileName = _dynamicTablesService.GetTableDisplayName(EditingDynamicTableID);
            var table = _dynamicTableService.Get(EditingDynamicTableID);

            var settings = GetGridViewEditTableSettings(table);
            settings.Columns.Add(column =>
            {
                column.FieldName = "ID";
                column.Name = "ID";
                column.Caption = "ID";
            });

            object data = new object();
            if (DynamicTableVersionID == null || DynamicTableVersionID == 0)
            {
                var dbSet = _dynamicTablesService.RawDataQueryable(EditingDynamicTableID);
                dbSet.Load();
                data = dbSet.Local;
            }
            else
            {
                var dynamicTableVersion = _dynamicTableVersionService.Get((long)DynamicTableVersionID);
                fileName += "_" + dynamicTableVersion.Name;

                //data = _dynamicTablesService.WhereEqual(EditingDynamicTableID, "DynamicTableVersionID", (long)DynamicTableVersionID, true);
                var items = _dynamicTablesService.GetAllByTableIDVersionID(EditingDynamicTableID, (long)DynamicTableVersionID);

                var toListMethod = typeof(System.Linq.Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(mi => mi.Name == "ToList").SingleOrDefault();
                var constructedToList = toListMethod.MakeGenericMethod(_dynamicTablesService.GetTableDTOType(EditingDynamicTableID));
                data = constructedToList.Invoke(null, new object[] { items });
            }

            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(settings, data, fileName);
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(settings, data, fileName);
        }

        [HttpPost]
        public virtual JsonResult Import(long dynamicTableID, string file, long? DynamicTableVersionID)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Импорт запущен"
                });
                var path = Server.MapPath("/Admin/" + file);
                _dynamicTablesExcelImportService.Import(
                    dynamicTableID,
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
                    }
                    , DynamicTableVersionID);
                return true;
            }, "Импорт успешно завершен");
            return null;
        }

        [HttpGet]
        public virtual JsonResult GetDynamicTableVersions(long tableID)
        {
            var dynamicTableVersionList = new List<DynamicTableVersionItem>();
            dynamicTableVersionList.Add(new DynamicTableVersionItem
            {
                id = 0,
                text = "Источник"
            });
            dynamicTableVersionList.AddRange(_dynamicTableVersionService.RawDataQueryable.Where(v => v.DynamicTableID == tableID).Select(dtv => new DynamicTableVersionItem
            {
                id = dtv.ID,
                text = dtv.Name
            }));

            return Json(new { items = dynamicTableVersionList, count = dynamicTableVersionList.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetDynamicTableVersion(long id)
        {
            var items = new List<DynamicTableVersionItem>();
            if (id == 0)
            {
                items.Add(new DynamicTableVersionItem
                {
                    id = 0,
                    text = "Источник"
                });
            }
            else
            {
                var dtver = _dynamicTableVersionService.Get(id);
                if (dtver == null) return null;
                items.Add(new DynamicTableVersionItem
                {
                    id = dtver.ID,
                    text = dtver.Name
                });
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        private class DynamicTableVersionItem
        {
            public long id { get; set; }
            public string text { get; set; }
        }

        [HttpGet]
        public virtual JsonResult CreateDynamicTableVersion(long tableID, string name)
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
            
                var version = _dynamicTableVersionService.Create(new DynamicTableVersion
                {
                    DynamicTableID = tableID,
                    Name = name,
                });
                _dynamicTablesService.CreateVersionEntities(
                    version.DynamicTableID,
                    version.ID,
                    (progressInPercent) =>
                    {
                        _notificationBus.NotificateAboutProgress(progressInPercent);
                    }
                );
                _adminClientBus.ClearDynamicTables();
                newVersionId = version.ID;
            
                return true;
            }, "Создание версии успешно завершено");
            return Json(newVersionId, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public virtual JsonResult DeleteDynamicTableVersion(long id)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Удаление версии запущено"
                });
            
                var version = _dynamicTableVersionService.Get(id);
                if (version != null)
                {
                    _catalogItemDynamicEntityService.DeleteCatalogItemDynamicEntityByVersionID(id);
                    _dynamicTableVersionService.Delete(id);
                    _adminClientBus.ClearDynamicTables();
                }
            
                return true;
            }, "Удаление версии успешно завершено");
            return null;
        }
    }

    public class DynamicEntityTableRecordModel
    {
        public long EntityID { get; set; }
        public dynamic Entity { get; set; }
        public string TableDisplayName { get; set; }
        public string TableVersionName { get; set; }
        public List<DynamicColumn> Columns { get; set; }
    }
}