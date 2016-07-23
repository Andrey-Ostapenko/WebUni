using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.CRM;
using livemenu.Models;
using Unit = System.Web.UI.WebControls.Unit;


namespace livemenu.Areas.CRM.Controllers
{
    public partial class BalanceController : CRMController
    {
        private readonly IBalanceService _balanceService;

        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "crm-balance",
                Name = "Баланс",
                InstructionLink = "http://www.WebUni.ru/platform-crm-balance"
            });
        }

        public virtual ActionResult GridViewPartial()
        {
            return View();
        }

        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "BalanceGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";

            settings.Columns.Add(column =>
            {
                column.FieldName = "CreationDate";
                column.Caption = "Дата";
                column.ColumnType = MVCxGridViewColumnType.DateEdit;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ClientID";
                column.Caption = "ID Клиента";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ClientName";
                column.Caption = "Клиент";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "IncomeValue";
                column.Caption = "Приход";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ExpenseValue";
                column.Caption = "Расход";
            });

            return settings;
        }


        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            foreach (var product in updateValues.Insert)
            {
                if (updateValues.IsValid(product))
                    Insert(product, updateValues);
            }
            foreach (var product in updateValues.Update)
            {
                if (updateValues.IsValid(product))
                    Update(product, updateValues);
            }
            foreach (var productID in updateValues.DeleteKeys)
            {
                Delete(productID, updateValues);
            }
            return PartialView(MVC.CRM.Balance.Views.GridViewPartial);
        }



        protected void Insert(ClientRole clientRole, MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() =>
                {
                //    _balanceService.Create(clientRole);
                });
            }
            else
            {
                updateValues.SetErrorText(clientRole, "Ошибка");
            }
        }
        protected void Update(ClientRole clientRole, MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                 //   _balanceService.Update(clientRole);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(clientRole, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<ClientRole, int> updateValues)
        {
            try
            {
                UpdateWrapper(() =>
                {
                 //   _balanceService.Delete(id);
                });
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }


    }
}