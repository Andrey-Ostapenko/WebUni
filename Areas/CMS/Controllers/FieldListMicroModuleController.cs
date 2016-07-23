using System.Linq;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Areas.BWP.Models;
using livemenu.Common.Kernel.Attributes;
using livemenu.Common.Kernel.MicroModules;
using livemenu.Common.Kernel.MicroModules.ElementGroups;
using livemenu.Controllers;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.MicroModules;
using livemenu.Kernel.MicroModules.ElementGroups;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class FieldListMicroModuleController : CMSController
    {
        private readonly IMMConfigManager _mmConfigManager;
        private readonly IMMStorageManager _mmStorageManager;
        private readonly IMicroModuleService _microModuleService;
        private readonly IElementGroupFactory _elementGroupFactory;
        private readonly IElementGroupService _elementGroupService;
        private readonly IAttributeGroupsService _attributeGroupsService;

        public FieldListMicroModuleController(
            IMMConfigManager mmConfigManager, 
            IMMStorageManager mmStorageManager, 
            IMicroModuleService microModuleService, 
            IElementGroupFactory elementGroupFactory, 
            IElementGroupService elementGroupService, 
            IAttributeGroupsService attributeGroupsService,
            INotificationBus notificationBus)
        {
            _mmConfigManager = mmConfigManager;
            _mmStorageManager = mmStorageManager;
            _microModuleService = microModuleService;
            _elementGroupFactory = elementGroupFactory;
            _elementGroupService = elementGroupService;
            _attributeGroupsService = attributeGroupsService;
        }
        public virtual ActionResult Index(string code, string id)
        {
            //получаем kind по code

            //var kind = _microModuleService.GetByCodes(new List<string>() { code }).First().Kind;
            
            var mm = _microModuleService.GetByCodes(new []{ code }).Single();
            var kind = mm.Kind;
            var config = _mmConfigManager.GetMicroModuleConfig<FieldListMicroModuleConfig>(kind);

            var eg = _elementGroupService.GetElementGroupsByMicroModuleID(mm.ID).Single();
            
            //грузим аттрибуты элементов

            var attributes = _attributeGroupsService.LoadAttributesForElementGroup(eg.ID);

            var model = new FieldListModel
            {
                ID = id,
                Kind = code,
                Elements = eg.Element.Select(e => 
                    new ElementModel
                    {
                        Element = e,
                        MultipleAttributesIDs = attributes.Where(a=>a.ElementAttribute.Select(x=>x.ElementID).Contains(e.ID)).Select(x => x.ID).ToList()
                    }).ToList(),
                ElementGroupContainerConfig = config,
                ElementGroupID = eg.ID,
                MicroModuleID = mm.ID,
                Code = code,
            };


           
            return View(model);
        }

        public virtual ActionResult Edit(string code, string id)
        {
           

            return View();
        }

        [HttpPost]
        public virtual ActionResult Save(FieldListModel model)
        {
            ModelState.Clear();
            //UpdateWrapper(
              //  () =>
              //  {
                    _elementGroupFactory.Save(model.MicroModuleID, model.ElementGroupID, model.Elements.ToList());
            //    });

            return RedirectToAction(MVC.CMS.FieldListMicroModule.Index(model.Code, model.ID));
        }
	}
}