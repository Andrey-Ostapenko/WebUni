using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Attributes;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.Executing;
using livemenu.Kernel.Management;
using livemenu.Kernel.MicroModules;
using livemenu.Kernel.Rights;
using Microsoft.Practices.ServiceLocation;

namespace livemenu.Helpers
{
    public static class Helpers
    {
        public static string GetClientRegistrationTypeName(ClientRegistrationMethodValue value)
        {
            switch (value)
            {
                case ClientRegistrationMethodValue.Automatic:
                    {
                        return "Автоматически";
                    }
                case ClientRegistrationMethodValue.FromWebsite:
                    {
                        return "Через сайт";
                    }
                case ClientRegistrationMethodValue.ManualByUser:
                    {
                        return "В ручную";
                    }

                  
            }
            return string.Empty;
        }

        public static string GetWebUniLicenseTypeName(WebUniLicenseType value)
        {
            switch (value)
            {
                case WebUniLicenseType.Rent:
                    {
                        return "Аренда";
                    }
                case WebUniLicenseType.Unlimited:
                    {
                        return "Бессрочная";
                    }
            }
            return string.Empty;
        }

        public static string GetWebUniLicenseTariffName(WebUniLicenseTariff value)
        {
            switch (value)
            {
                case WebUniLicenseTariff.Professional:
                    {
                        return "Профессиональный";
                    }
                case WebUniLicenseTariff.Specialist:
                    {
                        return "Специальный";
                    }
                case WebUniLicenseTariff.Standard:
                    {
                        return "Стандартный";
                    }
                case WebUniLicenseTariff.Ultimate:
                    {
                        return "Максимальный";
                    }
            }
            return string.Empty;
        }

        public static string GetWebUniLicensePeriodName(int? monthcount)
        {
            if(!monthcount.HasValue)
                return string.Empty;

            var years = monthcount/12;
            if (years == 1)
                return $"{years} год";
            else if(years > 1 && years < 5)
                return $"{years} года";
            else
            {
                return $"{years} лет";
            }
        }
        


        public static SelectList BorderStyleSelectList(string selectedValue, string dataTextField, object value)
        {
            return new SelectList(
                new[]
                {
                    new {Value = 0, Text = "Линия"},
                    new {Value = 1, Text = "Двойная линия"},
                    new {Value = 2, Text = "Штрих"},
                    new {Value = 3, Text = "Пунктир"},
                    new {Value = 4, Text = "Паз"},
                    new {Value = 5, Text = "Ребро"},
                    new {Value = 6, Text = "Вкладка"},
                    new {Value = 7, Text = "Накладка"},
                }, selectedValue, dataTextField, value);
        }

        public static SelectList LoginSubSystemSelectList(string selectedValue, string dataTextField, object value)
        {
            var license = Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance<ILicenseInfoService>().GetLicense();

            SelectListItem a;
            if (license.ShowAllSubsystems)
            {
                return new SelectList(
                    new[]
                    {
                        new {data_url = "a", Value = LMExecutingType.LM, Text = "управление профилем"},
                        new {data_url = "a", Value = LMExecutingType.Team, Text = "управление сотрудниками"},
                        new {data_url = "a", Value = LMExecutingType.Cloud, Text = "управление сервером"},
                        new {data_url = "a", Value = LMExecutingType.Communications, Text = "управление почтой"},
                        new {data_url = "a", Value = LMExecutingType.CMS, Text = "управление сайтом"},
                        new {data_url = "a", Value = LMExecutingType.Projects, Text = "управление новостями"},
                        new {data_url = "a", Value = LMExecutingType.CRM, Text = "управление продажами"},
                        new {data_url = "a", Value = LMExecutingType.WMS, Text = "управление адресами"},
                        new {data_url = "a", Value = LMExecutingType.Transit, Text = "управление доставкой"},
                        new {data_url = "a", Value = LMExecutingType.Market, Text = "магазин WebUni"},
                        new {data_url = "a", Value = LMExecutingType.Settings, Text = "настройки WebUni"},
                    }, selectedValue, dataTextField, value);
            }
            else
            {
                return new SelectList(
                    new[]
                    {
                        new {data_url = "a", Value = LMExecutingType.Team, Text = "управление сотрудниками"},
                        new {data_url = "a", Value = LMExecutingType.CMS, Text = "управление сайтом"},
                        new {data_url = "a", Value = LMExecutingType.Projects, Text = "управление новостями"},
                        new {data_url = "a", Value = LMExecutingType.CRM, Text = "управление продажами"},
                        new {data_url = "a", Value = LMExecutingType.WMS, Text = "управление адресами"},
                        new {data_url = "a", Value = LMExecutingType.Settings, Text = "настройки WebUni"},
                    }, selectedValue, dataTextField, value);
            }
        }

        public static MvcHtmlString BasicCheckBoxFor<T>(this HtmlHelper<T> html,
                                               Expression<Func<T, bool>> expression,
                                               object htmlAttributes = null)
        {
            var result = html.CheckBoxFor(expression, htmlAttributes).ToString();
            const string pattern = @"<input name=""[^""]+"" type=""hidden"" value=""false"" />";
            var single = Regex.Replace(result, pattern, "");
            return MvcHtmlString.Create(single);
        }

        #region attr

        public static List<BWPAttribute> GetAttributes(string agCode)
        {
            return ServiceLocator.Current.GetInstance<IAttributeGroupsService>().Get(agCode).Attributes;
        }

        #endregion


        public static string GetLayout(this HttpRequestBase request, string layoutName, params string[] regionIds)
        {
            if (request.Headers["X-PJAX"] != null && regionIds.Contains(request.Headers["X-PJAX-Container"]))
                return MVC.Shared.Views._PjaxLayout;
            else
                return string.Format("{0}", layoutName);
        }

        public static IEnumerable<CatalogItem> GetCatalogItemCodesByType(this HtmlHelper helper, string type)
        {
            var catalogItemTypeRelationService = (ICatalogItemService)ServiceLocator.Current.GetService(typeof(ICatalogItemService));
            return catalogItemTypeRelationService.GetCatalogItemCodesByType(type);
        }

        public static IEnumerable<IMicroModuleBase> GetAllMMByCatalogItemCode(this HtmlHelper helper, string code)
        {
            var microModuleFactory = (IMicroModuleFactory)ServiceLocator.Current.GetService(typeof(IMicroModuleFactory));
            return microModuleFactory.GetAllByCatalogItemCode(code);
        }

        #region rights

        public static bool CheckRights(this HtmlHelper helper, SimpleRightValue srv)
        {
            return ServiceLocator.Current.GetInstance<ISimpleRightsContextScope>().CheckAccess(srv);
        }

        public static void DoIfAccess(this HtmlHelper helper, SimpleRightValue srv, Action action)
        {
            ServiceLocator.Current.GetInstance<ISimpleRightsContextScope>().DoIfAccess(srv, action);
        }

        public static bool CanEditCatalog(long id)
        {
            var cu = ServiceLocator.Current.GetInstance<IAccountManager>().GetCurrentUser();
            if (cu.AccessMode == AccessMode.Admin)
                return true;
            else
            {
                var item = ServiceLocator.Current.GetInstance<IRightsManager>().GetRootCatalogitem(id, cu.RightSubjectID.Value);
                return item.EditAllowed;
            }


        }


        #endregion
    }
}