using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using DevExpress.Utils;
using DevExpress.Web.Mvc;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.CRM.Orders;
using livemenu.Models;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class OrderStatusController : CRMController
    {
        private readonly IOrderStatusService _orderStatusService;

        public OrderStatusController(IOrderStatusService orderStatusService)
        {
            _orderStatusService = orderStatusService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "crm-orders-statuses",
                Name = "Статусы заказов",
                InstructionLink = "http://www.WebUni.ru/platform-crm-order"
            });
        }


        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "OrderStatusGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";



            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });



            return settings;
        }



        public virtual ActionResult GridViewPartial()
        {
            return View(_orderStatusService.GetAll());
        }

        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<OrderStatus, int> updateValues)
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
            return PartialView(MVC.CRM.OrderStatus.Views.GridViewPartial, _orderStatusService.GetAll());
        }

        protected void Insert(OrderStatus orderStatus, MVCxGridViewBatchUpdateValues<OrderStatus, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _orderStatusService.Create(orderStatus));
            }
            else
            {
                updateValues.SetErrorText(orderStatus, "Ошибка");
            }
        }
        protected void Update(OrderStatus orderStatus, MVCxGridViewBatchUpdateValues<OrderStatus, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _orderStatusService.Update(orderStatus));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(orderStatus, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<OrderStatus, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _orderStatusService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }


	}
}