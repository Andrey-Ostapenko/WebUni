using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BWP.Kernel.Notification;
using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Common;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Settings;
using livemenu.Excel.CRM;
using livemenu.Helpers;
using livemenu.Kernel.BWP.Pages.SEO;
using livemenu.Models;
using livemenu.Models.Widgets;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;


namespace livemenu.Areas.CMS.Controllers
{
    public partial class SEOController : CMSController
    {
        private readonly ISettingsService _settingsService;
        private readonly ISitemapPageService _sitemapPageService;
        private readonly ISEOPageService _seoPageService;
        private readonly ISEOMetaExcelImportService _seoMetaExcelImportService;
        private readonly ISitemapExcelImportService _sitemapExcelImportService;

        public SEOController(
            ISettingsService settingsService, 
            ISitemapPageService sitemapPageService, 
            ISEOPageService seoPageService, 
            ISEOMetaExcelImportService seoMetaExcelImportService, 
            ISitemapExcelImportService sitemapExcelImportService)
        {
            _settingsService = settingsService;
            _sitemapPageService = sitemapPageService;
            _seoPageService = seoPageService;
            _seoMetaExcelImportService = seoMetaExcelImportService;
            _sitemapExcelImportService = sitemapExcelImportService;
        }

        #region seo

        [HttpPost]
        public virtual JsonResult ImportMeta(string file)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Импорт запущен"
                });
                var path = Server.MapPath("/Admin/" + file);
                //var path = Server.MapPath(file);
                _seoMetaExcelImportService.Import(path, mes =>
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
        public virtual ActionResult ExportMetaTo()
        {
            var storages = _seoPageService.RawDataQueryable.ToList();


            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetMetaGridViewSettings(), storages, "Meta");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetMetaGridViewSettings(), storages, "Meta");
        }


        public virtual ActionResult Meta()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-seo-meta",
                Name = "Метаданные страниц",
                InstructionLink = "http://www.WebUni.ru/platform-cms-seo"
            });
        }

        public GridViewSettings GetMetaGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "MetaGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

        

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                column.ColumnType = MVCxGridViewColumnType.TextBox;

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Code";
                column.Caption = "Код";
                column.ColumnType = MVCxGridViewColumnType.TextBox;

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "IsVisible";
                column.Caption = "Видимость";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "SEOUseNameAsTitle";
                column.Caption = "Использовать название как заголовок";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                column.Width = new Unit(1, UnitType.Percentage);
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "SEOTitle";
                column.Caption = "Заголовок";
                column.Width = new Unit(30, UnitType.Percentage);
            });


           


            settings.Columns.Add(column =>
            {
                column.FieldName = "SEODescription";
                column.Caption = "Описание";
                column.ColumnType = MVCxGridViewColumnType.Memo;
                column.Width = new Unit(50, UnitType.Percentage);
                var props = (MemoProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
                props.Columns = 1;
                props.Rows = 10;
                
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SEOKeywords";
                column.Caption = "Ключевые слова";
                column.ColumnType = MVCxGridViewColumnType.Memo;
                column.Width = new Unit(50, UnitType.Percentage);
                var props = (MemoProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
                props.Columns = 1;
                props.Rows = 10;
            });
            settings.Columns.Add(column =>
            {
                column.FieldName = "ID";
                column.Caption = "ID";

            });

            return settings;
        }

        public virtual ActionResult MetaGridViewPartial()
        {
            return View();
        }

        [ValidateInput(false)]
        public virtual ActionResult MetaGridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<SEOPage, int> updateValues)
        {
            foreach (var ci in updateValues.Update)
            {
                if (updateValues.IsValid(ci))
                    MetaUpdate(ci, updateValues);
            }
            return PartialView(MVC.CMS.SEO.Views.MetaGridViewPartial);
        }

        protected void MetaUpdate(SEOPage unipage, MVCxGridViewBatchUpdateValues<SEOPage, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _seoPageService.Update(unipage));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(unipage, "Ошибка");
            }
        }
        

        #endregion

        #region sitemap


        [HttpPost]
        public virtual JsonResult ImportSitemap(string file)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Импорт запущен"
                });
                var path = Server.MapPath("/Admin/" + file);
                //var path = Server.MapPath(file);
                _sitemapExcelImportService.Import(path, mes =>
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
        public virtual ActionResult ExportSitemapTo()
        {
            var storages = _sitemapPageService.RawDataQueryable.ToList();


            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetSitemapGridViewSettings(), storages, "Sitemap");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetSitemapGridViewSettings(), storages, "Sitemap");
        }



        public virtual ActionResult Sitemap()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-seo-sitemap",
                Name = "Sitemap",
                InstructionLink = "http://www.WebUni.ru/platform-cms-seo"
            });
        }

        public GridViewSettings GetSitemapGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "SitemapGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

          

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                column.ColumnType = MVCxGridViewColumnType.TextBox;

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Code";
                column.Caption = "Код";
                column.ColumnType = MVCxGridViewColumnType.TextBox;

                column.EditFormSettings.Visible = DefaultBoolean.False;

                var props = (TextBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "IsVisible";
                column.Caption = "Видимость";

                column.EditFormSettings.Visible = DefaultBoolean.False;

                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "UseInSitemap";
                column.Caption = "Выгружать в Sitemap";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapUsePriority";
                column.Caption = "Выгружать priority";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });
            

            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapPriority";
                column.Caption = "priority";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                var props = (SpinEditProperties)column.PropertiesEdit;
                props.MinValue = 0;
                props.MaxValue = 1;
                props.Increment = (decimal)0.1;
                props.DecimalPlaces = 1;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapUseLastmod";
                column.Caption = "Выгружать lastmod";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapUseCacheDateAsLastmod";
                column.Caption = "Использовать lastmod из кэша";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapLastmod";
                column.Caption = "lastmod";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
            });



            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapUseChangefreq";
                column.Caption = "Выгружать changefreq";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
                var props = (CheckBoxProperties)column.PropertiesEdit;
                props.ValidationSettings.Display = Display.None;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "SitemapChangefreq";
                column.Caption = "changefreq";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<ISitemapChangefreqTypeService>().GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Code";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
               
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ID";
                column.Caption = "ID";

            });

            return settings;
        }
       
        public virtual ActionResult SitemapGridViewPartial()
        {
            return View();
        }

       

        [ValidateInput(false)]
        public virtual ActionResult SitemapGridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<SitemapPage, int> updateValues)
        {
            foreach (var ci in updateValues.Update)
            {
                if (updateValues.IsValid(ci))
                    SitemapUpdate(ci, updateValues);
            }
            return PartialView(MVC.CMS.SEO.Views.SitemapGridViewPartial);
        }

        protected void SitemapUpdate(SitemapPage unipage, MVCxGridViewBatchUpdateValues<SitemapPage, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _sitemapPageService.Update(unipage));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(unipage, "Ошибка");
            }
        }

        public virtual ActionResult SitemapSettings()
        {
            return View(_settingsService.GetSitemapSettings());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SaveSitemapSettings(SitemapSettings settings)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _settingsService.SaveSitemapSettings(settings);
                    });

                return View(MVC.CMS.SEO.Views.SitemapSettingsInternalEdit, _settingsService.GetSitemapSettings());
            }
            else
            {
                return View(MVC.CMS.SEO.Views.SitemapSettingsInternalEdit, _settingsService);
            }

        }

        #endregion

        #region robots

        public virtual ActionResult Robots()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "cms-seo-robots",
                Name = "robots.txt",
                InstructionLink = "http://www.WebUni.ru/platform-cms-seo"
            });
        }


        public virtual ActionResult RobotsSettings()
        {
            return View(_settingsService.GetRobotsSettings());
        }

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult SaveRobotsSettings(RobotsSettings settings)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(
                    () =>
                    {
                        _settingsService.SaveRobotsSettings(settings);
                    });

                return View(MVC.CMS.SEO.Views.RobotsSettingsInternalEdit, _settingsService.GetRobotsSettings());
            }
            else
            {
                return View(MVC.CMS.SEO.Views.RobotsSettingsInternalEdit, _settingsService);
            }

        }



        #endregion
    }
}