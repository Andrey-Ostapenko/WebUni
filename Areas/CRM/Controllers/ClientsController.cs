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
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Helpers;
using livemenu.Kernel.CRM;
using livemenu.Kernel.CRM.Clients;
using livemenu.Kernel.Projects;
using livemenu.Models;
using livemenu.Models.Cards;
using Microsoft.Practices.ServiceLocation;
using Unit = System.Web.UI.WebControls.Unit;

namespace livemenu.Areas.CRM.Controllers
{
    public partial class ClientsController : CRMController
    {
        private readonly IClientService _clientService;
        private readonly IClientListService _clientListService;
        private readonly IClientLegalPersonalityService _clientLegalPersonalityService;
        private readonly IClientNaturalPersonService _clientNaturalPersonService;
        private readonly IClientIndividualProprietorService _clientIndividualProprietorService;

        public ClientsController(IClientService clientService, IClientListService clientListService, IClientLegalPersonalityService clientLegalPersonalityService, IClientNaturalPersonService clientNaturalPersonService, IClientIndividualProprietorService clientIndividualProprietorService)
        {
            _clientService = clientService;
            _clientListService = clientListService;
            _clientLegalPersonalityService = clientLegalPersonalityService;
            _clientNaturalPersonService = clientNaturalPersonService;
            _clientIndividualProprietorService = clientIndividualProprietorService;
        }

        public virtual ActionResult Index()
        {
            return View(new ResponsiveLayoutModel
            {
                CustomID = "crm-clients",
                Name = "Клиенты",
                InstructionLink = "http://www.WebUni.ru/platform-crm-clients"
            });
        }


        public virtual ActionResult ClientListGridViewPartial()
        {
            return View(MVC.CRM.Clients.Views.ClientListGridViewPartial);
        }


        public virtual ActionResult NaturalPersonGridViewPartial()
        {
            return View(MVC.CRM.Clients.Views.NaturalPersonGridViewPartial);
        }

        public virtual ActionResult ExportTo()
        {
            foreach (var typeName in GridViewHelpers.ExportTypes.Keys)
            {
                if (Request.Params[typeName.ToString()] != null)
                    return GridViewHelpers.ExportTypes[typeName].Method(GetNaturalPersonGridViewSettings(), _clientNaturalPersonService.GetAll(), "Клиенты - физические лица");
            }
            return GridViewHelpers.ExportTypes[GridViewExportType.Xlsx].Method(GetNaturalPersonGridViewSettings(), _clientNaturalPersonService.GetAll(), "Клиенты - физические лица");
        }

        public virtual ActionResult LegalPersonalityCard(long? id)
        {
            if (id.HasValue)
            {
                var item = _clientLegalPersonalityService.Get(id.Value);

                item.ClientRoleIDs = _clientService.GetClientRolesByID(id.Value);

                return View(MVC.CRM.Clients.Views.LegalPersonalityCardLayout, item);
            }
            else
            {
                var item = _clientLegalPersonalityService.PrepareNew();
                return View(MVC.CRM.Clients.Views.LegalPersonalityCardLayout, item);
            }
        }

        public virtual ActionResult LegalPersonalityCardSave(ClientLegalPersonality item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ClientRoleID == 0)
                    {
                        item.ClientRoleID = null;
                    }


                    var saved = _clientLegalPersonalityService.CreateOrUpdate(item);
                    saved.ClientRoleIDs = _clientService.GetClientRolesByID(item.ID);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент успешно сохранен." });
                    saved.NewLogin = saved.Login;
                    ModelState.Clear();
                    return View(MVC.CRM.Clients.Views.LegalPersonalityCard, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CRM.Clients.Views.LegalPersonalityCard, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен." }, NotificationType.Warning);
                return View(MVC.CRM.Clients.Views.LegalPersonalityCard, item);
            }
        }

        public virtual ActionResult ClientCard(long type)
        {
            switch ((ClientLegalTypeValue)type)
            {
                case ClientLegalTypeValue.LegalPersonality:
                    {
                        var item = _clientLegalPersonalityService.PrepareNew();
                        return View(MVC.CRM.Clients.Views.LegalPersonalityCardLayout, item);
                    }
                case ClientLegalTypeValue.NaturalPerson:
                    {
                        var item = _clientNaturalPersonService.PrepareNew();
                        return View(MVC.CRM.Clients.Views.NaturalPersonCardLayout, item);
                    }
                case ClientLegalTypeValue.IndividualProprietor:
                    {
                        var item = _clientIndividualProprietorService.PrepareNew();
                        return View(MVC.CRM.Clients.Views.IndividualProprietorCardLayout, item);
                    }
            }

            return null;

        }

        

        public virtual ActionResult WithoutTypeCard(long? id)
        {
            if (id.HasValue)
            {
                
                var item = _clientService.Get(id.Value);

                item.ClientRoleIDs = _clientService.GetClientRolesByID(id.Value);

                return View(MVC.CRM.Clients.Views.WithoutTypeCardLayout, item);
            }
            else
            {
                var item = _clientNaturalPersonService.PrepareNew();
                return View(MVC.CRM.Clients.Views.WithoutTypeCardLayout, item);
            }
        }

        public virtual ActionResult WithoutTypeCardSave(Client item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ClientRoleID == 0)
                    {
                        item.ClientRoleID = null;
                    }


