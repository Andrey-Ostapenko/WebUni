using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Common.Entities;

namespace livemenu.Areas.CMS.Helpers
{
    public static class WebUniUniBlockEditHelper
    {
        public static Dictionary<CatalogItemTypeValue, Func<long, ActionResult>> WebUniLoaders = new Dictionary<CatalogItemTypeValue, Func<long, ActionResult>>
        {
            { CatalogItemTypeValue.Site, (id) =>  MVC.CMS.WebUni.Site(id)  },
            { CatalogItemTypeValue.Version, (id) =>  MVC.CMS.WebUni.Version(id)  },
            { CatalogItemTypeValue.Filter, (id) =>  MVC.CMS.WebUni.Filter(id)  },
            { CatalogItemTypeValue.Page, (id) =>  MVC.CMS.WebUni.Page(id)  },
            { CatalogItemTypeValue.Line, (id) =>  MVC.CMS.WebUni.Line(id)  },
            { CatalogItemTypeValue.Section, (id) =>  MVC.CMS.WebUni.Section(id)  },
            { CatalogItemTypeValue.Caption, (id) =>  MVC.CMS.WebUni.Caption(id)  },
            { CatalogItemTypeValue.Text, (id) =>  MVC.CMS.WebUni.Text(id)  },
            { CatalogItemTypeValue.Graphics, (id) =>  MVC.CMS.WebUni.Graphics(id)  },
            { CatalogItemTypeValue.Cart, (id) =>  MVC.CMS.WebUni.Cart(id)  },
            { CatalogItemTypeValue.CartList, (id) =>  MVC.CMS.WebUni.CartList(id)  },
            { CatalogItemTypeValue.Login, (id) =>  MVC.CMS.WebUni.Login(id)  },
            { CatalogItemTypeValue.Register, (id) =>  MVC.CMS.WebUni.Register(id)  },
            { CatalogItemTypeValue.Checkout, (id) =>  MVC.CMS.WebUni.Checkout(id)  },
            { CatalogItemTypeValue.ProfileName, (id) =>  MVC.CMS.WebUni.ProfileName(id)  },

            { CatalogItemTypeValue.CheckoutWithoutRegistration, (id) =>  MVC.CMS.WebUni.CheckoutWithoutRegistration(id)  },
            { CatalogItemTypeValue.Profile, (id) =>  MVC.CMS.WebUni.Profile(id)  },

            { CatalogItemTypeValue.Price, (id) =>  MVC.CMS.WebUni.Price(id)  },
            { CatalogItemTypeValue.Forgot, (id) =>  MVC.CMS.WebUni.Forgot(id)  },

            { CatalogItemTypeValue.Panels, (id) =>  MVC.CMS.WebUni.Panels(id)  },
            { CatalogItemTypeValue.Header, (id) =>  MVC.CMS.WebUni.Header(id)  },
            { CatalogItemTypeValue.Footer, (id) =>  MVC.CMS.WebUni.Footer(id)  },
            { CatalogItemTypeValue.LeftSideBar, (id) =>  MVC.CMS.WebUni.LeftSideBar(id)  },
            { CatalogItemTypeValue.RightSideBar, (id) =>  MVC.CMS.WebUni.RightSideBar(id)  },

            { CatalogItemTypeValue.Lists, (id) =>  MVC.CMS.WebUni.Lists(id)  },
            { CatalogItemTypeValue.Buttons, (id) =>  MVC.CMS.WebUni.Buttons(id)  },
            { CatalogItemTypeValue.Row, (id) =>  MVC.CMS.WebUni.Row(id)  },
            { CatalogItemTypeValue.Menu, (id) =>  MVC.CMS.WebUni.Menu(id)  },
            { CatalogItemTypeValue.Layouts, (id) =>  MVC.CMS.WebUni.Layouts(id)  },
            { CatalogItemTypeValue.Pages, (id) =>  MVC.CMS.WebUni.Pages(id)  },
            { CatalogItemTypeValue.Elements, (id) =>  MVC.CMS.WebUni.Elements(id)  },
            { CatalogItemTypeValue.Solutions, (id) =>  MVC.CMS.WebUni.Solutions(id)  },
            { CatalogItemTypeValue.Projects, (id) =>  MVC.CMS.WebUni.Projects(id)  },
            { CatalogItemTypeValue.Actions, (id) =>  MVC.CMS.WebUni.UniActions(id)  },
            { CatalogItemTypeValue.Vacancies, (id) =>  MVC.CMS.WebUni.Vacancies(id)  },
            { CatalogItemTypeValue.SitePanel, (id) => MVC.CMS.WebUni.UniSitePanel(id) },
            { CatalogItemTypeValue.Goods, (id) =>  MVC.CMS.WebUni.Goods(id)  },
            { CatalogItemTypeValue.OrderHistory, (id) =>  MVC.CMS.WebUni.OrderHistory(id)  },
            { CatalogItemTypeValue.Gallery, (id) =>  MVC.CMS.WebUni.Gallery(id)  },
            { CatalogItemTypeValue.Photo, (id) =>  MVC.CMS.WebUni.Photo(id)  },
            { CatalogItemTypeValue.SearchFilter, (id) =>  MVC.CMS.WebUni.SearchFilter(id)  },
            { CatalogItemTypeValue.SearchFilterGroup, (id) =>  MVC.CMS.WebUni.SearchFilterGroup(id)  },

            { CatalogItemTypeValue.Balance, (id) =>  MVC.CMS.WebUni.Balance(id)  },
            { CatalogItemTypeValue.BalanceCharge, (id) =>  MVC.CMS.WebUni.BalanceCharge(id)  },
            { CatalogItemTypeValue.BalanceList, (id) =>  MVC.CMS.WebUni.BalanceList(id)  },
            { CatalogItemTypeValue.DropDownList, (id) =>  MVC.CMS.WebUni.DropDownList(id)  },

            { CatalogItemTypeValue.Docs, (id) =>  MVC.CMS.WebUni.Docs(id)  },
            { CatalogItemTypeValue.Postcards, (id) =>  MVC.CMS.WebUni.Postcards(id)  },
            { CatalogItemTypeValue.News, (id) =>  MVC.CMS.WebUni.News(id)  },
            { CatalogItemTypeValue.Tasks, (id) =>  MVC.CMS.WebUni.Tasks(id)  },

            { CatalogItemTypeValue.Ideas, (id) =>  MVC.CMS.WebUni.Ideas(id)  },
            { CatalogItemTypeValue.Plans, (id) =>  MVC.CMS.WebUni.Plans(id)  },
            { CatalogItemTypeValue.AppLists, (id) =>  MVC.CMS.WebUni.AppLists(id)  },
            { CatalogItemTypeValue.Places, (id) =>  MVC.CMS.WebUni.Places(id)  },
            { CatalogItemTypeValue.Calendar, (id) =>  MVC.CMS.WebUni.Calendar(id)  },
            { CatalogItemTypeValue.Events, (id) =>  MVC.CMS.WebUni.Events(id)  },
            { CatalogItemTypeValue.Reserve, (id) =>  MVC.CMS.WebUni.Reserve(id)  },
            { CatalogItemTypeValue.Print, (id) =>  MVC.CMS.WebUni.Print(id)  },
            { CatalogItemTypeValue.Research, (id) =>  MVC.CMS.WebUni.Research(id)  },
            { CatalogItemTypeValue.Ads, (id) =>  MVC.CMS.WebUni.Ads(id)  },
            { CatalogItemTypeValue.Map, (id) =>  MVC.CMS.WebUni.Map(id)  },
            { CatalogItemTypeValue.Input, (id) =>  MVC.CMS.WebUni.Input(id)  },
            { CatalogItemTypeValue.ListSelector, (id) =>  MVC.CMS.WebUni.ListSelector(id)  },
            { CatalogItemTypeValue.Application, (id) =>  MVC.CMS.WebUni.Application(id)  },
            { CatalogItemTypeValue.Project, (id) =>  MVC.CMS.WebUni.Project(id)  },
            { CatalogItemTypeValue.Webinar, (id) =>  MVC.CMS.WebUni.Webinar(id)  },
            { CatalogItemTypeValue.Storage, (id) =>  MVC.CMS.WebUni.Storage(id)  },

        };

