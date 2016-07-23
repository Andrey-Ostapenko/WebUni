using System.Linq;
using System.Web.Mvc;
using BWP.Kernel.Notification;
using livemenu.Areas.BWP.Models;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.MicroModules;
using livemenu.Common.Kernel.MicroModules.ElementGroups;
using livemenu.Controllers;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.MicroModules;
using livemenu.Kernel.MicroModules.ElementGroups;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class VerticalListMicroModuleController : CMSController
    {
        private readonly IMMConfigManager _mmConfigManager;
        private readonly IElementGroupFactory _elementGroupFactory;
        private readonly IMicroModuleService _microModuleService;
        private readonly IMMStorageManager _mmStorageManager;
        private readonly IElementGroupService _elementGroupService;

        public VerticalListMicroModuleController(
            IMMConfigManager mmConfigManager, 
            IElementGroupFactory elementGroupFactory, 
            IMicroModuleService microModuleService, 
            IMMStorageManager mmStorageManager, 
            IElementGroupService elementGroupService, 
            INotificationBus notificationBus)
        {
            _mmConfigManager = mmConfigManager;
            _elementGroupFactory = elementGroupFactory;
            _microModuleService = microModuleService;
            _mmStorageManager = mmStorageManager;
            _elementGroupService = elementGroupService;
        }
      
        public virtual ActionResult Index(string code, string id)
        {
            var mm = _microModuleService.GetByCodes(new []{ code }).Single();
            var kind = mm.Kind;

            var config = _mmConfigManager.GetMicroModuleConfig<VerticalListMicroModuleConfig>(kind);

            var model = new VerticalListModel
            {
                Headers = config.GetMasterCaptions(),
                ID = id,
                Kind = code,
                MMID = mm.ID,
                ElementGroups = _elementGroupService.GetElementGroupsByMicroModuleID(mm.ID)
            };

            return View(model);
        }

        public virtual ActionResult Edit(string code, string uiID, long mmID, long id)
        {
            var kind = _microModuleService.GetByID(mmID).Kind;
            var config = _mmConfigManager.GetMicroModuleConfig<VerticalListMicroModuleConfig>(kind);

            var model = new VerticalListEditModel
            {
                ElementGroupContainerConfig = config,
                MicroModuleID = mmID,
                Code = code,
                UIID = uiID
            };

            ElementGroup eg = null;

            if (id == 0) //надо сгенерировать ГЭ
            {
                eg = _elementGroupFactory.GenerateElementGroupForMicroModule(config);
                model.ElementGroupID = 0;
            }
            else
            {
                eg = _elementGroupService.GetElementGroupByID(id);
                model.ElementGroupID = eg.ID;
            }

            model.Elements = eg.Element.Select(e => new ElementModel { Element = e }).ToList();
            
            return View(model);
        }

        public virtual ActionResult Save(VerticalListEditModel model)
        {
            long egID = 0;
            ModelState.Clear();
           // UpdateWrapper(() =>
           // {
                egID = _elementGroupFactory.Save(model.MicroModuleID, model.ElementGroupID, model.Elements.ToList());
           // });

            return RedirectToAction(MVC.CMS.VerticalListMicroModule.Edit(model.Code, model.UIID, model.MicroModuleID, egID));
        }

        [HttpPost]
        public virtual ActionResult Delete(long id)
        {
            UpdateWrapper(() => _elementGroupService.DeleteElementGroup(new[] { id }));
            return null;
        }

    }
}
