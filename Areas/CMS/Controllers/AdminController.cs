using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using CsQuery.EquationParser.Implementation.Functions;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Controllers;
using livemenu.Excel.CMS;
using livemenu.Helpers;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.Configurator;
using livemenu.Kernel.MicroModules;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class AdminController : BaseController
    {
        private readonly IConfiguratorService _configuratorService;
        private readonly ICatalogItemBatchExcelImportService _catalogItemBatchExcelImportService;
        private readonly IMicroModuleFactory _microModuleFactory;
        private readonly ICatalogItemService _catalogItemService;

        public AdminController(IConfiguratorService configuratorService, ICatalogItemBatchExcelImportService catalogItemBatchExcelImportService, IMicroModuleFactory  microModuleFactory, ICatalogItemService catalogItemService)
        {
            _configuratorService = configuratorService;
            _catalogItemBatchExcelImportService = catalogItemBatchExcelImportService;
            _microModuleFactory = microModuleFactory;
            _catalogItemService = catalogItemService;
        }
        [ValidateInput(false)]
        public virtual ActionResult ExportTo(string filterString)
        {
            CriteriaOperator filterCriteria = CriteriaOperator.Parse(filterString);

            CriteriaToExpressionConverter converter = new CriteriaToExpressionConverter();

            var source = _catalogItemService.RawDataQueryable;
           var filteredData = source.AppendWhere(converter, filterCriteria) as IQueryable<CatalogItem>;

            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetCatalogsGridViewSettings(false), filteredData.ToList(), "Элементы каталога");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetCatalogsGridViewSettings(false), filteredData.ToList(), "Элементы каталога");
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

                var path = !Debugger.IsAttached ? Server.MapPath("/Admin/" + file) : Server.MapPath("/" + file);
                _catalogItemBatchExcelImportService.Import(path, mes =>
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

        public GridViewSettings GetCatalogsGridViewSettings(bool isExport = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "AdminCatalogsGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "ID";
                column.Caption = "ID";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CatalogItem3.Name";
                column.Caption = "Каталог";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "CatalogItem2.Code";
                column.Caption = "Код родителя";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });
            if (!isExport)
            {
                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName = "CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 2";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName = "CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 3";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName = "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 4";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName = "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 5";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName =
                        "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 6";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName =
                        "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 7";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName =
                        "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 8";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName =
                        "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 9";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });

                settings.Columns.Add(column =>
                {
                    column.Visible = false;
                    column.FieldName =
                        "CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.CatalogItem2.Code";
                    column.Caption = "Код родителя 10";
                    column.ReadOnly = true;
                    //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                    //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                });
            }
            settings.Columns.Add(column =>
            {
                column.FieldName = "CatalogItem2.Name";
                column.Caption = "Назв. родителя";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Code";
                column.Caption = "Код";

                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CustomCode";
                column.Caption = "Код страниц";

                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CatalogItemType.Name";
                column.Caption = "Тип";
                column.ReadOnly = true;
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                //  column.ColumnType = MVCxGridViewColumnType.ComboBox;
                // column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.TextDataText.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.TextDataText];
                c.PropertiesEdit.EncodeHtml = true;
                c.ColumnType = MVCxGridViewColumnType.Memo;
                c.UnboundType = DevExpress.Data.UnboundColumnType.String;
            });

            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.CaptionDataText.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.CaptionDataText];
                c.PropertiesEdit.EncodeHtml = true;
                c.ColumnType = MVCxGridViewColumnType.Memo;
                c.UnboundType = DevExpress.Data.UnboundColumnType.String;
            });

            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignMarginTop.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignMarginTop];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignMarginBottom.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignMarginBottom];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignMarginLeft.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignMarginLeft];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignMarginRight.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignMarginRight];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });


            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignPaddingTop.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignPaddingTop];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignPaddingBottom.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignPaddingBottom];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignPaddingLeft.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignPaddingLeft];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });
            settings.Columns.Add(c =>
            {
                c.Width = Unit.Pixel(100);
                c.Visible = false;
                c.FieldName = CatalogItemBatchExcelImportCellType.LineDesignPaddingRight.ToString();
                c.Caption = CatalogItemBatchExcelImportCellTypeHelpers.Names[CatalogItemBatchExcelImportCellType.LineDesignPaddingRight];
                c.ColumnType = MVCxGridViewColumnType.SpinEdit;
                c.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
            });



            settings.CustomUnboundColumnData = (s, e) =>
            {
                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.TextDataText.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniText") as ICollection<UniText>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).Text : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.CaptionDataText.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniCaption") as ICollection<UniCaption>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).Text : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignMarginTop.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).MarginTop : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignMarginBottom.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).MarginBottom : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignMarginLeft.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).MarginLeft : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignMarginRight.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).MarginRight : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignPaddingTop.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).PaddingTop : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignPaddingBottom.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).PaddingBottom : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignPaddingLeft.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).PaddingLeft : null;
                }

                if (e.Column.FieldName == CatalogItemBatchExcelImportCellType.LineDesignPaddingRight.ToString())
                {
                    var tasks = e.GetListSourceFieldValue("UniLine") as ICollection<UniLine>;

                    e.Value = tasks.Any() ? tasks.ElementAt(0).PaddingRight : null;
                }


            };

            settings.Columns.Add(column =>
            {
                column.FieldName = "IsVisible";
                column.Caption = "Видимость";
                column.Width = Unit.Pixel(1);
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            if (isExport)
            {
                settings.Columns.Add(column =>
                {
                    column.FieldName = "ToDelete";
                    column.Caption = "УДАЛИТЬ";
                });
            }

            return settings;
        }

        
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult GenerateBWPConfigCodes()
        {
            return View(_configuratorService.GenerateBWPConfigCodes());
        }

        public virtual ActionResult Catalogs()
        {
            return View();
        }
        [ValidateInput(false)]
        public virtual ActionResult CatalogsGridViewPartial()
        {
            return View();
        }



        [ValidateInput(false)]
        public virtual ActionResult CatalogsGridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<CatalogItem, int> updateValues)
        {
            foreach (var ci in updateValues.Insert)
            {
                if (updateValues.IsValid(ci))
                    Insert(ci, updateValues);
            }
            foreach (var ci in updateValues.Update)
            {
                if (updateValues.IsValid(ci))
                    Update(ci, updateValues);
            }
            foreach (var ciID in updateValues.DeleteKeys)
            {
                Delete(ciID, updateValues);
            }
            return PartialView(MVC.CMS.Admin.Views.CatalogsGridViewPartial);
        }




        protected void Insert(CatalogItem storage, MVCxGridViewBatchUpdateValues<CatalogItem, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _catalogItemService.Create(storage));
            }
            else
            {
                updateValues.SetErrorText(storage, "Ошибка");
            }
        }
        protected void Update(CatalogItem storage, MVCxGridViewBatchUpdateValues<CatalogItem, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _catalogItemService.ShallowUpdate(storage));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(storage, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<CatalogItem, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _catalogItemService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }
	}
}