        public static Dictionary<CatalogItemTypeValue, Func<long, ActionResult>> WebUniDataLoaders = new Dictionary<CatalogItemTypeValue, Func<long, ActionResult>>
        {
            { CatalogItemTypeValue.Site, (id) =>  MVC.CMS.WebUni.SiteData(id)  },
            { CatalogItemTypeValue.Filter, (id) =>  MVC.CMS.WebUni.FilterData(id)  },
            { CatalogItemTypeValue.Page, (id) =>  MVC.CMS.WebUni.PageData(id)  },
            { CatalogItemTypeValue.Line, (id) =>  MVC.CMS.WebUni.LineData(id)  },
            { CatalogItemTypeValue.Section, (id) =>  MVC.CMS.WebUni.SectionData(id)  },
            { CatalogItemTypeValue.Text, (id) =>  MVC.CMS.WebUni.TextData(id)  },
            { CatalogItemTypeValue.Graphics, (id) =>  MVC.CMS.WebUni.GraphicsData(id)  },
            { CatalogItemTypeValue.Caption, (id) =>  MVC.CMS.WebUni.CaptionData(id)  },
            { CatalogItemTypeValue.Addresses, (id) =>  MVC.CMS.WebUni.AddressesData(id)  },
            { CatalogItemTypeValue.Video, (id) =>  MVC.CMS.WebUni.VideoData(id)  },
            { CatalogItemTypeValue.Cart, (id) =>  MVC.CMS.WebUni.CartData(id)  },

            { CatalogItemTypeValue.CartList, (id) =>  MVC.CMS.WebUni.CartListData(id)  },
            { CatalogItemTypeValue.Login, (id) =>  MVC.CMS.WebUni.LoginData(id)  },
            { CatalogItemTypeValue.Register, (id) =>  MVC.CMS.WebUni.RegisterData(id)  },
            { CatalogItemTypeValue.Checkout, (id) =>  MVC.CMS.WebUni.CheckoutData(id)  },
            { CatalogItemTypeValue.ProfileName, (id) =>  MVC.CMS.WebUni.ProfileNameData(id)  },

            { CatalogItemTypeValue.CheckoutWithoutRegistration, (id) =>  MVC.CMS.WebUni.CheckoutWithoutRegistrationData(id)  },
            { CatalogItemTypeValue.Profile, (id) =>  MVC.CMS.WebUni.ProfileData(id)  },

            { CatalogItemTypeValue.Price, (id) =>  MVC.CMS.WebUni.PriceData(id)  },
            { CatalogItemTypeValue.Forgot, (id) =>  MVC.CMS.WebUni.ForgotData(id)  },

            { CatalogItemTypeValue.Panels, (id) =>  MVC.CMS.WebUni.PanelsData(id)  },
            { CatalogItemTypeValue.Header, (id) =>  MVC.CMS.WebUni.HeaderData(id)  },
            { CatalogItemTypeValue.Footer, (id) =>  MVC.CMS.WebUni.FooterData(id)  },
            { CatalogItemTypeValue.LeftSideBar, (id) =>  MVC.CMS.WebUni.LeftSideBarData(id)  },
            { CatalogItemTypeValue.RightSideBar, (id) =>  MVC.CMS.WebUni.RightSideBarData(id)  },

            { CatalogItemTypeValue.Lists, (id) =>  MVC.CMS.WebUni.ListsData(id)  },
            { CatalogItemTypeValue.Buttons, (id) =>  MVC.CMS.WebUni.ButtonsData(id)  },
            { CatalogItemTypeValue.Row, (id) =>  MVC.CMS.WebUni.RowData(id)  },
            { CatalogItemTypeValue.Menu, (id) =>  MVC.CMS.WebUni.MenuData(id)  },

            { CatalogItemTypeValue.Layouts, (id) =>  MVC.CMS.WebUni.LayoutsData(id)  },
            { CatalogItemTypeValue.Pages, (id) =>  MVC.CMS.WebUni.PagesData(id)  },
            { CatalogItemTypeValue.Elements, (id) =>  MVC.CMS.WebUni.ElementsData(id)  },
            { CatalogItemTypeValue.Projects, (id) =>  MVC.CMS.WebUni.ProjectsData(id)  },
            { CatalogItemTypeValue.Solutions, (id) =>  MVC.CMS.WebUni.SolutionsData(id)  },
            { CatalogItemTypeValue.Actions, (id) =>  MVC.CMS.WebUni.ActionsData(id)  },
            { CatalogItemTypeValue.SitePanel, (id) => MVC.CMS.WebUni.SitePanelData(id) },
            { CatalogItemTypeValue.Vacancies, (id) =>  MVC.CMS.WebUni.VacanciesData(id)  },
            { CatalogItemTypeValue.Goods, (id) =>  MVC.CMS.WebUni.GoodsData(id)  },
            { CatalogItemTypeValue.OrderHistory, (id) =>  MVC.CMS.WebUni.OrderHistoryData(id)  },
            { CatalogItemTypeValue.Gallery, (id) =>  MVC.CMS.WebUni.GalleryData(id)  },
            { CatalogItemTypeValue.Photo, (id) =>  MVC.CMS.WebUni.PhotoData(id)  },
            { CatalogItemTypeValue.SearchFilter, (id) =>  MVC.CMS.WebUni.SearchFilterData(id)  },
            { CatalogItemTypeValue.SearchFilterGroup, (id) =>  MVC.CMS.WebUni.SearchFilterGroupData(id)  },

            { CatalogItemTypeValue.Balance, (id) =>  MVC.CMS.WebUni.BalanceData(id)  },
            { CatalogItemTypeValue.BalanceCharge, (id) =>  MVC.CMS.WebUni.BalanceChargeData(id)  },
            { CatalogItemTypeValue.BalanceList, (id) =>  MVC.CMS.WebUni.BalanceListData(id)  },
            { CatalogItemTypeValue.DropDownList, (id) =>  MVC.CMS.WebUni.DropDownListData(id)  },

             { CatalogItemTypeValue.Docs, (id) =>  MVC.CMS.WebUni.DocsData(id)  },
            { CatalogItemTypeValue.Postcards, (id) =>  MVC.CMS.WebUni.PostcardsData(id)  },
            { CatalogItemTypeValue.News, (id) =>  MVC.CMS.WebUni.NewsData(id)  },
            { CatalogItemTypeValue.Tasks, (id) =>  MVC.CMS.WebUni.TasksData(id)  },




            { CatalogItemTypeValue.Ideas, (id) =>  MVC.CMS.WebUni.IdeasData(id)  },
            { CatalogItemTypeValue.Plans, (id) =>  MVC.CMS.WebUni.PlansData(id)  },
            { CatalogItemTypeValue.AppLists, (id) =>  MVC.CMS.WebUni.AppListsData(id)  },
            { CatalogItemTypeValue.Places, (id) =>  MVC.CMS.WebUni.PlacesData(id)  },
            { CatalogItemTypeValue.Calendar, (id) =>  MVC.CMS.WebUni.CalendarData(id)  },
            { CatalogItemTypeValue.Events, (id) =>  MVC.CMS.WebUni.EventsData(id)  },
            { CatalogItemTypeValue.Reserve, (id) =>  MVC.CMS.WebUni.ReserveData(id)  },
            { CatalogItemTypeValue.Print, (id) =>  MVC.CMS.WebUni.PrintData(id)  },
            { CatalogItemTypeValue.Research, (id) =>  MVC.CMS.WebUni.ResearchData(id)  },
            { CatalogItemTypeValue.Ads, (id) =>  MVC.CMS.WebUni.AdsData(id)  },

            { CatalogItemTypeValue.Map, (id) =>  MVC.CMS.WebUni.MapData(id)  },
            { CatalogItemTypeValue.Input, (id) =>  MVC.CMS.WebUni.InputData(id)  },
            { CatalogItemTypeValue.ListSelector, (id) =>  MVC.CMS.WebUni.ListSelectorData(id)  },
            { CatalogItemTypeValue.Application, (id) =>  MVC.CMS.WebUni.ApplicationData(id)  },
            { CatalogItemTypeValue.Project, (id) =>  MVC.CMS.WebUni.ProjectData(id)  },
            { CatalogItemTypeValue.Webinar, (id) =>  MVC.CMS.WebUni.WebinarData(id)  },
            { CatalogItemTypeValue.Storage, (id) =>  MVC.CMS.WebUni.StorageData(id)  },
        };

