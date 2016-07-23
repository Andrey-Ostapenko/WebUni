using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using livemenu.Common.Entities.Entities;

namespace livemenu.Areas.BWP.Models.CatalogItemSelector
{
    public class CatalogItemSelectorModel
    {
        public long CIID { get; set; }
        public List<CatalogItem> CatalogItems { get; set; }
    }
}