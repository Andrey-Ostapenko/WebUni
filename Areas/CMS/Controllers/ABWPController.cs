using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.CMS.Helpers;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Excel.CMS;
using livemenu.Kernel.BWP;
using livemenu.Kernel.BWP.Actions;
using livemenu.Kernel.BWP.SitePanel;
using livemenu.Kernel.BWP.Adresses;
using livemenu.Kernel.BWP.Buttons;
using livemenu.Kernel.BWP.Caption;
using livemenu.Kernel.BWP.Cart;
using livemenu.Kernel.BWP.CartList;
using livemenu.Kernel.BWP.Checkout;
using livemenu.Kernel.BWP.CheckoutWithoutRegistration;
using livemenu.Kernel.BWP.Application;
using livemenu.Kernel.BWP.Filter;
using livemenu.Kernel.BWP.SearchFilter;
using livemenu.Kernel.BWP.Footer;
using livemenu.Kernel.BWP.Forgot;
using livemenu.Kernel.BWP.Gallery;
using livemenu.Kernel.BWP.Goods;
using livemenu.Kernel.BWP.Graphics;
using livemenu.Kernel.BWP.Header;
using livemenu.Kernel.BWP.Layouts;
using livemenu.Kernel.BWP.LeftSideBar;
using livemenu.Kernel.BWP.Line;
using livemenu.Kernel.BWP.Lists;
using livemenu.Kernel.BWP.Login;
using livemenu.Kernel.BWP.Menu;
using livemenu.Kernel.BWP.OrderHistory;
using livemenu.Kernel.BWP.Pages;
using livemenu.Kernel.BWP.Panels;
using livemenu.Kernel.BWP.Photo;
using livemenu.Kernel.BWP.Price;
using livemenu.Kernel.BWP.Profile;
using livemenu.Kernel.BWP.ProfileName;
using livemenu.Kernel.BWP.Projects;
using livemenu.Kernel.BWP.Register;
using livemenu.Kernel.BWP.RightSideBar;
using livemenu.Kernel.BWP.Row;
using livemenu.Kernel.BWP.Section;
using livemenu.Kernel.BWP.Site;
using livemenu.Kernel.BWP.Text;
using livemenu.Kernel.BWP.Vacancies;
using livemenu.Kernel.BWP.Video;
using Microsoft.Practices.ServiceLocation;
using AutoMapper;
using livemenu.Kernel.BWP.Ads;
using livemenu.Kernel.BWP.AppLists;
using livemenu.Kernel.BWP.Balance;
using livemenu.Kernel.BWP.BalanceCharge;
using livemenu.Kernel.BWP.BalanceList;
using livemenu.Kernel.BWP.Calendar;
using livemenu.Kernel.BWP.Docs;
using livemenu.Kernel.BWP.DropDownList;
using livemenu.Kernel.BWP.Elements;
using livemenu.Kernel.BWP.Events;
using livemenu.Kernel.BWP.Ideas;
using livemenu.Kernel.BWP.Map;
using livemenu.Kernel.BWP.News;
using livemenu.Kernel.BWP.Places;
using livemenu.Kernel.BWP.Plans;
using livemenu.Kernel.BWP.Postcards;
using livemenu.Kernel.BWP.Print;
using livemenu.Kernel.BWP.Research;
using livemenu.Kernel.BWP.Reserve;
using livemenu.Kernel.BWP.Solutions;
using livemenu.Kernel.BWP.Tasks;
using livemenu.Kernel.BWP.Versions;
using livemenu.Kernel.BWP.Input;
using livemenu.Kernel.BWP.ListSelector;
using livemenu.Kernel.BWP.Project;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class WebUniController : CMSController
    {
        private readonly IWebUniExporter _WebUniexporter;

        public WebUniController(IWebUniExporter WebUniexporter)
        {
            _WebUniexporter = WebUniexporter;
        }

        public virtual ActionResult LoadData(long id, long type)
        {
            ModelState.Clear();
            return RedirectToAction(WebUniUniBlockEditHelper.WebUniDataLoaders[(CatalogItemTypeValue) type](id));
        }

        public virtual ActionResult Export(long id)
        {
            
            string myName = Server.UrlEncode("TEST" + "_" + DateTime.Now.ToShortDateString() + ".xlsx");
            MemoryStream stream = _WebUniexporter.Export(id);

           // Response.Clear();
           // Response.Buffer = true;
           // Response.AddHeader("content-disposition", "attachment; filename=" + myName);
           // Response.ContentType = "application/vnd.ms-excel";
           // Response.BinaryWrite(stream.ToArray());
           // Response.End();
            return new FileStreamResult(stream, "application/vnd.ms-excel")
            {
                FileDownloadName = myName
            };
        }
        public virtual ActionResult Site(long id)
        {
            return View(id);
        }

        public virtual ActionResult SiteData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSiteService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult SiteDataSave(UniSite site)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSiteService>();

            UpdateWrapper(() =>
            {
                site = service.UpdateData(site);
            });

            return View(MVC.CMS.WebUni.Views.SiteDataInternal, site);
        }

        public virtual ActionResult Version(long id)
        {
            return View(id);
        }

        public virtual ActionResult VersionData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniVersionService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult VersionDataSave(UniVersion version)
        {
            var service = ServiceLocator.Current.GetInstance<IUniVersionService>();

            UpdateWrapper(() =>
            {
                version = service.UpdateData(version);
            });

            return View(MVC.CMS.WebUni.Views.VersionDataInternal, version);
        }

        public virtual ActionResult Page(long id)
        {
            return View(id);
        }

        public virtual ActionResult PageData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPageService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult PageDataSave(UniPage page)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPageService>();

            UpdateWrapper(() =>
            {
                page = service.UpdateData(page);
            });

            return View(MVC.CMS.WebUni.Views.PageDataInternal, page);
        }

        public virtual ActionResult PageDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPageService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PageDesignSave(UniPage t)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPageService>();

            UpdateWrapper(() =>
            {
                t = service.UpdateDesign(t);
            });

            return View(MVC.CMS.WebUni.Views.PageDesignInternal, t);
        }

        public virtual ActionResult PageSEO(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPageService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult PageSEOSave(UniPage page)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPageService>();

            UpdateWrapper(() =>
            {
                page = service.UpdateSEO(page);
            });

            return View(MVC.CMS.WebUni.Views.PageSEOInternal, page);
        }


        public virtual ActionResult Graphics(long id)
        {
            return View(id);
        }

        public virtual ActionResult GraphicsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniGraphicsService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult GraphicsItemData(long grID, long? id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniGraphicsService>();
            return View(service.GetGraphicsItemData(grID, id));
        }


        

        public virtual JsonResult DeleteUniGraphicsItem(long id)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGraphicsService>();
            bool result = false;
            UpdateWrapper(() =>
            {
                service.DeleteUniGraphicsItem(id);
                result = true;
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult GraphicsItemDataSave(UniGraphicsItem graphicsItem)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGraphicsService>();

            UpdateWrapper(() =>
            {
                graphicsItem = service.UpdateItemData(graphicsItem);
            });

            return View(MVC.CMS.WebUni.Views.GraphicsItemData, graphicsItem);
        }

        public virtual ActionResult GraphicsDataSave(UniGraphics graphics)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGraphicsService>();

            UpdateWrapper(() =>
            {
                graphics = service.UpdateData(graphics);
            });

            return View(MVC.CMS.WebUni.Views.GraphicsDataInternal, graphics);
        }


        public virtual ActionResult TextDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniTextService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult TextDesignSave(UniText t)
        {
            var service = ServiceLocator.Current.GetInstance<IUniTextService>();

            UpdateWrapper(() =>
            {
                t = service.UpdateDesign(t);
            });

            return View(MVC.CMS.WebUni.Views.TextDesignInternal, t);
        }

        public virtual ActionResult TextData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniTextService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult TextDataSave(UniText t)
        {
            var service = ServiceLocator.Current.GetInstance<IUniTextService>();

            UpdateWrapper(() =>
            {
                t = service.UpdateData(t);
            });

            return View(MVC.CMS.WebUni.Views.TextDataInternal, t);
        }
        public virtual ActionResult Caption(long id)
        {
            return View(id);
        }

        public virtual ActionResult CaptionData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCaptionService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CaptionDataSave(UniCaption caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCaptionService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.CaptionDataInternal, caption);
        }

        public virtual ActionResult CaptionDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCaptionService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CaptionDesignSave(UniCaption caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCaptionService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.CaptionDesignInternal, caption);
        }

        public virtual ActionResult VideoData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniVideoService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult VideoDataSave(UniVideo video)
        {
            var service = ServiceLocator.Current.GetInstance<IUniVideoService>();

            UpdateWrapper(() =>
            {
                video = service.UpdateData(video);
            });

            return View(MVC.CMS.WebUni.Views.VideoDataInternal, video);
        }

        public virtual ActionResult AddressesData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniAddressesService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult AddressesDataSave(UniAddresses addresses)
        {
            var service = ServiceLocator.Current.GetInstance<IUniAddressesService>();

            UpdateWrapper(() =>
            {
                addresses = service.UpdateData(addresses);
            });

            return View(MVC.CMS.WebUni.Views.AddressesDataInternal, addresses);
        }

        public virtual ActionResult LineDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLineService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LineDesignSave(UniLine line)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLineService>();

            UpdateWrapper(() =>
            {
                line = service.UpdateDesign(line);
            });
            
            return Json(Mapper.DynamicMap<UniLineExport>(line));
        }

        public virtual ActionResult LineData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLineService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult LineDataSave(UniLine line)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLineService>();

            UpdateWrapper(() =>
            {
                line = service.UpdateData(line);
            });

            return Json(Mapper.DynamicMap<UniLineExport>(line));
        }

        public virtual ActionResult LineShop(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLineService>();
            return View(service.GetByCatalogItemID(id));
        }


        public virtual ActionResult LineShopSave(UniLine line)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLineService>();

            UpdateWrapper(() =>
            {
                line = service.UpdateShop(line);
            });

            return View(MVC.CMS.WebUni.Views.LineShopInternal, line);
        }

        
        public virtual ActionResult SectionDesignSave(UniSection section)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSectionService>();

            UpdateWrapper(() =>
            {
                section = service.UpdateDesign(section);
            });

            return View(MVC.CMS.WebUni.Views.SectionDesignInternal, section);
        }

        public virtual ActionResult SectionDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSectionService>();
            return View(service.GetByCatalogItemID(id));
        }
        public virtual ActionResult SectionDataSave(UniSection section)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSectionService>();

            UpdateWrapper(() =>
            {
                section = service.UpdateData(section);
            });

            return View(MVC.CMS.WebUni.Views.SectionDataInternal, section);
        }

        public virtual ActionResult SectionData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSectionService>();
            return View(service.GetByCatalogItemID(id));
        }

        #region UniLine

        public virtual ActionResult Line(long id)
        {
            ModelState.Clear();

            return View(id);
        }

        public virtual ActionResult LineSave(UniLine line)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLineService>();

            UpdateWrapper(() =>
            {
                line = service.Update(line);
            });

            return View(MVC.CMS.WebUni.Views.LineInternal, line);
        }

        public virtual ActionResult Section(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSectionService>();
            return View(service.GetByCatalogItemID(id));
        }

        public virtual ActionResult SectionSave(UniSection line)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSectionService>();

            UpdateWrapper(() =>
            {
                line = service.Update(line);
            });

            return View(MVC.CMS.WebUni.Views.SectionInternal, line);
        }

        public virtual ActionResult Text(long id)
        {
            return View(id);
        }

        public virtual ActionResult TextSave(UniText line)
        {
            var service = ServiceLocator.Current.GetInstance<IUniTextService>();

            UpdateWrapper(() =>
            {
                line = service.Update(line);
            });

            return View(MVC.CMS.WebUni.Views.TextInternal, line);
        }

        #endregion

        #region UniFilter

        public virtual ActionResult Filter(long id)
        {
            return View(id);
        }

        public virtual ActionResult FilterData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniFilterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult FilterDataSave(UniFilter caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniFilterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.FilterDataInternal, caption);
        }

        public virtual ActionResult FilterDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniFilterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult FilterDesignSave(UniFilter caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniFilterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.FilterDesignInternal, caption);
        }

        #endregion

        #region UniSearchFilter

        public virtual ActionResult SearchFilter(long id)
        {
            return View(id);
        }

        public virtual ActionResult SearchFilterData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SearchFilterDataSave(UniSearchFilter caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.SearchFilterDataInternal, caption);
        }

        public virtual ActionResult SearchFilterDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SearchFilterDesignSave(UniSearchFilter caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.SearchFilterDesignInternal, caption);
        }

        #endregion

        #region UniSearchFilterGroup

        public virtual ActionResult SearchFilterGroup(long id)
        {
            return View(id);
        }

        public virtual ActionResult SearchFilterGroupData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterGroupService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SearchFilterGroupDataSave(UniSearchFilterGroup caption, long[] searchFilterIds)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterGroupService>();

            caption.UniSearchFilterGroupRelation.Clear();
            if(searchFilterIds != null)
            {
                Array.Reverse(searchFilterIds);
                int i = 0;
                foreach (var filterId in searchFilterIds)
                {
                    caption.UniSearchFilterGroupRelation.Add(new UniSearchFilterGroupRelation
                    {
                        SearchFilterGroupID = caption.ID,
                        SearchFilterID = filterId,
                        Priority = i
                    });
                    i++;
                }
            }

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.SearchFilterGroupDataInternal, caption);
        }

        public virtual ActionResult SearchFilterGroupDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSearchFilterGroupService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SearchFilterGroupDesignSave(UniSearchFilterGroup caption)
        {
            // SearchFilterGroup have not designe
            //var service = ServiceLocator.Current.GetInstance<IUniSearchFilterGroupService>();
            //
            //UpdateWrapper(() =>
            //{
            //    caption = service.UpdateDesign(caption);
            //});

            return View(MVC.CMS.WebUni.Views.SearchFilterGroupDesignInternal, caption);
        }

        #endregion

        #region UniCart

        public virtual ActionResult Cart(long id)
        {
            return View(id);
        }

        public virtual ActionResult CartData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCartService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CartDataSave(UniCart caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCartService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.CartDataInternal, caption);
        }

        public virtual ActionResult CartDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCartService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CartDesignSave(UniCart caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCartService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.CartDesignInternal, caption);
        }


        #endregion

        #region UniCartList

        public virtual ActionResult CartList(long id)
        {
            return View(id);
        }

        public virtual ActionResult CartListData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCartListService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CartListDataSave(UniCartList caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCartListService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.CartListDataInternal, caption);
        }

        public virtual ActionResult CartListDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCartListService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CartListDesignSave(UniCartList caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCartListService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.CartListDesignInternal, caption);
        }


        #endregion

        #region UniLogin

        public virtual ActionResult Login(long id)
        {
            return View(id);
        }

        public virtual ActionResult LoginData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLoginService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LoginDataSave(UniLogin caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLoginService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.LoginDataInternal, caption);
        }

        public virtual ActionResult LoginDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLoginService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LoginDesignSave(UniLogin caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLoginService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.LoginDesignInternal, caption);
        }


        #endregion

        #region UniLogin

        public virtual ActionResult Register(long id)
        {
            return View(id);
        }

        public virtual ActionResult RegisterData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniRegisterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult RegisterDataSave(UniRegister caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniRegisterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.RegisterDataInternal, caption);
        }

        public virtual ActionResult RegisterDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniRegisterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult RegisterDesignSave(UniRegister caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniRegisterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.RegisterDesignInternal, caption);
        }


        #endregion

        #region UniCheckout

        public virtual ActionResult Checkout(long id)
        {
            return View(id);
        }

        public virtual ActionResult CheckoutData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCheckoutService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CheckoutDataSave(UniCheckout caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCheckoutService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.CheckoutDataInternal, caption);
        }

        public virtual ActionResult CheckoutDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCheckoutService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CheckoutDesignSave(UniCheckout caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCheckoutService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.CheckoutDesignInternal, caption);
        }


        #endregion

        #region UniProfileName

        public virtual ActionResult ProfileName(long id)
        {
            return View(id);
        }

        public virtual ActionResult ProfileNameData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProfileNameService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProfileNameDataSave(UniProfileName caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProfileNameService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProfileNameDataInternal, caption);
        }

        public virtual ActionResult ProfileNameDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProfileNameService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProfileNameDesignSave(UniProfileName caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProfileNameService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProfileNameDesignInternal, caption);
        }


        #endregion

        #region UniCheckoutWithoutRegistration

        public virtual ActionResult CheckoutWithoutRegistration(long id)
        {
            return View(id);
        }

        public virtual ActionResult CheckoutWithoutRegistrationData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCheckoutWithoutRegistrationService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CheckoutWithoutRegistrationDataSave(UniCheckoutWithoutRegistration caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCheckoutWithoutRegistrationService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.CheckoutWithoutRegistrationDataInternal, caption);
        }

        public virtual ActionResult CheckoutWithoutRegistrationDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCheckoutWithoutRegistrationService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CheckoutWithoutRegistrationDesignSave(UniCheckoutWithoutRegistration caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCheckoutWithoutRegistrationService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.CheckoutWithoutRegistrationDesignInternal, caption);
        }


        #endregion

        #region Profile

        public virtual ActionResult Profile(long id)
        {
            return View(id);
        }

        public virtual ActionResult ProfileData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProfileService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProfileDataSave(UniProfile caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProfileService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProfileDataInternal, caption);
        }

        public virtual ActionResult ProfileDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProfileService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProfileDesignSave(UniProfile caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProfileService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProfileDesignInternal, caption);
        }


        #endregion

        #region Price

        public virtual ActionResult Price(long id)
        {
            return View(id);
        }

        public virtual ActionResult PriceData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPriceService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PriceDataSave(UniPrice caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPriceService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PriceDataInternal, caption);
        }

        public virtual ActionResult PriceDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPriceService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PriceDesignSave(UniPrice caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPriceService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PriceDesignInternal, caption);
        }


        #endregion


        #region Forgot

        public virtual ActionResult Forgot(long id)
        {
            return View(id);
        }

        public virtual ActionResult ForgotData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniForgotService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ForgotDataSave(UniForgot caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniForgotService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ForgotDataInternal, caption);
        }

        public virtual ActionResult ForgotDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniForgotService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ForgotDesignSave(UniForgot caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniForgotService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ForgotDesignInternal, caption);
        }


        #endregion


        #region Panels

        public virtual ActionResult Panels(long id)
        {
            return View(id);
        }

        public virtual ActionResult PanelsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPanelsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PanelsDataSave(UniPanels caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPanelsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PanelsDataInternal, caption);
        }

        public virtual ActionResult PanelsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPanelsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PanelsDesignSave(UniPanels caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPanelsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PanelsDesignInternal, caption);
        }


        #endregion

      

        #region Header

        public virtual ActionResult Header(long id)
        {
            return View(id);
        }

        public virtual ActionResult HeaderData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniHeaderService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult HeaderDataSave(UniHeader caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniHeaderService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.HeaderDataInternal, caption);
        }

        public virtual ActionResult HeaderDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniHeaderService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult HeaderDesignSave(UniHeader caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniHeaderService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.HeaderDesignInternal, caption);
        }


        #endregion


        #region Footer

        public virtual ActionResult Footer(long id)
        {
            return View(id);
        }

        public virtual ActionResult FooterData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniFooterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult FooterDataSave(UniFooter caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniFooterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.FooterDataInternal, caption);
        }

        public virtual ActionResult FooterDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniFooterService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult FooterDesignSave(UniFooter caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniFooterService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.FooterDesignInternal, caption);
        }


        #endregion


        #region LeftSideBar

        public virtual ActionResult LeftSideBar(long id)
        {
            return View(id);
        }

        public virtual ActionResult LeftSideBarData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLeftSideBarService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LeftSideBarDataSave(UniLeftSideBar caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLeftSideBarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.LeftSideBarDataInternal, caption);
        }

        public virtual ActionResult LeftSideBarDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLeftSideBarService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LeftSideBarDesignSave(UniLeftSideBar caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLeftSideBarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.LeftSideBarDesignInternal, caption);
        }


        #endregion

        #region RightSideBar

        public virtual ActionResult RightSideBar(long id)
        {
            return View(id);
        }

        public virtual ActionResult RightSideBarData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniRightSideBarService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult RightSideBarDataSave(UniRightSideBar caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniRightSideBarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.RightSideBarDataInternal, caption);
        }

        public virtual ActionResult RightSideBarDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniRightSideBarService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult RightSideBarDesignSave(UniRightSideBar caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniRightSideBarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.RightSideBarDesignInternal, caption);
        }


        #endregion


        #region Buttons

        public virtual ActionResult Buttons(long id)
        {
            return View(id);
        }

        public virtual ActionResult ButtonsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniButtonsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ButtonsDataSave(UniButtons caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniButtonsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ButtonsDataInternal, caption);
        }

        public virtual ActionResult ButtonsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniButtonsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ButtonsDesignSave(UniButtons caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniButtonsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ButtonsDesignInternal, caption);
        }


        #endregion

        #region Lists

        public virtual ActionResult Lists(long id)
        {
            return View(id);
        }

        public virtual ActionResult ListsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniListsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ListsDataSave(UniLists caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniListsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ListsDataInternal, caption);
        }

        public virtual ActionResult ListsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniListsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ListsDesignSave(UniLists caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniListsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ListsDesignInternal, caption);
        }


        #endregion

        #region Row

        public virtual ActionResult Row(long id)
        {
            return View(id);
        }

        public virtual ActionResult RowData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniRowService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult RowDataSave(UniRow caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniRowService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.RowDataInternal, caption);
        }

        public virtual ActionResult RowDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniRowService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult RowDesignSave(UniRow caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniRowService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.RowDesignInternal, caption);
        }


        #endregion


        #region Menu

        public virtual ActionResult Menu(long id)
        {
            return View(id);
        }

        public virtual ActionResult MenuData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniMenuService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult MenuDataSave(UniMenu caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniMenuService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.MenuDataInternal, caption);
        }

        public virtual ActionResult MenuDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniMenuService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult MenuDesignSave(UniMenu caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniMenuService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.MenuDesignInternal, caption);
        }


        #endregion


        #region Pages

        public virtual ActionResult Pages(long id)
        {
            return View(id);
        }

        public virtual ActionResult PagesData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPagesService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PagesDataSave(UniPages caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPagesService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PagesDataInternal, caption);
        }

        public virtual ActionResult PagesDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPagesService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PagesDesignSave(UniPages caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPagesService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PagesDesignInternal, caption);
        }


        #endregion

        #region Layouts

        public virtual ActionResult Layouts(long id)
        {
            return View(id);
        }

        public virtual ActionResult LayoutsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLayoutsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LayoutsDataSave(UniLayouts caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLayoutsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.LayoutsDataInternal, caption);
        }

        public virtual ActionResult LayoutsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniLayoutsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult LayoutsDesignSave(UniLayouts caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniLayoutsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.LayoutsDesignInternal, caption);
        }


        #endregion


        #region Elements

        public virtual ActionResult Elements(long id)
        {
            return View(id);
        }

        public virtual ActionResult ElementsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniElementsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ElementsDataSave(UniElements caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniElementsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ElementsDataInternal, caption);
        }

        public virtual ActionResult ElementsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniElementsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ElementsDesignSave(UniElements caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniElementsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ElementsDesignInternal, caption);
        }


        #endregion


        #region Solutions

        public virtual ActionResult Solutions(long id)
        {
            return View(id);
        }

        public virtual ActionResult SolutionsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSolutionsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SolutionsDataSave(UniSolutions caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSolutionsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.SolutionsDataInternal, caption);
        }

        public virtual ActionResult SolutionsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSolutionsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SolutionsDesignSave(UniSolutions caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSolutionsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.SolutionsDesignInternal, caption);
        }


        #endregion


        #region Actions
        
        public virtual  ActionResult UniActions(long id)
        {
            return View(MVC.CMS.WebUni.Views.Actions, id);
        }

        public virtual ActionResult ActionsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniActionsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ActionsDataSave(UniActions caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniActionsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ActionsDataInternal, caption);
        }

        public virtual ActionResult ActionsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniActionsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ActionsDesignSave(UniActions caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniActionsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ActionsDesignInternal, caption);
        }


        #endregion

        #region SitePanel

        public virtual ActionResult UniSitePanel(long id)
        {
            return View(MVC.CMS.WebUni.Views.SitePanel, id);
        }

        public virtual ActionResult SitePanelData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSitePanelService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SitePanelDataSave(UniSitePanel caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSitePanelService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.SitePanelDataInternal, caption);
        }

        public virtual ActionResult SitePanelDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniSitePanelService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SitePanelDesignSave(UniSitePanel caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniSitePanelService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.SitePanelDesignInternal, caption);
        }


        #endregion

        #region Projects

        public virtual ActionResult Projects(long id)
        {
            return View(id);
        }

        public virtual ActionResult ProjectsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProjectsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProjectsDataSave(UniProjects caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProjectsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProjectsDataInternal, caption);
        }

        public virtual ActionResult ProjectsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProjectsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProjectsDesignSave(UniProjects caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProjectsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProjectsDesignInternal, caption);
        }


        #endregion

        #region Vacancies

        public virtual ActionResult Vacancies(long id)
        {
            return View(id);
        }

        public virtual ActionResult VacanciesData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniVacanciesService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult VacanciesDataSave(UniVacancies caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniVacanciesService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.VacanciesDataInternal, caption);
        }

        public virtual ActionResult VacanciesDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniVacanciesService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult VacanciesDesignSave(UniVacancies caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniVacanciesService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.VacanciesDesignInternal, caption);
        }


        #endregion

        #region Goods

        public virtual ActionResult Goods(long id)
        {
            return View(id);
        }

        public virtual ActionResult GoodsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniGoodsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult GoodsDataSave(UniGoods caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGoodsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.GoodsDataInternal, caption);
        }

        public virtual ActionResult GoodsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniGoodsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult GoodsDesignSave(UniGoods caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGoodsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.GoodsDesignInternal, caption);
        }


        #endregion

        #region OrderHistory

        public virtual ActionResult OrderHistory(long id)
        {
            return View(id);
        }

        public virtual ActionResult OrderHistoryData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniOrderHistoryService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult OrderHistoryDataSave(UniOrderHistory caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniOrderHistoryService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.OrderHistoryDataInternal, caption);
        }

        public virtual ActionResult OrderHistoryDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniOrderHistoryService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult OrderHistoryDesignSave(UniOrderHistory caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniOrderHistoryService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.OrderHistoryDesignInternal, caption);
        }


        #endregion

        #region Gallery

        public virtual ActionResult Gallery(long id)
        {
            return View(id);
        }

        public virtual ActionResult GalleryData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniGalleryService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult GalleryDataSave(UniGallery caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGalleryService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.GalleryDataInternal, caption);
        }

        public virtual ActionResult GalleryDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniGalleryService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult GalleryDesignSave(UniGallery caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniGalleryService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.GalleryDesignInternal, caption);
        }


        #endregion

        #region Photo

        public virtual ActionResult Photo(long id)
        {
            return View(id);
        }

        public virtual ActionResult PhotoData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPhotoService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PhotoDataSave(UniPhoto caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPhotoService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PhotoDataInternal, caption);
        }

        public virtual ActionResult PhotoDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPhotoService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PhotoDesignSave(UniPhoto caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPhotoService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PhotoDesignInternal, caption);
        }


        #endregion

        #region Balance

        public virtual ActionResult Balance(long id)
        {
            return View(id);
        }

        public virtual ActionResult BalanceData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniBalanceService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult BalanceDataSave(UniBalance caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniBalanceService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.BalanceDataInternal, caption);
        }

        public virtual ActionResult BalanceDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniBalanceService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult BalanceDesignSave(UniBalance caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniBalanceService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.BalanceDesignInternal, caption);
        }


        #endregion

        #region BalanceList

        public virtual ActionResult BalanceList(long id)
        {
            return View(id);
        }

        public virtual ActionResult BalanceListData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniBalanceListService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult BalanceListDataSave(UniBalanceList caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniBalanceListService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.BalanceListDataInternal, caption);
        }

        public virtual ActionResult BalanceListDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniBalanceListService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult BalanceListDesignSave(UniBalanceList caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniBalanceListService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.BalanceListDesignInternal, caption);
        }


        #endregion

        #region BalanceCharge

        public virtual ActionResult BalanceCharge(long id)
        {
            return View(id);
        }

        public virtual ActionResult BalanceChargeData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniBalanceChargeService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult BalanceChargeDataSave(UniBalanceCharge caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniBalanceChargeService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.BalanceChargeDataInternal, caption);
        }

        public virtual ActionResult BalanceChargeDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniBalanceChargeService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult BalanceChargeDesignSave(UniBalanceCharge caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniBalanceChargeService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.BalanceChargeDesignInternal, caption);
        }


        #endregion

        #region DropDownList

        public virtual ActionResult DropDownList(long id)
        {
            return View(id);
        }

        public virtual ActionResult DropDownListData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniDropDownListService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult DropDownListDataSave(UniDropDownList caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniDropDownListService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.DropDownListDataInternal, caption);
        }

        public virtual ActionResult DropDownListDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniDropDownListService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult DropDownListDesignSave(UniDropDownList caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniDropDownListService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.DropDownListDesignInternal, caption);
        }


        #endregion


        #region Postcards

        public virtual ActionResult Postcards(long id)
        {
            return View(id);
        }

        public virtual ActionResult PostcardsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPostcardsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PostcardsDataSave(UniPostcards caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPostcardsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PostcardsDataInternal, caption);
        }

        public virtual ActionResult PostcardsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPostcardsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PostcardsDesignSave(UniPostcards caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPostcardsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PostcardsDesignInternal, caption);
        }


        #endregion

        #region News

        public virtual ActionResult News(long id)
        {
            return View(id);
        }

        public virtual ActionResult NewsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniNewsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult NewsDataSave(UniNews caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniNewsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.NewsDataInternal, caption);
        }

        public virtual ActionResult NewsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniNewsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult NewsDesignSave(UniNews caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniNewsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.NewsDesignInternal, caption);
        }


        #endregion



        #region Tasks

        public virtual ActionResult Tasks(long id)
        {
            return View(id);
        }

        public virtual ActionResult TasksData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniTasksService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult TasksDataSave(UniTasks caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniTasksService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.TasksDataInternal, caption);
        }

        public virtual ActionResult TasksDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniTasksService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult TasksDesignSave(UniTasks caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniTasksService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.TasksDesignInternal, caption);
        }


        #endregion







        #region Docs

        public virtual ActionResult Docs(long id)
        {
            return View(id);
        }

        public virtual ActionResult DocsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniDocsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult DocsDataSave(UniDocs caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniDocsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.DocsDataInternal, caption);
        }

        public virtual ActionResult DocsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniDocsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult DocsDesignSave(UniDocs caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniDocsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.DocsDesignInternal, caption);
        }


        #endregion





        #region Ideas

        public virtual ActionResult Ideas(long id)
        {
            return View(id);
        }

        public virtual ActionResult IdeasData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniIdeasService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult IdeasDataSave(UniIdeas caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniIdeasService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.IdeasDataInternal, caption);
        }

        public virtual ActionResult IdeasDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniIdeasService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult IdeasDesignSave(UniIdeas caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniIdeasService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.IdeasDesignInternal, caption);
        }


        #endregion




        #region Plans

        public virtual ActionResult Plans(long id)
        {
            return View(id);
        }

        public virtual ActionResult PlansData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPlansService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PlansDataSave(UniPlans caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPlansService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PlansDataInternal, caption);
        }

        public virtual ActionResult PlansDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPlansService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PlansDesignSave(UniPlans caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPlansService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PlansDesignInternal, caption);
        }


        #endregion



        #region AppLists

        public virtual ActionResult AppLists(long id)
        {
            return View(id);
        }

        public virtual ActionResult AppListsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniAppListsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult AppListsDataSave(UniAppLists caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniAppListsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.AppListsDataInternal, caption);
        }

        public virtual ActionResult AppListsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniAppListsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult AppListsDesignSave(UniAppLists caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniAppListsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.AppListsDesignInternal, caption);
        }


        #endregion



        #region Places

        public virtual ActionResult Places(long id)
        {
            return View(id);
        }

        public virtual ActionResult PlacesData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPlacesService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PlacesDataSave(UniPlaces caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPlacesService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PlacesDataInternal, caption);
        }

        public virtual ActionResult PlacesDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPlacesService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PlacesDesignSave(UniPlaces caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPlacesService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PlacesDesignInternal, caption);
        }


        #endregion



        #region Calendar

        public virtual ActionResult Calendar(long id)
        {
            return View(id);
        }

        public virtual ActionResult CalendarData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCalendarService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CalendarDataSave(UniCalendar caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCalendarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.CalendarDataInternal, caption);
        }

        public virtual ActionResult CalendarDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniCalendarService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult CalendarDesignSave(UniCalendar caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniCalendarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.CalendarDesignInternal, caption);
        }


        #endregion




        #region Events

        public virtual ActionResult Events(long id)
        {
            return View(id);
        }

        public virtual ActionResult EventsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniEventsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult EventsDataSave(UniEvents caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniEventsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.EventsDataInternal, caption);
        }

        public virtual ActionResult EventsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniEventsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult EventsDesignSave(UniEvents caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniEventsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.EventsDesignInternal, caption);
        }


        #endregion





        #region Reserve

        public virtual ActionResult Reserve(long id)
        {
            return View(id);
        }

        public virtual ActionResult ReserveData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniReserveService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ReserveDataSave(UniReserve caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniReserveService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ReserveDataInternal, caption);
        }

        public virtual ActionResult ReserveDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniReserveService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ReserveDesignSave(UniReserve caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniReserveService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ReserveDesignInternal, caption);
        }


        #endregion




        #region Print

        public virtual ActionResult Print(long id)
        {
            return View(id);
        }

        public virtual ActionResult PrintData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPrintService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PrintDataSave(UniPrint caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPrintService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.PrintDataInternal, caption);
        }

        public virtual ActionResult PrintDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniPrintService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult PrintDesignSave(UniPrint caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniPrintService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.PrintDesignInternal, caption);
        }


        #endregion






        #region Research

        public virtual ActionResult Research(long id)
        {
            return View(id);
        }

        public virtual ActionResult ResearchData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniResearchService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ResearchDataSave(UniResearch caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniResearchService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ResearchDataInternal, caption);
        }

        public virtual ActionResult ResearchDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniResearchService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ResearchDesignSave(UniResearch caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniResearchService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ResearchDesignInternal, caption);
        }


        #endregion




        #region Ads

        public virtual ActionResult Ads(long id)
        {
            return View(id);
        }

        public virtual ActionResult AdsData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniAdsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult AdsDataSave(UniAds caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniAdsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.AdsDataInternal, caption);
        }

        public virtual ActionResult AdsDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniAdsService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult AdsDesignSave(UniAds caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniAdsService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.AdsDesignInternal, caption);
        }


        #endregion


        #region Map

        public virtual ActionResult Map(long id)
        {
            return View(id);
        }

        public virtual ActionResult MapData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniMapService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult MapDataSave(UniMap caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniMapService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.MapDataInternal, caption);
        }

        public virtual ActionResult MapDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniMapService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult MapDesignSave(UniMap caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniMapService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.MapDesignInternal, caption);
        }


        #endregion


        #region ListSelector

        public virtual ActionResult ListSelector(long id)
        {
            return View(id);
        }

        public virtual ActionResult ListSelectorData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniListSelectorService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ListSelectorDataSave(UniListSelector caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniListSelectorService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ListSelectorDataInternal, caption);
        }

        public virtual ActionResult ListSelectorDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniListSelectorService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ListSelectorDesignSave(UniListSelector caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniListSelectorService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ListSelectorDesignInternal, caption);
        }


        #endregion



        #region Input

        public virtual ActionResult Input(long id)
        {
            return View(id);
        }

        public virtual ActionResult InputData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniInputService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult InputDataSave(UniInput caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniInputService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.InputDataInternal, caption);
        }

        public virtual ActionResult InputDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniInputService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult InputDesignSave(UniInput caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniInputService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.InputDesignInternal, caption);
        }


        #endregion


        #region Project

        public virtual ActionResult Project(long id)
        {
            return View(id);
        }

        public virtual ActionResult ProjectData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProjectService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProjectDataSave(UniProject caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProjectService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProjectDataInternal, caption);
        }

        public virtual ActionResult ProjectDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniProjectService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ProjectDesignSave(UniProject caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniProjectService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ProjectDesignInternal, caption);
        }


        #endregion

        #region Application

        public virtual ActionResult Application(long id)
        {
            return View(id);
        }

        public virtual ActionResult ApplicationData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniApplicationService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ApplicationDataSave(UniApplication caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniApplicationService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.ApplicationDataInternal, caption);
        }

        public virtual ActionResult ApplicationDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<IUniApplicationService>();
            return View(service.GetByCatalogItemID(id));
        }
        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult ApplicationDesignSave(UniApplication caption)
        {
            var service = ServiceLocator.Current.GetInstance<IUniApplicationService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.ApplicationDesignInternal, caption);
        }


        #endregion

        #region Webinar

        public virtual ActionResult Webinar(long id)
        {
            return View(id);
        }

        public virtual ActionResult WebinarData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Webinar.IUniWebinarService>();
            return View(service.GetByCatalogItemID(id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult WebinarDataSave(UniWebinar caption)
        {
            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Webinar.IUniWebinarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.WebinarDataInternal, caption);
        }

        public virtual ActionResult WebinarDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Webinar.IUniWebinarService>();
            return View(service.GetByCatalogItemID(id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult WebinarDesignSave(UniWebinar caption)
        {
            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Webinar.IUniWebinarService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.WebinarDesignInternal, caption);
        }

        #endregion

        #region Storage

        public virtual ActionResult Storage(long id)
        {
            return View(id);
        }

        public virtual ActionResult StorageData(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Storage.IUniStorageService>();
            return View(service.GetByCatalogItemID(id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult StorageDataSave(UniStorage caption)
        {
            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Storage.IUniStorageService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateData(caption);
            });

            return View(MVC.CMS.WebUni.Views.StorageDataInternal, caption);
        }

        public virtual ActionResult StorageDesign(long id)
        {
            ModelState.Clear();

            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Storage.IUniStorageService>();
            return View(service.GetByCatalogItemID(id));
        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult StorageDesignSave(UniStorage caption)
        {
            var service = ServiceLocator.Current.GetInstance<WebUni.Kernel.BWP.Storage.IUniStorageService>();

            UpdateWrapper(() =>
            {
                caption = service.UpdateDesign(caption);
            });

            return View(MVC.CMS.WebUni.Views.StorageDesignInternal, caption);
        }
        #endregion
    }
}