        public static Dictionary<CatalogItemTypeValue, Func<long, ActionResult>> WebUniShopLoaders = new Dictionary<CatalogItemTypeValue, Func<long, ActionResult>>
        {
            { CatalogItemTypeValue.Line, (id) =>  MVC.CMS.WebUni.LineShop(id)  },
        };


        public static Dictionary<CatalogItemTypeValue, Func<long, ActionResult>> WebUniDesignLoaders = new Dictionary<CatalogItemTypeValue, Func<long, ActionResult>>
        {
            { CatalogItemTypeValue.Site, (id) =>  MVC.CMS.WebUni.SiteData(id)  },
            { CatalogItemTypeValue.Filter, (id) =>  MVC.CMS.WebUni.FilterDesign(id)  },
            //{ CatalogItemTypeValue.Page, (id) =>  MVC.CMS.WebUni.PageData(id)  },
            { CatalogItemTypeValue.Line, (id) =>  MVC.CMS.WebUni.LineDesign(id)  },
            //{ CatalogItemTypeValue.Section, (id) =>  MVC.CMS.WebUni.SectionData(id)  },
            { CatalogItemTypeValue.Text, (id) =>  MVC.CMS.WebUni.TextDesign(id)  },
            //{ CatalogItemTypeValue.Graphics, (id) =>  MVC.CMS.WebUni.GraphicsData(id)  },

            { CatalogItemTypeValue.Caption, (id) =>  MVC.CMS.WebUni.CaptionDesign(id)  },
              { CatalogItemTypeValue.Cart, (id) =>  MVC.CMS.WebUni.CartDesign(id)  },
            //{ CatalogItemTypeValue.Addresses, (id) =>  MVC.CMS.WebUni.AddressesData(id)  },
            //{ CatalogItemTypeValue.Video, (id) =>  MVC.CMS.WebUni.VideoData(id)  },
            { CatalogItemTypeValue.CartList, (id) =>  MVC.CMS.WebUni.CartListDesign(id)  },
            { CatalogItemTypeValue.Login, (id) =>  MVC.CMS.WebUni.LoginDesign(id)  },
            { CatalogItemTypeValue.Register, (id) =>  MVC.CMS.WebUni.RegisterDesign(id)  },
            { CatalogItemTypeValue.Checkout, (id) =>  MVC.CMS.WebUni.CheckoutDesign(id)  },
            { CatalogItemTypeValue.ProfileName, (id) =>  MVC.CMS.WebUni.ProfileNameDesign(id)  },

            { CatalogItemTypeValue.CheckoutWithoutRegistration, (id) =>  MVC.CMS.WebUni.CheckoutWithoutRegistrationDesign(id)  },
            { CatalogItemTypeValue.Profile, (id) =>  MVC.CMS.WebUni.ProfileDesign(id)  },

            { CatalogItemTypeValue.Price, (id) =>  MVC.CMS.WebUni.PriceDesign(id)  },
            { CatalogItemTypeValue.Forgot, (id) =>  MVC.CMS.WebUni.ForgotDesign(id)  },

            { CatalogItemTypeValue.Panels, (id) =>  MVC.CMS.WebUni.PanelsDesign(id)  },
            { CatalogItemTypeValue.Header, (id) =>  MVC.CMS.WebUni.HeaderDesign(id)  },
            { CatalogItemTypeValue.Footer, (id) =>  MVC.CMS.WebUni.FooterDesign(id)  },
            { CatalogItemTypeValue.LeftSideBar, (id) =>  MVC.CMS.WebUni.LeftSideBarDesign(id)  },
            { CatalogItemTypeValue.RightSideBar, (id) =>  MVC.CMS.WebUni.RightSideBarDesign(id)  },

            { CatalogItemTypeValue.Lists, (id) =>  MVC.CMS.WebUni.ListsDesign(id)  },
            { CatalogItemTypeValue.Buttons, (id) =>  MVC.CMS.WebUni.ButtonsDesign(id)  },

            { CatalogItemTypeValue.Row, (id) =>  MVC.CMS.WebUni.RowDesign(id)  },
            { CatalogItemTypeValue.Menu, (id) =>  MVC.CMS.WebUni.MenuDesign(id)  },

            { CatalogItemTypeValue.Layouts, (id) =>  MVC.CMS.WebUni.LayoutsDesign(id)  },
            { CatalogItemTypeValue.Pages, (id) =>  MVC.CMS.WebUni.PagesDesign(id)  },
            { CatalogItemTypeValue.Elements, (id) =>  MVC.CMS.WebUni.ElementsDesign(id)  },
            { CatalogItemTypeValue.Projects, (id) =>  MVC.CMS.WebUni.ProjectsDesign(id)  },
            { CatalogItemTypeValue.Solutions, (id) =>  MVC.CMS.WebUni.SolutionsDesign(id)  },
            { CatalogItemTypeValue.Actions, (id) =>  MVC.CMS.WebUni.ActionsDesign(id)  },
            { CatalogItemTypeValue.SitePanel, (id) => MVC.CMS.WebUni.SitePanelDesign(id) },
            { CatalogItemTypeValue.Vacancies, (id) =>  MVC.CMS.WebUni.VacanciesDesign(id)  },
            { CatalogItemTypeValue.Goods, (id) =>  MVC.CMS.WebUni.GoodsDesign(id)  },
            { CatalogItemTypeValue.OrderHistory, (id) =>  MVC.CMS.WebUni.OrderHistoryDesign(id)  },
            { CatalogItemTypeValue.Gallery, (id) =>  MVC.CMS.WebUni.GalleryDesign(id)  },
            { CatalogItemTypeValue.Photo, (id) =>  MVC.CMS.WebUni.PhotoDesign(id)  },
            { CatalogItemTypeValue.SearchFilter, (id) =>  MVC.CMS.WebUni.SearchFilterDesign(id)  },
            { CatalogItemTypeValue.SearchFilterGroup, (id) =>  MVC.CMS.WebUni.SearchFilterGroupDesign(id)  },

            { CatalogItemTypeValue.Balance, (id) =>  MVC.CMS.WebUni.BalanceDesign(id)  },
            { CatalogItemTypeValue.BalanceCharge, (id) =>  MVC.CMS.WebUni.BalanceChargeDesign(id)  },
            { CatalogItemTypeValue.BalanceList, (id) =>  MVC.CMS.WebUni.BalanceListDesign(id)  },
            { CatalogItemTypeValue.DropDownList, (id) =>  MVC.CMS.WebUni.DropDownListDesign(id)  },

            { CatalogItemTypeValue.Docs, (id) =>  MVC.CMS.WebUni.DocsDesign(id)  },
            { CatalogItemTypeValue.Postcards, (id) =>  MVC.CMS.WebUni.PostcardsDesign(id)  },
            { CatalogItemTypeValue.News, (id) =>  MVC.CMS.WebUni.NewsDesign(id)  },
            { CatalogItemTypeValue.Tasks, (id) =>  MVC.CMS.WebUni.TasksDesign(id)  },


            { CatalogItemTypeValue.Ideas, (id) =>  MVC.CMS.WebUni.IdeasDesign(id)  },
            { CatalogItemTypeValue.Plans, (id) =>  MVC.CMS.WebUni.PlansDesign(id)  },
            { CatalogItemTypeValue.AppLists, (id) =>  MVC.CMS.WebUni.AppListsDesign(id)  },
            { CatalogItemTypeValue.Places, (id) =>  MVC.CMS.WebUni.PlacesDesign(id)  },
            { CatalogItemTypeValue.Calendar, (id) =>  MVC.CMS.WebUni.CalendarDesign(id)  },
            { CatalogItemTypeValue.Events, (id) =>  MVC.CMS.WebUni.EventsDesign(id)  },
            { CatalogItemTypeValue.Reserve, (id) =>  MVC.CMS.WebUni.ReserveDesign(id)  },
            { CatalogItemTypeValue.Print, (id) =>  MVC.CMS.WebUni.PrintDesign(id)  },
            { CatalogItemTypeValue.Research, (id) =>  MVC.CMS.WebUni.ResearchDesign(id)  },
            { CatalogItemTypeValue.Ads, (id) =>  MVC.CMS.WebUni.AdsDesign(id)  },

            { CatalogItemTypeValue.Map, (id) =>  MVC.CMS.WebUni.MapDesign(id)  },
            { CatalogItemTypeValue.Input, (id) =>  MVC.CMS.WebUni.InputDesign(id)  },
            { CatalogItemTypeValue.ListSelector, (id) =>  MVC.CMS.WebUni.ListSelectorDesign(id)  },
            { CatalogItemTypeValue.Application, (id) =>  MVC.CMS.WebUni.ApplicationDesign(id)  },
            { CatalogItemTypeValue.Project, (id) =>  MVC.CMS.WebUni.ProjectDesign(id)  },
            { CatalogItemTypeValue.Webinar, (id) =>  MVC.CMS.WebUni.WebinarDesign(id)  },
            { CatalogItemTypeValue.Storage, (id) =>  MVC.CMS.WebUni.StorageDesign(id)  },
        };
    }
}