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

using livemenu.Areas.CRM.Services;
using livemenu.Common.Entities.Entities;
using livemenu.Helpers;
using livemenu.Kernel.CRM.Clients;
using livemenu.Kernel.CRM.Orders;
using livemenu.Models;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class OrderController : CRMController
    {
        private readonly IOrderService _orderService;
        private readonly IOrderListService _orderListService;
        private readonly IOrderElementService _orderElementService;
        private readonly IEmailSenderService _emailSenderService;

        public OrderController(IOrderService orderService, IOrderListService orderListService, IOrderElementService orderElementService, IEmailSenderService emailSenderService)
        {
            _orderService = orderService;
            _orderListService = orderListService;
            _orderElementService = orderElementService;
            _emailSenderService = emailSenderService;
        }

        public virtual ActionResult ExportTo()
        {
            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetGridViewSettings(), _orderListService.GetAll(), "Заказы");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetGridViewSettings(), _orderListService.GetAll(), "Заказы");
        }


        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "crm-orders",
                Name = "Заказы",
                InstructionLink = "http://www.WebUni.ru/platform-crm-orders"
            });
        }
        
        public GridViewSettings GetGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "OrderGridView";
            settings.Width = new Unit(100, UnitType.Percentage);
            settings.Settings.ShowFilterRow = true;
            settings.KeyFieldName = "ID";



            settings.Columns.Add(column =>
            {
                column.FieldName = "Number";
                column.Caption = " Номер";
            });

            settings.Columns.AddBand(orderBand =>
            {
                orderBand.Caption = "Поступил";
                orderBand.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "CreationDateDate";
                    column.Caption = "Дата";

                    column.ColumnType = MVCxGridViewColumnType.DateEdit;
                    var prop = column.PropertiesEdit as DateEditProperties;
                });
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "CreationDateTime";
                    column.Caption = "Время";

                    column.ColumnType = MVCxGridViewColumnType.TimeEdit;
                    var prop = column.PropertiesEdit as TimeEditProperties;
                });
            });

            settings.Columns.AddBand(orderBand =>
            {
                orderBand.Caption = "Принято";
                orderBand.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "AcceptedDateDate";
                    column.Caption = "Дата";

                    column.ColumnType = MVCxGridViewColumnType.DateEdit;
                    var prop = column.PropertiesEdit as DateEditProperties;
                });
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "AcceptedDateTime";
                    column.Caption = "Время";

                    column.ColumnType = MVCxGridViewColumnType.TimeEdit;
                    var prop = column.PropertiesEdit as TimeEditProperties;
                });
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "InchargeUserName";
                column.Caption = "Ответственный";
            });


            settings.Columns.AddBand(orderBand =>
            {
                orderBand.Caption = "Сдача";
                orderBand.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "DeadlineDateDate";
                    column.Caption = "Дата";

                    column.ColumnType = MVCxGridViewColumnType.DateEdit;
                    var prop = column.PropertiesEdit as DateEditProperties;
                });
                orderBand.Columns.Add(column =>
                {
                    column.FieldName = "DeadlineDateTime";
                    column.Caption = "Время";

                    column.ColumnType = MVCxGridViewColumnType.TimeEdit;
                    var prop = column.PropertiesEdit as TimeEditProperties;
                });
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ClientID";
                column.Caption = "Клиент";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<IClientListService>().GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Name";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "OrderStatusID";
                column.Caption = "Статус";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<IOrderStatusService>().GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Name";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "IsPaid";
                column.Caption = "Оплата";
                column.ColumnType = MVCxGridViewColumnType.CheckBox;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Income";
                column.Caption = "Доход";
                column.ColumnType = MVCxGridViewColumnType.TextBox;
                column.PropertiesEdit.DisplayFormatString = "0.00";

            });


            return settings;
        }

        public virtual ActionResult OrderGridViewPartial()
        {
            return View();
        }

        public virtual ActionResult GridViewDeletePartial(long id)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _orderService.Delete(id));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView(MVC.CRM.Order.Views.OrderGridViewPartial);
        }


        public virtual ActionResult Card(long? id)
        {
            if (id.HasValue)
            {
                var item = _orderService.DeepLoad(id.Value);
                item.AcceptedTime = item.AcceptedDate.HasValue ? item.AcceptedDate.Value.ToString("HH:mm") : null;
                item.DeadlineTime = item.DeadlineDate.HasValue ? item.DeadlineDate.Value.ToString("HH:mm") : null;

                return View(MVC.CRM.Order.Views.CardLayout, item);
            }
            else
            {
                var item = _orderService.PrepareNew();
                return View(MVC.CRM.Order.Views.CardLayout, item);
            }
        }

        public virtual ActionResult Test()
        {
            _emailSenderService.Test();
            return null;
        }

        public virtual ActionResult CardSave(Order item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.InchargeUserID == 0)
                    {
                        item.InchargeUserID = null;
                    }

                    var saved = _orderService.CreateOrUpdate(item);

                    //var emailer = ServiceLocator.Current.GetInstance<IEmailSenderService>();
                    //emailer.OrderCreated(saved.ID);

                    _notificationBus.Notificate(new NotificationMessage() { Text = "Заказ успешно сохранен." });
                    ModelState.Clear();
                    return View(MVC.CRM.Order.Views.Card,  _orderService.DeepLoad(saved.ID));
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Заказ не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CRM.Order.Views.Card, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Заказ не был сохранен." }, NotificationType.Warning);
                return View(MVC.CRM.Order.Views.Card, item);
            }
        }

        public GridViewSettings GetOrderElementGridViewSettings()
        {
            var settings = new GridViewSettings();


            settings.Name = "OrderElementGridView";
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
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });



            settings.Columns.Add(column =>
            {
                column.FieldName = "Articul";
                column.Caption = "Артикул";

                column.ReadOnly = true;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Наименование";

                column.ReadOnly = true;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Count";
                column.Caption = "Количество";

                //   column.PropertiesEdit.DisplayFormatString = "N2";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                spinEditProperties.DecimalPlaces = 2;
                spinEditProperties.NumberType = SpinEditNumberType.Float;
                spinEditProperties.MinValue = 1;
                spinEditProperties.MaxValue = int.MaxValue;

            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "CustomPrice";
                column.Caption = "Стоимость";

                //   column.PropertiesEdit.DisplayFormatString = "N2";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                spinEditProperties.DecimalPlaces = 2;
                spinEditProperties.NumberType = SpinEditNumberType.Float;
                spinEditProperties.MinValue = 0;

            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "CustomDiscount";
                column.Caption = "Скидка";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                spinEditProperties.NumberType = SpinEditNumberType.Float;
                spinEditProperties.DecimalPlaces = 2;
                spinEditProperties.MinValue = 0;

            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "CustomBonus";
                column.Caption = "Бонус";

                column.ColumnType = MVCxGridViewColumnType.SpinEdit;
                var spinEditProperties = column.PropertiesEdit as SpinEditProperties;
                spinEditProperties.NumberType = SpinEditNumberType.Float;
                spinEditProperties.DecimalPlaces = 2;
                spinEditProperties.MinValue = 0;

            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Total";
                column.Caption = "Итого";
                column.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                column.PropertiesEdit.DisplayFormatString = "N2";
            });

            settings.CustomUnboundColumnData = (sender, e) =>
            {
                if (e.Column.FieldName == "Total")
                {
                    double price = (double)((decimal?)e.GetListSourceFieldValue("CustomPrice") ?? 0);
                    int count = Convert.ToInt32(e.GetListSourceFieldValue("Count"));
                    double discount = (double?)e.GetListSourceFieldValue("CustomDiscount") ?? 0;
                    var tprice = price*count;
                    e.Value = tprice - tprice * discount / 100;
                }
            };
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Count, "DisplayIndex");
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Sum, "Total").DisplayFormat = "N2";
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Min, "Count");
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Average, "Count");
            settings.TotalSummary.Add(DevExpress.Data.SummaryItemType.Max, "Count");
            settings.Settings.ShowFooter = true;

            return settings;
        }


        public virtual ActionResult OrderElementGridViewPartial(long id, int? focusedRowIndex, int? draggingIndex, int? targetIndex, string command)
        {
            ViewBag.OID = id;


            if (command == "DRAGROW")
            {
                var model = _orderElementService.GetByOrderID(id);

                var draggingRowKey = GetKeyIdByDisplayIndex(draggingIndex, id);
                var targetRowKey = GetKeyIdByDisplayIndex(targetIndex, id);
                var draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
                for (int rowIndex = 0; rowIndex < model.Count(); rowIndex++)
                {
                    var rowKey = GetKeyIdByDisplayIndex(rowIndex, id);
                    if ((rowIndex > Math.Min(targetIndex.Value, draggingIndex.Value)) &&
                        (rowIndex < Math.Max(targetIndex.Value, draggingIndex.Value)))
                    {
                        UpdateDisplayIndex(rowKey, rowIndex + draggingDirection, id);
                    }
                }
                UpdateDisplayIndex(draggingRowKey, targetIndex, id);
                UpdateDisplayIndex(targetRowKey, targetIndex + draggingDirection, id);
            }


             return View(_orderElementService.DeepLoadByOrderID(id));
        }

        public long GetKeyIdByDisplayIndex(int? displayIndex, long id)
        {
            var model = _orderElementService.GetByOrderID(id);
            var rowKey = model.Where(q => q.DisplayIndex == displayIndex).Select(q => q.ID).FirstOrDefault();
            return rowKey;
        }

        public void UpdateDisplayIndex(long? rowKey, int? displayIndex, long id)
        {
            var model = _orderElementService.GetByOrderID(id);
            var product = model.FirstOrDefault(q => q.ID == rowKey);
            product.DisplayIndex = displayIndex ?? 0;
            _orderElementService.Update(product);
        }



        [ValidateInput(false)]
        public virtual ActionResult GridViewBatchUpdatePartial(MVCxGridViewBatchUpdateValues<OrderElement, int> updateValues, long elementOrderID)
        {
            ViewBag.OID = elementOrderID;

            foreach (var orderElement in updateValues.Insert)
            {
                if (updateValues.IsValid(orderElement))
                    Insert(orderElement, updateValues);
            }
            foreach (var orderElement in updateValues.Update)
            {
                if (updateValues.IsValid(orderElement))
                    Update(orderElement, updateValues);
            }
            foreach (var orderElementID in updateValues.DeleteKeys)
            {
                Delete(orderElementID, updateValues);
            }
            return PartialView(MVC.CRM.Order.Views.OrderElementGridViewPartial, _orderElementService.DeepLoadByOrderID(elementOrderID));
        }

        protected void Insert(OrderElement orderElement, MVCxGridViewBatchUpdateValues<OrderElement, int> updateValues)
        {
            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _orderElementService.Create(orderElement));
            }
            else
            {
                updateValues.SetErrorText(orderElement, "Ошибка");
            }
        }
        protected void Update(OrderElement orderElement, MVCxGridViewBatchUpdateValues<OrderElement, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _orderElementService.Update(orderElement));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(orderElement, "Ошибка");
            }
        }
        protected void Delete(int id, MVCxGridViewBatchUpdateValues<OrderElement, int> updateValues)
        {
            try
            {
                UpdateWrapper(() => _orderElementService.Delete(id));
            }
            catch (Exception e)
            {
                updateValues.SetErrorText(id, e.Message);
            }
        }

       
        
        [HttpPost]
        public virtual ActionResult AddOrderElements(AddOrderElementsModel data)
        {
            _orderElementService.AppendToOrder(data.orderid, data.ids);
            return Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult OrderCreated(long id)
        {
            _emailSenderService.OrderCreated(id);
            return Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult OrderIncomeRecived(long id)
        {
            _emailSenderService.OrderIncomeRecived(id);
            return Json(true);
        }


        [AllowAnonymous]
        public virtual JsonResult OrderIncomeRecivedTEST()
        {
            _emailSenderService.OrderIncomeRecived(26);
            return Json(true);
        }


        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult ClientActivationRequest(long id)
        {
            _emailSenderService.ClientActivationRequest(id);
            return Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult ClientFourDigitActivationRequest(long id)
        {
            _emailSenderService.ClientFourDigitActivationRequest(id);
            return Json(true);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual JsonResult ClientForgotPassword(long id)
        {
            _emailSenderService.ClientForgotPassword(id);
            return Json(true);
        }

    }

    public class AddOrderElementsModel
    {
        public long orderid { get; set; }
        public long[] ids { get; set; }
    }
}