                    var saved = _clientService.CreateOrUpdate(item);
                    saved.ClientRoleIDs = _clientService.GetClientRolesByID(item.ID);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент успешно сохранен." });
                    saved.NewLogin = saved.Login;
                    ModelState.Clear();
                    return View(MVC.CRM.Clients.Views.WithoutTypeCard, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CRM.Clients.Views.WithoutTypeCard, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен." }, NotificationType.Warning);
                return View(MVC.CRM.Clients.Views.WithoutTypeCard, item);
            }
        }


        public virtual ActionResult NaturalPersonCard(long? id)
        {
            if (id.HasValue)
            {
                var item = _clientNaturalPersonService.Get(id.Value); // дублирование np падаем

                item.ClientRoleIDs = _clientService.GetClientRolesByID(id.Value);

                return View(MVC.CRM.Clients.Views.NaturalPersonCardLayout, item);
            }
            else
            {
                var item = _clientNaturalPersonService.PrepareNew();
                return View(MVC.CRM.Clients.Views.NaturalPersonCardLayout, item);
            }
        }

        public virtual ActionResult NaturalPersonCardSave(ClientNaturalPerson item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ClientRoleID == 0)
                    {
                        item.ClientRoleID = null;
                    }

                    var saved = _clientNaturalPersonService.CreateOrUpdate(item);

                    saved.ClientRoleIDs = _clientService.GetClientRolesByID(item.ID);

                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент успешно сохранен." });
                    saved.NewLogin = saved.Login;
                    ModelState.Clear();
                    return View(MVC.CRM.Clients.Views.NaturalPersonCard, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CRM.Clients.Views.NaturalPersonCard, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен." }, NotificationType.Warning);
                return View(MVC.CRM.Clients.Views.NaturalPersonCard, item);
            }
        }

        public virtual ActionResult IndividualProprietorCard(long? id)
        {
            if (id.HasValue)
            {
                var item = _clientIndividualProprietorService.Get(id.Value);

                item.ClientRoleIDs = _clientService.GetClientRolesByID(id.Value);

                return View(MVC.CRM.Clients.Views.IndividualProprietorCardLayout, item);
            }
            else
            {
                var item = _clientIndividualProprietorService.PrepareNew();
                return View(MVC.CRM.Clients.Views.IndividualProprietorCardLayout, item);
            }
        }

        public virtual ActionResult IndividualProprietorCardSave(ClientIndividualProprietor item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (item.ClientRoleID == 0)
                    {
                        item.ClientRoleID = null;
                    }



                    var saved = _clientIndividualProprietorService.CreateOrUpdate(item);
                    saved.ClientRoleIDs = _clientService.GetClientRolesByID(item.ID);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент успешно сохранен." });
                    saved.NewLogin = saved.Login;
                    ModelState.Clear();
                    return View(MVC.CRM.Clients.Views.IndividualProprietorCard, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CRM.Clients.Views.IndividualProprietorCard, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Клиент не был сохранен." }, NotificationType.Warning);
                return View(MVC.CRM.Clients.Views.IndividualProprietorCard, item);
            }
        }

        public GridViewSettings GetClientListGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "ClientListGridView";
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
                column.FieldName = "ClientLegalTypeID";
                column.Caption = "Тип";

                column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
                column.ColumnType = MVCxGridViewColumnType.ComboBox;
                column.Settings.HeaderFilterMode = HeaderFilterMode.CheckedList;
                var comboBoxProperties = column.PropertiesEdit as ComboBoxProperties;
                comboBoxProperties.DataSource = ServiceLocator.Current.GetInstance<IClientLegalTypeService>().GetAll();
                comboBoxProperties.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
                comboBoxProperties.AllowUserInput = true;
                comboBoxProperties.DropDownStyle = DropDownStyle.DropDown;
                comboBoxProperties.TextField = "Name";
                comboBoxProperties.ValueField = "ID";
                comboBoxProperties.ValueType = typeof(long);
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "ClientRoles";
                column.Caption = "Роль";
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Name";
                column.Caption = "Название";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


            settings.Columns.Add(column =>
            {

                column.FieldName = "Phone";
                column.Caption = "Телефон";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Email";
                column.Caption = "Почта";


            });


            return settings;
        }

        public GridViewSettings GetNaturalPersonGridViewSettings()
        {
            var settings = new GridViewSettings();

            settings.Settings.ShowFooter = true;


            settings.Name = "NaturalPersonGridView";
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
                column.FieldName = "ID";
                column.Caption = "ID";
                column.ReadOnly = true;
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "LastName";
                column.Caption = "Фамилия";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });


            settings.Columns.Add(column =>
            {
                column.FieldName = "FirstName";
                column.Caption = "Имя";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "FathersName";
                column.Caption = "Отчество";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {

                column.FieldName = "Phone";
                column.Caption = "Телефон";
                //column.Settings.FilterMode = ColumnFilterMode.DisplayText;
                //column.Settings.AutoFilterCondition = AutoFilterCondition.Contains;
            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Email";
                column.Caption = "Почта";


            });

            settings.Columns.Add(column =>
            {
                column.FieldName = "Address";
                column.Caption = "Адрес";
            });
            return settings;
        }

        public virtual ActionResult SelectorClientListGridViewPartial()
        {
            return View();
        }
    }
}