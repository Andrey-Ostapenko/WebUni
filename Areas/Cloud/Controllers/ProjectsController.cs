using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebUni.Bus.Common;
using DevExpress.Data.Utils;
using DevExpress.Web;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Cloud;
using livemenu.Models.Auth;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.Cloud.Controllers
{
    [CustomAuthorize(AccessMode.OnlyWebUni)]
    public partial class ProjectsController : CloudController
    {
        private readonly ISitesService _sitesService;

        public ProjectsController()
        {
            
            _sitesService = ServiceLocator.Current.GetService<ISitesService>();

        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Test2DB()
        {
            _sitesService.Test2DB();
            return new EmptyResult();
        }

        public GridViewSettings GetGridViewSettings(bool isExport = false)
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "SitesGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "ProjectID";
                column.Caption = "ID проекта";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ProjectName";
                column.Caption = "Имя проекта";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CreationDate";
                column.Caption = "Дата создания";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
                column.ReadOnly = true;
            });

            settings.Columns.AddBand(orderBand =>
            {
                orderBand.Caption = "Клиент";
                orderBand.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "ClientID";
                    column.Caption = "ID";
                    column.ReadOnly = true;
                });

                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "ClientName";
                    column.Caption = "Имя";
                    column.ReadOnly = true;
                });
            });


            settings.Columns.AddBand(orderBand =>
            {
                orderBand.Caption = "Лицензия";
                orderBand.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "IsTrial";
                    column.Caption = "Триал";
                    column.ReadOnly = true;
                    column.ColumnType = MVCxGridViewColumnType.CheckBox;
                });


                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "LicenseTypeName";
                    column.Caption = "Тип";
                    column.ReadOnly = true;
                });

                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "LicensePeriodTypeName";
                    column.Caption = "Срок действия";
                    column.ReadOnly = true;
                });

                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "LicenseStartDate";
                    column.Caption = "Дата активации";
                    column.ColumnType = MVCxGridViewColumnType.DateEdit;
                    column.ReadOnly = true;
                });
            });



            settings.Columns.AddBand(orderBand =>
                  {
                      orderBand.Caption = "Хостинг";
                      orderBand.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                      orderBand.Columns.Add(column =>
                      {
                          column.FieldName = "HostingTypeName";
                          column.Caption = "Тип";
                          column.ReadOnly = true;
                      });

                      orderBand.Columns.Add(column =>
                           {
                               column.FieldName = "HostingPeriodTypeName";
                               column.Caption = "Срок действия";
                               column.ReadOnly = true;
                           });
                      orderBand.Columns.Add(column =>
                      {
                          column.FieldName = "HostingStartDate";
                          column.Caption = "Дата активации";
                          column.ColumnType = MVCxGridViewColumnType.DateEdit;
                          column.ReadOnly = true;
                      });

                      orderBand.Columns.Add(column =>
                      {
                          column.FieldName = "Bindings";
                          column.Caption = "Домены";
                          column.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                          column.ColumnType = MVCxGridViewColumnType.Memo;
                          var MemoProperties = column.PropertiesEdit as MemoProperties;


                          MemoProperties.Rows = 5;
                          //  MemoProperties.Height = System.Web.UI.WebControls.Unit.Percentage(100);
                      //    MemoProperties.Width = System.Web.UI.WebControls.Unit.Pixel(200);
                      });
                  });


            settings.Columns.Add(column =>
            {
                column.FieldName = "State";
                column.Caption = "Состояние";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            return settings;
        }


        public virtual ActionResult GridViewPartial()
        {
            return View();
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<WebUniSite, int> updateValues)
        {
            foreach (var ci in updateValues.Update)
            {
                if (updateValues.IsValid(ci))
                    Update(ci, updateValues);
            }
            return PartialView(MVC.Cloud.Projects.Views.GridViewPartial);
        }

        protected void Update(WebUniSite site, MVCxGridViewBatchUpdateValues<WebUniSite, int> updateValues)
        {
            try
            {
                OperationWrapper(() =>
                {
                    var errors = _sitesService.Update(site);
                    if (errors != null)
                    {
                        foreach (var error in errors)
                        {
                            var errorText = error.Error == UpdateProjectsResponseErrorType.BindingAlreadyExist
                                ? "Один из доменов уже используется другим проектом"
                                : "Произошла ошибка";
                            updateValues.SetErrorText(site, errorText);
                        }

                        return false;
                    }
                    return true;

                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(site, "Ошибка");
            }
        }

        public virtual JsonResult Start(Guid projectID)
        {
            _sitesService.Start(projectID);
            return Json(true);
        }

        public virtual JsonResult Stop(Guid projectID)
        {
            _sitesService.Stop(projectID);
            return Json(true);
        }

        public virtual JsonResult Restart(Guid projectID)
        {
            _sitesService.Restart(projectID);
            return Json(true);
        }

        public virtual JsonResult Delete(Guid projectID)
        {
            _sitesService.Delete(projectID);
            return Json(true);
        }

        public virtual JsonResult Create(string name)
        {
            _sitesService.Create(name);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}