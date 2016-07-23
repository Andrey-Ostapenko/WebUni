using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Data;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.BWP;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.BWP;
using livemenu.Kernel.CRM.PriceList;
using livemenu.Kernel.CRM.Properties;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class SearchController : CMSController
    {
        private readonly ISearchPropertyService _searchPropertyService;
        private readonly ISearchPropertiesImportService _searchPropertyExcelImportService;

        public SearchController(ISearchPropertyService searchPropertyService, ISearchPropertiesImportService searchPropertyExcelImportService)
        {
            _searchPropertyService = searchPropertyService;
            _searchPropertyExcelImportService = searchPropertyExcelImportService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }


        public virtual ActionResult GridViewPartial(int? focusedRowIndex, int? draggingIndex, int? targetIndex, string command)
        {
            if (command == "DRAGROW")
            {
                var model = _searchPropertyService.RawDataQueryable.OrderBy(q => q.DisplayIndex);

                var draggingRowKey = GetKeyIdByDisplayIndex(draggingIndex);
                var targetRowKey = GetKeyIdByDisplayIndex(targetIndex);
                var draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
                for (int rowIndex = 0; rowIndex < model.Count(); rowIndex++)
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
            return View(MVC.CMS.Search.Views.GridViewPartial);
        }
        public long GetKeyIdByDisplayIndex(int? displayIndex)
        {
            var model = _searchPropertyService.RawDataQueryable;
            var rowKey = model.Where(q => q.DisplayIndex == displayIndex).Select(q => q.ID).FirstOrDefault();
            return rowKey;
        }

        public void UpdateDisplayIndex(long? rowKey, int? displayIndex)
        {
            var model = _searchPropertyService.RawDataQueryable;
            var product = model.FirstOrDefault(q => q.ID == rowKey);
            product.DisplayIndex = displayIndex ?? 0;
            _searchPropertyService.Commit();
        }


        public virtual ActionResult SelectorGridViewPartial()
        {
            return View();
        }
        public virtual ActionResult ExportTo()
        {
            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewSettings(false, true), _searchPropertyService.GetAll(), "Поиск");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewSettings(false, true), _searchPropertyService.GetAll(), "Поиск");
        }

        public GridViewSettings GetGridViewSettings(bool isSelector = false, bool showID = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "SearchPropertyGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";


            settings.Columns.Add(column =>
            {
                column.Width = new Unit(4, UnitType.Pixel);
                column.FieldName = "DisplayIndex";
                column.Caption = "#";
                column.SortIndex = 1;
                column.SortOrder = ColumnSortOrder.Ascending;
                column.ReadOnly = false;

                //var d = (column.PropertiesEdit as TextBoxProperties);
                //d.ValidationSettings.CausesValidation = false;
                //d.ValidationSettings.RequiredField.IsRequired = false;
                //d.ClientSideEvents.Validation = "onFakeValidation";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Group";
                column.Caption = "Группа";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


       

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";


            });

            if (showID)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "ID";
                    column.Caption = "ID";


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
                _searchPropertyExcelImportService.Import(path, mes =>
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



        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<SearchProperty, int> updateValues)
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
            return PartialView(MVC.CMS.Search.Views.GridViewPartial, _searchPropertyService.GetAll());
        }

        protected void Insert(SearchProperty property, MVCxGridViewBatchUpdateValues<SearchProperty, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _searchPropertyService.Create(property));
            }
            else
            {
                updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Update(SearchProperty property, MVCxGridViewBatchUpdateValues<SearchProperty, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _searchPropertyService.Update(property));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<SearchProperty, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _searchPropertyService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

	}
}