using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using livemenu.Areas.BWP.Models.Attributes;
using livemenu.Common.Entities.Entities;
using livemenu.Common.Kernel.Attributes;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class AttributesController : CMSController
    {
        private readonly IAttributeGroupsService _attributeGroupsService;

        public AttributesController(IAttributeGroupsService attributeGroupsService)
        {
            _attributeGroupsService = attributeGroupsService;
        }

        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult ComboBoxAttributesSelector(string uID, string agCode, long? id)
        {
            return PartialView(new ComboBoxAttributesSelector
            {
                AttributeList = _attributeGroupsService.Get(agCode).Attributes.ToList(),
                Value = id,
                AgCode = agCode,
                uID = uID

            });
        }

        public virtual ActionResult EditPartial(string agCode)
        {
            return PartialView(new AttributesEditModel
            {
                AttributesGroupCode = agCode,
                Attributes = _attributeGroupsService.Get(agCode).Attributes.ToList()
            });

        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult EditAddNewPartial(BWPAttribute attribute, string agCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _attributeGroupsService.Create(attribute, agCode);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";
            return PartialView("EditPartial", new AttributesEditModel
            {
                AttributesGroupCode = agCode,
                Attributes = _attributeGroupsService.Get(agCode).Attributes.ToList()
            });
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult EditUpdatePartial(BWPAttribute attribute, string agCode)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _attributeGroupsService.Update(attribute, agCode);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            else
                ViewData["EditError"] = "Please, correct all errors.";

            return PartialView("EditPartial", new AttributesEditModel
            {
                AttributesGroupCode = agCode,
                Attributes = _attributeGroupsService.Get(agCode).Attributes.ToList()
            });
        }

        [HttpPost, ValidateInput(false)]
        public virtual ActionResult EditDeletePartial(int id, string agCode)
        {
            if (id >= 0)
            {
                try
                {
                    _attributeGroupsService.Delete(id);
                }
                catch (Exception e)
                {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("EditPartial", new AttributesEditModel
            {
                AttributesGroupCode = agCode,
                Attributes = _attributeGroupsService.Get(agCode).Attributes.ToList()
            });
        }

    }
}