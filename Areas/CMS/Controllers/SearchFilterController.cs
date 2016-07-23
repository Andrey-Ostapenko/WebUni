using livemenu.Kernel.BWP.SearchFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace livemenu.Areas.CMS.Controllers
{
    public partial class SearchFilterController : CMSController
    {
        private readonly IUniSearchFilterService _uniSearchFilterService;

        public SearchFilterController(IUniSearchFilterService uniSearchFilterService)
        {
            _uniSearchFilterService = uniSearchFilterService;
        }

        [HttpGet]
        public virtual JsonResult SearchSearchFilter(string id)
        {
            var lid = id.ToLower();
            var filters = _uniSearchFilterService.RawDataQueryable
                .Where(f => f.UniSearchFilterGroupRelation.Count == 0)
                .Where(f => f.CatalogItem.Name.ToLower().Contains(lid))
                .Select(f => new SelectItem
                 {
                     id = (int)f.ID,
                     text = f.CatalogItem.Name
                 })
                .ToList();

            return Json(filters, JsonRequestBehavior.AllowGet);
        }
    }
}