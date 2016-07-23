using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Areas.CMS.Controllers;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.CMS;
using livemenu.Kernel.CRM.Storages;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;


namespace livemenu.Areas.CMS.Controllers
{
    public partial class TagsController : CMSController
    {
        private readonly ITagService _tagService;
        private readonly ITagGroupService _tagGroupService;
        private readonly ITagListService _tagListService;

        public TagsController(ITagService tagService, ITagListService tagListService, ITagGroupService tagGroupService)
        {
            _tagService = tagService;
            _tagListService = tagListService;
            _tagGroupService = tagGroupService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-tags",
                Name = "Фильтры",
                InstructionLink = "http://www.WebUni.ru/platform-cms-filters"
            });
        }


        public virtual ActionResult GridViewPartial(int? focusedRowIndex, int? draggingIndex, int? targetIndex, string command)
        {
            if (command == "DRAGROW")
            {
                var model = _tagService.RawDataQueryable.OrderBy(q => q.DisplayIndex);

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
            return View(MVC.CMS.Tags.Views.GridViewPartial);
        }
        public long GetKeyIdByDisplayIndex(int? displayIndex)
        {
            var model = _tagService.RawDataQueryable;
            var rowKey = model.Where(q => q.DisplayIndex == displayIndex).Select(q => q.ID).FirstOrDefault();
            return rowKey;
        }

        public void UpdateDisplayIndex(long? rowKey, int? displayIndex)
        {
            var model = _tagService.RawDataQueryable;
            var product = model.FirstOrDefault(q => q.ID == rowKey);
            product.DisplayIndex = displayIndex ?? 0;
            _tagService.Commit();
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
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewSettings(false, true), _tagService.GetAll(), "Фильтры");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewSettings(false, true), _tagService.GetAll(), "Фильтры");
        }

        public GridViewSettings GetGridViewSettings(bool isSelector = false, bool showID = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "TagGridView";
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
                column.FieldName = "GroupName";
                column.Caption = "Группа";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });




            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";


            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "UseNameAsActiveName";
                column.Caption = "Использовать название при включении";
             
           //     column.EditFormSettings.Visible = DefaultBoolean.False;

                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;

            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ActiveName";
                column.Caption = "Название при включении";


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
                //_searchPropertyExcelImportService.Import(path, mes =>
                //{
                //    _notificationBus.Notificate(new NotificationMessage()
                //    {
                //        Text = mes
                //    });
                //});
                return true;
            }, "Импорт успешно завершен");
            return null;
        }



        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<TagList, int> updateValues)
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
            return PartialView(MVC.CMS.Tags.Views.GridViewPartial, _tagService.GetAll());
        }

        protected void Insert(TagList property, MVCxGridViewBatchUpdateValues<TagList, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _tagListService.Create(property));
            }
            else
            {
                updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Update(TagList property, MVCxGridViewBatchUpdateValues<TagList, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _tagListService.Update(property));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(property, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<TagList, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _tagListService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }


        
        

        [HttpGet]
        public virtual JsonResult SearchTag(string id)
        {

            var groups = new List<SelectItemGroup>();

            var lid = id.ToLower();
            var grouped = _tagService.GetAll().Where(x=>x.Name.ToLower().Contains(lid) || x.TagGroup.Name.Contains(lid)).OrderBy(x=>x.DisplayIndex).GroupBy(x => x.TagGroup.Name);

            foreach (var group in grouped)
            {
                groups.Add(new SelectItemGroup
                {
                    text = group.Key,
                    children = group.Select(x => new SelectItem
                    {
                        id = (int) x.ID,
                        text = x.Name
                    }).ToList()
                });
            }

           // var query = _makes.Where(m => m.text.ToLower().Contains(id.ToLower()));

            return Json(groups, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetTag(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var tags = _tagService.GetAll().OrderBy(x => x.DisplayIndex);

            string[] idList = id.Split(new char[] { ',' });
            foreach (var idStr in idList)
            {
                int idInt;
                if (int.TryParse(idStr, out idInt))
                {
                    var tag = tags.FirstOrDefault(m => m.ID == idInt);
                    if (tag == null)
                        continue;
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


    public class SelectItem
    {
        public int id { get; set; }
        public string text { get; set; }
    }

    public class SelectItemGroup
    {
        public string text { get; set; }

        public List<SelectItem> children { get; set; }
    }


}