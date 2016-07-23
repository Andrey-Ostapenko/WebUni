using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using DevExpress.Data.Linq;
using livemenu.Areas.BWP.Models.Catalog;
using livemenu.Common.Entities;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Settings;
using livemenu.Kernel.BWP;
using livemenu.Kernel.BWP.Pages;
using livemenu.Kernel.Catalog;
using livemenu.Kernel.CMS.Export;
using livemenu.Kernel.CRM.PriceList;
using livemenu.Kernel.MicroModules;
using livemenu.Kernel.Transliteration;
using System.Text;
using System.Web.Script.Serialization;
using DevExpress.Office.Utils;
using livemenu.Areas.CMS.Models;
using livemenu.Areas.CMS.Models.Catalog;
using livemenu.Areas.CMS.Models.CatalogItemSelector;
using livemenu.Common.Kernel.CatalogItemCache;
using livemenu.Kernel.Catalog.Models;
using livemenu.Kernel.CMS;
using livemenu.Kernel.Configuration;
using livemenu.Common.Attributes;
using livemenu.Kernel.Management;
using livemenu.Common.Kernel.Catalog.FamilyTree;
using CatalogItemFlat = livemenu.Kernel.Catalog.CatalogItemFlat;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class CatalogController : CMSController
    {
        private readonly ILicenseInfoService _licenseInfoService;
        private readonly IPriceListElementService _priceListElementService;
        private readonly ICatalogItemService _catalogItemService;
        private readonly ICatalogItemLinkService _catalogItemLinkService;
        private readonly ICatalogItemFamilyTreeService _catalogItemFamilyTreeService;
        private readonly ICatalogItemSearchPropertyService _catalogItemSearchPropertyService;
        private readonly IMicroModuleFactory _microModuleFactory;
        private readonly ISiteGroupPriceListDataService _siteGroupPriceListDataService;
        private readonly IPageService _pageService;
        private readonly ICatalogItemTemplateService _catalogItemTemplateService;
        private readonly ICatalogItemExporter _catalogItemExporter;
        private readonly ICatalogItemCacheService _catalogItemCacheService;

        //private readonly ICatalogAssignModel _catalogAssignModel;

        //private readonly IMainConfigService _mainConfigService;

        public CatalogController(
            ILicenseInfoService licenseInfoService,
            IPriceListElementService priceListElementService,
            ICatalogItemService catalogItemService,
            ICatalogItemLinkService catalogItemLinkService,
            ICatalogItemFamilyTreeService catalogItemFamilyTreeService,
            ICatalogItemSearchPropertyService catalogItemSearchPropertyService,
            IMicroModuleFactory microModuleFactory,
            ISiteGroupPriceListDataService siteGroupPriceListDataService,
            IPageService pageService,
            ICatalogItemTemplateService catalogItemTemplateService,
            ICatalogItemExporter catalogItemExporter,
            ICatalogItemCacheService catalogItemCacheService
            //IMainConfigService mainConfigService

            )
        {
            _licenseInfoService = licenseInfoService;
            _priceListElementService = priceListElementService;
            _catalogItemService = catalogItemService;
            _catalogItemLinkService = catalogItemLinkService;
            _catalogItemFamilyTreeService = catalogItemFamilyTreeService;
            _catalogItemSearchPropertyService = catalogItemSearchPropertyService;
            _microModuleFactory = microModuleFactory;
            _siteGroupPriceListDataService = siteGroupPriceListDataService;
            _pageService = pageService;
            _catalogItemTemplateService = catalogItemTemplateService;
            _catalogItemExporter = catalogItemExporter;
            _catalogItemCacheService = catalogItemCacheService;
            //_mainConfigService = mainConfigService;
        }

        public virtual ActionResult Index(string code)
        {
            var catalogItem = _catalogItemService.Get(code);

            if (catalogItem == null)
            {
                return new HttpStatusCodeResult(500);
            }

            return View(new CatalogMainModel
            {
                CanHaveChildren = true,
                RootID = catalogItem.RootID,
                CatalogItemCode = catalogItem.Code,
                CatalogItemID = catalogItem.ID
            });
        }

        public virtual ActionResult Export(long id)
        {
            var allowExportImport = _licenseInfoService.GetLicense().AllowExportImport;
            if (!allowExportImport)
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "В тестовой версии импорт/экспорт не возможен. Действие отменено"
                }, NotificationType.Warning);

                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Length", "1");
                Response.SuppressContent = true;
                Response.AddHeader("Content-Disposition", "filename=prevent");
                return new EmptyResult();
            }
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Экспорт запущен"
                });
                var ci = _catalogItemService.Get(id); //todo: убрать отсюда нахер, грузим только для получения имени

                var downloadFileName = string.Format("{0}_{1}_{2}.WebUni", ci.Code,
                    ci.Name.Replace(" ", "_").Replace(",", ""), DateTime.Now.ToString("dd-MM-yyyy-HH_mm_ss"));
                Response.ContentType = "application/zip";
                Response.AddHeader("Content-Disposition", "filename=" + downloadFileName);
                var root = Server.MapPath("~/");
                _catalogItemExporter.ExportToStream(Response.OutputStream, root, id);

                return true;
            }, "Экспорт успешно завершен");
            return new EmptyResult();
        }

        public virtual ActionResult Import(long id, string file)
        {
            var allowExportImport = _licenseInfoService.GetLicense().AllowExportImport;
            if (!allowExportImport)
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "В тестовой версии импорт/экспорт не возможен. Действие отменено"
                }, NotificationType.Warning);


                return new EmptyResult();
            }

            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Импорт запущен"
                });

                //var path = Server.MapPath("/Admin/" + file);
                //var prefix = Request.Url.IsLoopback ? "" : "/Admin/";
                var path = Path.Combine(Server.MapPath("~"), file);

                _catalogItemExporter.ImportFromFile(path, Server.MapPath("~"), id);
                return true;
            }, "Импорт успешно завершен");
            return new EmptyResult();
        }

        public virtual ActionResult TemplateSelector(CatalogItemTypeValue type, long id)
        {
            return View(new CatalogItemTemplateModel
            {
                ID = id,
                Type = type,
                Items = _catalogItemTemplateService.GetByType(type).Select(x => new CatalogItemTemplateItemModel
                {
                    ID = x.ID,
                    Name = string.Format("({0}) {1}", _catalogItemService.GetCatalogItemElementName(x), x.Name),
                    IsSelected = false
                }).ToList()

            });
        }

        public virtual ActionResult GroupStructure(long id, CatalogItemViewType type)
        {
            var model = _catalogItemService.GetGroupStructure(id, type);
            return View(model);
        }

        public virtual ActionResult Structure(long id, CatalogItemViewType viewtype, CatalogItemTypeValue? type = null)
        {
            var model = _catalogItemService.GetStructure(id, viewtype, type);
            return View(model);
        }

        [HttpPost]
        public virtual JsonResult ChangeStructure(ChangeStructureModel model)
        {
            OperationWrapper(() =>
            {
                _catalogItemService.ChangeStructure(model);

                return true;
            }, "Изменения успешно сохранены");
            return new JsonResult();
        }

        [HttpPost]
        public virtual JsonResult ChangeCollapsedStateCatalogItem(ChangeCollapsedStateCatalogItemModel model)
        {
            //OperationWrapper(() =>
            //{
            _catalogItemService.ChangeCollapsedStateCatalogItem(model);
            //  return true;
            //}, "Изменения успешно сохранены");
            return new JsonResult();
        }


        [HttpPost]
        public virtual JsonResult ChangeGroupStructure(ChangeGroupStructureModel model)
        {
            OperationWrapper(() =>
            {
                _catalogItemService.ChangeGroupStructure(model);

                return true;
            }, "Изменения успешно сохранены");
            return new JsonResult();
        }



        [HttpPost]
        public virtual JsonResult MakeTemplate(CatalogItemTypeValue type, long id)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Создание шаблона запущено"
                });

                var toID = CatalogItemTypeCodeHelpers.CatalogItemTypeCodeMap[type].ToLong();
                _catalogItemService.CopyCatalogItems(toID, new[] {id},
                    new CopyingContext(CopyingContextType.MakeTemplate)
                    {
                        IsDeep = false,
                        CopyMakeActive = CatalogItemAssignVisibilityType.MakeVisible,
                        CopyCount = 1
                    });

                return true;
            }, "Создание шаблона успешно завершено");
            return new JsonResult();
        }

        public virtual ActionResult CreatePhotoFromFolder(long parentID, string folder)
        {
            OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage()
                {
                    Text = "Создание фото запущено"
                });


                var abs = Server.MapPath("~/" + folder);

                _catalogItemService.CreatePhotoFromFolder(parentID, abs, folder);

                return true;
            }, "Создание фото завершено");
            return new EmptyResult();
        }

        [HttpPost]
        public virtual ActionResult InsertTemplates(CatalogItemTemplateModel model)
        {
           OperationWrapper(() =>
           {
               _notificationBus.Notificate(new NotificationMessage()
               {
                   Text = "Копирование запущено"
               }, isPermanent: true);

               _catalogItemService.CopyCatalogItems(model.ID, model.Items.Where(x=>x.IsSelected).Select(x=>x.ID).ToArray());
               return true;
           }, "Копирование успешно завершено", isPermanent: true);
            return new EmptyResult();
        }


        public virtual ActionResult CatalogTreeWrapper(string code)
        {
            var catalogItem = _catalogItemService.Get(code);
            return View(MVC.CMS.Catalog.Views.CatalogTree, new CatalogMainModel
            {
                CanHaveChildren = true,
                RootID = catalogItem.RootID,
                CatalogItemCode = catalogItem.Code,
                CatalogItemID = catalogItem.ID
            });
        }

        public virtual ActionResult CatalogItemPriceListElement(long? id)
        {
            return View(id.HasValue ? _priceListElementService.Get(id.Value) : null);
        }


        public virtual ActionResult CatalogWorkspace(string key)
        {
            if (NewCatalogItemModel.IsKeyLink(key))
            {
                return View(new CatalogItemWorkspaceModel //hardcode - link
                {
                    CanHaveChildren = false,
                    CatalogItem = null,
                    CatalogItemChildren = null
                });
            }

            var id = NewCatalogItemModel.GetIDFromKey(key);

            var ci = _catalogItemService.GetCatalogItem(id);
            var model = new CatalogItemWorkspaceModel
            {
                CatalogItem = new CatalogItemGridModel(ci),
                CatalogItemChildren =
                    _catalogItemService.GetCatalogItemsChildrenFast(id).Select(c => new CatalogItemGridModel(c)),

                //    CanHaveChildren = _catalogItemService.CanHaveChildren(ci)
            };
            return View(model);
        }

        public virtual ActionResult CatalogItemCardCreation(string type, long id, long rootID)
        {
            ModelState.Clear();
            ViewData["CatalogCardLoadingState"] = CatalogCardLoadingState.Loaded;
            var model = new CatalogItemCardModel
            {
                ParentID = id,
                TypeCode = type,
                RootID = rootID
            };


            return View(MVC.CMS.Catalog.Views.CatalogItemCard, model);
        }

        public virtual ActionResult CatalogItemCardEditing(string key)
        {
            ModelState.Clear();

            ViewData["CatalogCardLoadingState"] = CatalogCardLoadingState.Loaded;

            CatalogItemCardModel model = null;

            if (NewCatalogItemModel.IsKeyLink(key))
            {
                model = _catalogItemService.GetCatalogItemLinkCardModel(key);
            }
            else
            {
                var id = NewCatalogItemModel.GetIDFromKey(key);
                model = _catalogItemService.GetCatalogItemCardModel(id);
            }


            return View(MVC.CMS.Catalog.Views.CatalogItemCard, model);
        }



        public virtual JsonResult Transliterate(string text)
        {
            var res = _catalogItemService.FindUniqueTransliteratedCode(text);
            return Json(res, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult CatalogTree(CatalogMainModel model)
        {
            return View(model);
        }

        [SingleExecution(lockKeyPrefix = "GetFamilyTree", lockKeyParam = "id")]
        public virtual JsonResult GetWebPageLink(long id)
        {
            return Json(_catalogItemService.GetWebPageLink(id), JsonRequestBehavior.AllowGet);

        }

        [SingleExecution(lockKeyPrefix = "GetFamilyTree", lockKeyParam = "id")]
        public virtual JsonResult GetFamilyTree(long id)
        {
            var ft = _catalogItemFamilyTreeService.GetFamilyTreeByCatalogItemID(id);
            var items = ft == null ? new List<CatalogItem>() : ft.FamilyTree;
            return
                Json(
                    items.Select(
                        x =>
                            new
                            {
                                Name = x.Name,
                                Code = x.Code,
                                ID = x.ID,
                                Key = NewCatalogItemModel.MakeKey(x.ParentID, x.ID)
                            }), JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public virtual JsonResult LinkCatalogItem(long parentCIID, long linkCIID)
        {
            return Json(OperationWrapper(() =>
            {
                _catalogItemService.Link(parentCIID, linkCIID);
                return true;
            }, "Элемент успешно привязан"));

        }

        [HttpPost]
        public virtual ActionResult SaveCatalogItem(CatalogItemCardModel catalogItemCardModel)
        {
            CatalogItemCardModel res = catalogItemCardModel;
            if (ModelState.IsValid)
            {
                OperationWrapper(() =>
                {

                    var state = catalogItemCardModel.IsNew()
                        ? CatalogCardLoadingState.Created
                        : CatalogCardLoadingState.Saved;

                    ViewData["CatalogCardLoadingState"] = state;

                    if (catalogItemCardModel.IsLink)
                    {
                        _catalogItemLinkService.Save(catalogItemCardModel.ID, catalogItemCardModel.IsActive);
                        var saved = _catalogItemLinkService.GetByID(catalogItemCardModel.ID);

                        // update catalogItem in Tree on client
                        var newCatalogTreeItemData = _catalogItemService.GetCatalogItemTreeUpdate(saved);
                        _notificationBus.notificateAboutSavedCatalogItem(newCatalogTreeItemData,
                            NotificationTargetType.Current);
                    }
                    else
                    {
                        var saved = _catalogItemService.Update(new NewCatalogItemModel
                        {
                            Name = catalogItemCardModel.Name,
                            Code = catalogItemCardModel.Code,
                            CustomCode = catalogItemCardModel.CustomCode,
                            ChangingUrl = catalogItemCardModel.ChangingUrl,
                            ID = catalogItemCardModel.ID,
                            IsActive = catalogItemCardModel.IsActive,
                            ParentID =
                                catalogItemCardModel.ParentID == -1 ? (long?) null : catalogItemCardModel.ParentID,
                            RootID = catalogItemCardModel.RootID == -1 ? (long?) null : catalogItemCardModel.RootID,
                            TypeCode = catalogItemCardModel.TypeCode,
                            ElementName = catalogItemCardModel.ElementName,
                            IsGrouped = catalogItemCardModel.IsGrouped,
                            G = catalogItemCardModel.IsGeneriсDesign,
                        }, false, true);



                        ModelState.Clear();
                        var modifiedDateText = string.Format("сохранено в ") + DateTime.Now.ToString("HH:mm");

                        res = new CatalogItemCardModel
                        {
                            CatalogItemType = (CatalogItemTypeValue?) saved.CatalogItemTypeID,
                            ModifiedDateText = modifiedDateText,
                            Name = saved.Name,
                            Code = saved.Code,
                            CustomCode = saved.CustomCode,
                            ChangingUrl = saved.ChangingUrl,
                            ID = saved.ID,
                            IsActive = saved.IsVisible,
                            IsLink = saved.IsLink,
                            ParentID = saved.ParentID.HasValue ? saved.ParentID.Value : -1,
                            RootID = saved.RootID.HasValue ? saved.RootID.Value : -1,
                            TypeCode = catalogItemCardModel.TypeCode,
                            ShowWebPageLink = catalogItemCardModel.ShowWebPageLink,
                            ElementName = saved.ElementName,

                            //IsGeneriсDesign = saved.IsGeneriсDesign,
                            IsGeneriсDesign = saved.IsGeneriсDesign,
                            IsGrouped = saved.IsGrouped,

                            CardName =$"({_catalogItemService.GetCatalogItemElementName(saved)}) {saved.Name}"
                        };

                        // update catalogItem in Tree on client
                        if (saved.CatalogItemTypeValue != CatalogItemTypeValue.Line &&
                            saved.CatalogItemTypeValue != CatalogItemTypeValue.Page)
                        {
                            var newCatalogTreeItemData = _catalogItemService.GetCatalogItemTreeUpdate(saved);
                            _notificationBus.notificateAboutSavedCatalogItem(newCatalogTreeItemData,
                                NotificationTargetType.Current);
                        }
                        else if (saved.IsGrouped && !catalogItemCardModel.firstItemDataIsLoaded)
                        {
                            var newCatalogTreeItemData = _catalogItemService.GetCatalogItemTreeUpdate(saved,
                                saved.UniLine.Single());
                            _notificationBus.notificateAboutSavedCatalogItem(newCatalogTreeItemData,
                                NotificationTargetType.Current);
                        }
                    }

                    return true;
                });
            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage
                {
                    Text = "Изменения не были сохранены"
                }, NotificationType.Error);
            }

            return View(MVC.CMS.Catalog.Views.CatalogItemCardInternal, res);
        }

        [HttpPost]
        public virtual ActionResult Reorder(string sourceKey, string targetKey, string type)
        {
            bool result = false;
            OperationWrapper(() =>
            {
                var assignType = (CatalogItemAssignType) Enum.Parse(typeof (CatalogItemAssignType), type, true);
                result = _catalogItemService.Reorder(sourceKey, targetKey, assignType);
                return result;
            }, "Порядок успешно изменен");

            return Json(result);
        }

        [HttpPost]
        public virtual JsonResult ChangeActivity(string[] ids, bool value, bool isRec)
        {
            if (ids == null) return Json("");
            return Json(OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage
                {
                    Text = "Изменение видимости запущено"
                });
                var lids = ids.Select(NewCatalogItemModel.GetIDFromKey).ToArray();
                _catalogItemService.ChangeActivity(lids, value, isRec);


                return true;
            }, "Изменение видимости успешно завершено"));
        }


        [HttpPost]
        public virtual JsonResult CascadeDeleteCatalogItems(string[] ids)
        {
            if (ids == null) return Json("");
            return Json(OperationWrapper(() =>
            {
                _notificationBus.Notificate(new NotificationMessage
                {
                    Text = "Удаление запущено"
                });
                foreach (var id in ids)
                {

                    if (NewCatalogItemModel.IsKeyLink(id))
                    {
                        _catalogItemLinkService.Unlink(NewCatalogItemModel.GetParentIDFromKey(id).Value,
                            NewCatalogItemModel.GetIDFromKey(id));
                    }
                    else
                    {
                        _catalogItemService.Delete(NewCatalogItemModel.GetIDFromKey(id));
                    }
                }

                return true;
            }, "Удаление успешно завершено"));
        }

        [HttpPost]
        public virtual ActionResult Assign(OptionsRightModalPage assignModel)
        {
            return Json(OperationWrapper(
                () => _catalogItemService.Assign(assignModel), 
                assignModel.ModeEnum == CatalogItemAssignMode.Copy 
               ? 
                "Копирование успешно завершено" 
                : "Перемещение успешно завершено", isPermanent: assignModel.ModeEnum == CatalogItemAssignMode.Copy)); 
        }


        //[HttpPost]
        //public virtual ActionResult Assign(OptionsRightModalPage assignModel)
        //{
        //    return Json(OperationWrapper(() =>
        //    {
        //        return _catalogItemService.Assign(assignModel.SourceKey,
        //                               assignModel.TargetKey,
        //                               assignModel.AssignMode,
        //                               assignModel.AssignType,
        //                               assignModel.CopyCount,
        //                               assignModel.CopyMakeActive,
        //                               assignModel.IsCopyDeep);

           // }, 

        //[HttpPost]
        //public virtual ActionResult Assign(string sourceKey, string targetKey, string type, string mode, int copyCount, CatalogItemAssignVisibilityType? copyMakeActive, bool copyDeep)
        //{
        //    var assignMode = (CatalogItemAssignMode)Enum.Parse(typeof(CatalogItemAssignMode), mode, true);

        //    var assignType = (CatalogItemAssignType)Enum.Parse(typeof(CatalogItemAssignType), type, true);

        //    return Json(OperationWrapper(() =>
        //    {
        //        return _catalogItemService.Assign(sourceKey, targetKey, assignMode, assignType, copyCount, copyMakeActive, copyDeep);

        //    }, assignMode == CatalogItemAssignMode.Copy ? "Копирование успешно завершено" : "Перемещение успешно завершено"));

        //}


        public virtual JsonResult GetCatalogTreeItemModels(long rootID, long? id = null)
        {
            var parentID = id ?? rootID;
            var items = _catalogItemService.GetCatalogItemsChildrenMegaFast(parentID).Select(c => {
                var canHaveChildren = !c.IsLink && ((c.CatalogItemTypeID.HasValue && UniBlockServiceHelper.GetChildren((CatalogItemTypeValue)c.CatalogItemTypeID.Value).Any()) || !c.CatalogItemTypeID.HasValue);
                return new CatalogTreeItemModel
                {
                    CatalogItemTypeID = c.CatalogItemTypeID,
                    Priority = c.Priority,
                    key = c.Key,
                    Code = c.Code,
                    title = c.Name,
                    ItemName = c.Name, //дубликат, необходимо, так как плагин название в чистом виде не сохраняет
                                       //folder = c.HasChildren,
                    isActive = c.IsVisible,
                    isLink = c.IsLink,
                    lazy = c.Lazy,

                    ElementName = c.ElementName,

                    typeCode = c.TypeCode,
                    typeName = "B",
                    RootID = c.RootID,
                    ParentID = c.ParentID,
                    ID = c.ID,

                    TemplateType = c.TemplateType,
                    FiltersEnabled = c.FiltersEnabled,
                    HasFilters = c.HasFilters,

                    LinksEnabled = c.LinksEnabled,
                    HasLinks = c.HasLinks,

                    DynamicEntityEnabled = c.DynamicEntityEnabled,
                    HasDynamicEntity = c.HasDynamicEntity,

                    IsChain = c.IsChain,




                    HasShopModelPriceListElement = c.HasShopModelPriceListElement,
                    IsShopModeEnabled = c.IsShopModeEnabled,
                    IsShopModeInherits = c.IsShopModeInherits,
                    IsBuyButtonModeEnabled = c.IsBuyButtonModeEnabled,
                    IsGrouped = c.IsGrouped,
                    //IsGenericDesign = c.IsGeneriсDesign,
                    CanHaveChildren = canHaveChildren,

                    AllowCreate = canHaveChildren,
                    AllowCopy = c.RootID.HasValue && !c.IsLink,
                    AllowLink = canHaveChildren && c.RootID.HasValue,
                    AllowDelete = c.RootID.HasValue,
                    AllowMove = c.RootID.HasValue,

                    ActionType = c.ActionType
                };
            });


            return Json(items, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult GetCatalogTreeItemRoot(long rootID)
        {
            List<CatalogTreeItemModel> result = new List<CatalogTreeItemModel>();
            var c = _catalogItemService.GetCatalogItem(rootID);
            var item = new CatalogTreeItemModel
            {
                Priority = c.Priority,
                key = c.Key,
                Code = c.Code,
                title = c.Name,
                ItemName = c.Name,
                //folder = c.HasChildren,
                isActive = c.IsActive,
                isLink = c.IsLink,
                lazy = c.HasChildren,
                typeName = c.TypeName,
                RootID = c.RootID,
                ParentID = c.ParentID,
                ID = c.ID,

                ElementName = c.ElementName,
                
                CanHaveChildren = true,

                AllowCopy = false,
                AllowDelete = false,
                AllowCreate = true,
                AllowMove = false,
            };
            result.Add(item);

            if (rootID == CatalogItemCodes.Site.ToLong())
            {
                foreach (var ci in new []
                {
                   CatalogItemCodes.Goods.ToLong(),

                   12,
                   CatalogItemCodes.Actions.ToLong(),
                   CatalogItemCodes.Vacancies.ToLong(),
                   14,

                   CatalogItemCodes.Projects.ToLong(),
                   CatalogItemCodes.Templates.ToLong(),
                   CatalogItemCodes.Elements.ToLong(),

                   7,
                   16,
                   17,
                   13,
                   18,

                   20,
                   21,
                   19,
                   22,

                   15,
                   23,
                   24,
                   25

                })
                {
                    var c1 = _catalogItemService.GetCatalogItem(ci);
                    var item1 = new CatalogTreeItemModel
                    {
                        Priority = c1.Priority,
                        key = c1.Key,
                        Code = c1.Code,
                        title = c1.Name,
                        ItemName = c1.Name,
                        //folder = c1.HasChildren,
                        isActive = c1.IsActive,
                        isLink = c1.IsLink,
                        lazy = c1.HasChildren,
                        typeName = c1.TypeName,
                        RootID = c1.RootID,
                        ParentID = c1.ParentID,
                        ID = c1.ID,

                        ElementName = c1.ElementName,

                        CanHaveChildren = true,

                        AllowCopy = false,
                        AllowDelete = false,
                        AllowCreate = true,
                        AllowMove = false,
                    };
                    result.Add(item1);
                }
                
            }
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        public virtual ActionResult CatalogItemDetail(long id)
        {
            var mms = _microModuleFactory.GetAllByCatalogItemID(id);
            var model = new CatalogItemDetailModel
            {
                ID = id,
                MMs = mms.Select(mm => new CatalogItemMMModel
                {
                    CatalogItemID = id,
                    ID = mm.MM.ID,
                    Code = mm.MM.Code,
                    Name = mm.Title
                })
            };

            return View(model);
        }

        [HttpPost]
        public virtual JsonResult FastCreate(long parentID, string type, long? catalogItemTypeID)
        {
            //hardcode
            if (type == "UniversalPage")
            {
                catalogItemTypeID = 1;
            }

            CatalogItem result = new CatalogItem();
            return Json(OperationWrapper(() =>
            {
                result = _catalogItemService.FastCreate(parentID, type, catalogItemTypeID);
                return true;

            }, string.Format("Элемент {0} успешно создан", _catalogItemService.GetCatalogItemElementName(result))));

        }


        public virtual JsonResult GetAvailableChildrenTypes(int id)
        {
            return Json(_catalogItemService.GetCreationCatalogItems(id), JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult SearchPropertyGridViewPartial(long id)
        {
            return View(id);
        }

        public virtual ActionResult SearchPropertyDeleteGridViewPartial(long id, long ciid)
        {
            _catalogItemSearchPropertyService.Delete(ciid, id);
            return View(MVC.CMS.Catalog.Views.SearchPropertyGridViewPartial, ciid);
        }
        
        [HttpPost]
        public virtual ActionResult AddSearchProperties(AddSearchPropertiesModel data)
        {
            _catalogItemSearchPropertyService.AddNew(data.ciid, data.ids);
            return Json(true);
        }



        #region SiteGroupPriceList


        public virtual ActionResult SiteGroupPriceListDetailGridViewPartial(long id)
        {
            ViewBag.CIID = id;


            return View(_siteGroupPriceListDataService.GetAllByCatalogItemID(id));
        }

        public virtual ActionResult SiteGroupPriceListGridViewPartial(long id, int? focusedRowIndex, int? draggingIndex, int? targetIndex, string command)
        {
            ViewBag.CIID = id;

            if (command == "DRAGROW")
            {
                var model = _siteGroupPriceListDataService.GetAllByCatalogItemID(id);

                var draggingRowKey = SiteGroupPriceListGridViewPartialGetKeyIdByDisplayIndex(draggingIndex, id);
                var targetRowKey = SiteGroupPriceListGridViewPartialGetKeyIdByDisplayIndex(targetIndex, id);
                var draggingDirection = (targetIndex < draggingIndex) ? 1 : -1;
                for (int rowIndex = 0; rowIndex < model.Count(); rowIndex++)
                {
                    var rowKey = SiteGroupPriceListGridViewPartialGetKeyIdByDisplayIndex(rowIndex, id);
                    if ((rowIndex > Math.Min(targetIndex.Value, draggingIndex.Value)) &&
                        (rowIndex < Math.Max(targetIndex.Value, draggingIndex.Value)))
                    {
                        SiteGroupPriceListGridViewPartialUpdateDisplayIndex(rowKey, rowIndex + draggingDirection, id);
                    }
                }
                SiteGroupPriceListGridViewPartialUpdateDisplayIndex(draggingRowKey, targetIndex, id);
                SiteGroupPriceListGridViewPartialUpdateDisplayIndex(targetRowKey, targetIndex + draggingDirection, id);
            }


            return View(_siteGroupPriceListDataService.GetAllByCatalogItemID(id));
        }

        [HttpPost]
        public virtual ActionResult SiteGroupPriceListGridViewPartialDeletePartial(long id)
        {
            var cID = _siteGroupPriceListDataService.Get(id).CatalogItemID;
            ViewBag.CIID = cID;


            if (ModelState.IsValid)
            {
                UpdateWrapper(() => _siteGroupPriceListDataService.Delete(cID, id));
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return View(MVC.CMS.Catalog.Views.SiteGroupPriceListGridViewPartial, _siteGroupPriceListDataService.GetAllByCatalogItemID(cID));
        }


        public long SiteGroupPriceListGridViewPartialGetKeyIdByDisplayIndex(int? displayIndex, long id)
        {
            var model = _siteGroupPriceListDataService.GetAllByCatalogItemID(id);
            var rowKey = model.Where(q => q.Priority == displayIndex).Select(q => q.ID).FirstOrDefault();
            return rowKey;
        }

        public void SiteGroupPriceListGridViewPartialUpdateDisplayIndex(long? rowKey, int? displayIndex, long id)
        {
            var model = _siteGroupPriceListDataService.GetAllByCatalogItemID(id);
            var product = model.Where(q => q.ID == rowKey).FirstOrDefault();
            product.Priority = displayIndex ?? 0;
            _siteGroupPriceListDataService.Update(product);
        }

        [HttpPost]
        public virtual ActionResult AddSiteGroupPriceListElements(AddPriceListElementsModel data)
        {
            _siteGroupPriceListDataService.AddNew(data.ciid, data.ids);
            return Json(true);
        }


        public virtual ActionResult SiteGroupPriceListDataCard(long id)
        {
            var item = _siteGroupPriceListDataService.Get(id);
            return View(MVC.CMS.Catalog.Views.SiteGroupPriceListDataCardLayout, item);

        }

        [ValidateInput(false)]
        [HttpPost]
        public virtual ActionResult SiteGroupPriceListDataCardSave(SiteGroupPriceListData item)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var saved = _siteGroupPriceListDataService.CreateOrUpdate(item);
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Данные успешно сохранены." });
                    ModelState.Clear();
                    return View(MVC.CMS.Catalog.Views.SiteGroupPriceListDataCard, saved);
                }
                catch (Exception e)
                {
                    _notificationBus.Notificate(new NotificationMessage() { Text = "Данные не были сохранены. " + e.Message }, NotificationType.Warning);
                    return View(MVC.CMS.Catalog.Views.SiteGroupPriceListDataCard, item);
                }

            }
            else
            {
                _notificationBus.Notificate(new NotificationMessage() { Text = "Данные не были сохранены." }, NotificationType.Warning);
                return View(MVC.CMS.Catalog.Views.SiteGroupPriceListDataCard, item);
            }
        }

        #endregion



        public virtual JsonResult RegenerateCatalogItemPage(long id, CatalogItemTypeValue? type)
        {
            OperationWrapper(() =>
            {
                this._notificationBus.Notificate(new NotificationMessage
                {
                    Text = "Генерация запущена"
                });
                _pageService.RegenerateCatalogItemPage(id, type);
                return true;
            }, "Генерация завершена");

            return Json(true);
        }

        public virtual JsonResult ClearCatalogItemPageCache(long id, CatalogItemTypeValue? type)
        {
            OperationWrapper(() =>
            {
                _pageService.ClearCatalogItemPageCache(id, type);
                return true;
            }, "Кэш сброшен");

            return Json(true);
        }

        public virtual JsonResult GetFlat(long id)
        {
            CatalogItemFlat[] flat;

            flat = _catalogItemService.GetFlat(id);

            return Json(flat);
        }

        public virtual JsonResult RestoreOrder(string parentKey)
        {
            _catalogItemService.RestoreOrder(parentKey);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult ChangeGrouping(long id, bool isgroup)
        {
            _catalogItemService.ChangeGrouping(id, isgroup);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult ChangeChain(long id, bool ischain)
        {
            _catalogItemService.ChangeChain(id, ischain);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult TransformToTemplate(long id, CatalogItemTypeValue? type = null)
        {
            _catalogItemService.TransformToTemplate(id, type);
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public virtual JsonResult ChangeGenericDesign(long id, bool isgroup)
        {

            _catalogItemService.ChangeGenericDesign(id, isgroup);

            return Json(true, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public virtual JsonResult SearchPriceListElement(string id)
        {

            IQueryable<PriceListElement> ple = _priceListElementService.RawDataQueryable;

            var children = ple.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();



            // var query = _makes.Where(m => m.text.ToLower().Contains(id.ToLower()));

            return Json(children, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetPriceListElement(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

             IQueryable<PriceListElement> ple = null;

            int idInt;
            if (int.TryParse(id, out idInt))
            {
                ple = _priceListElementService.RawDataQueryable.Where(x => x.ID == idInt); ;
            }


            var children = ple.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(children, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult SearchPage(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var secID = (long)CatalogItemTypeValue.Page;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).OrderBy(x=>x.Name).Skip((page -1)*pagelimit).Take(pagelimit);
            var count = _catalogItemService.RawDataQueryable.Count(x => x.CatalogItemTypeID == secID);
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new {items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetPage(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)CatalogItemTypeValue.Page;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID);

            int idInt;
            if (int.TryParse(id, out idInt))
            {
                var tag = sections.FirstOrDefault(m => m.ID == idInt);

                items.Add(new SelectItem
                {
                    id = (int)tag.ID,
                    text = tag.Name
                });
            }


            return Json(items, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult SearchSection(string id, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page -1)*pagelimit).Take(pagelimit);
            var count = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).Count(x => x.CatalogItemTypeID == secID);
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();
            
            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetSection(string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID);

            int idInt;
            if (int.TryParse(id, out idInt))
            {
                var tag = sections.FirstOrDefault(m => m.ID == idInt);

                items.Add(new SelectItem
                {
                    id = (int)tag.ID,
                    text = tag.Name
                });
            }


            return Json(items, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public virtual JsonResult SearchByType(string id, CatalogItemTypeValue type, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var secID = (long)type;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page -1)*pagelimit).Take(pagelimit);
            var count = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).Count(x => x.CatalogItemTypeID == secID);
          

            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();

            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetByType(string id, CatalogItemTypeValue type)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)type;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID);
            string[] idList = id.Split(new char[] { ',' });
            foreach (var s in idList)
            {
                if (string.IsNullOrEmpty(s))
                    continue;

                int idInt;
                if (int.TryParse(s, out idInt))
                {
                    var tag = sections.FirstOrDefault(m => m.ID == idInt);

                    items.Add(new SelectItem
                    {
                        id = (int)tag.ID,
                        text = tag.Name
                    });
                }
            }
           


            return Json(items, JsonRequestBehavior.AllowGet);
        }




        [HttpGet]
        public virtual JsonResult SearchChildLine(string id, long parentID, int page, int pagelimit)
        {
            var lid = id.ToLower();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.ParentID == parentID && x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).OrderBy(x => x.Name).Skip((page -1)*pagelimit).Take(pagelimit);
            var count = _catalogItemService.RawDataQueryable.Where(x => x.CatalogItemTypeID == secID && x.Name.ToLower().Contains(lid)).Count(x => x.CatalogItemTypeID == secID);
            var children = sections.Select(x => new SelectItem
            {
                id = (int)x.ID,
                text = x.Name
            }).ToList();


            return Json(new { items = children, count = count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public virtual JsonResult GetChildLine(string id, long parentID)
        {
            if (string.IsNullOrWhiteSpace(id)) return null;

            var items = new List<SelectItem>();
            var secID = (long)CatalogItemTypeValue.Line;
            var sections = _catalogItemService.RawDataQueryable.Where(x => x.ParentID == parentID && x.CatalogItemTypeID == secID);

            int idInt;
            if (int.TryParse(id, out idInt))
            {
                var tag = sections.FirstOrDefault(m => m.ID == idInt);

                items.Add(new SelectItem
                {
                    id = (int)tag.ID,
                    text = tag.Name
                });
            }


            return Json(items, JsonRequestBehavior.AllowGet);
        }

    }

    public class AddPriceListElementsModel
    {
        public long ciid { get; set; }
        public long[] ids { get; set; }
    }

    public class AddSearchPropertiesModel
    {
        public long ciid { get; set; }
        public long[] ids { get; set; }
    }
}
