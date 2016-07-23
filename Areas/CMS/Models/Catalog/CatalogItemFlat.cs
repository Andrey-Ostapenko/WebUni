using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace livemenu.Areas.CMS.Models.Catalog
{
    public class CatalogItemFlat
    {
        public long CatalogItemID { get; set; }
        public string CatalogItemName { get; set; }

        public long CatalogItemTypeID { get; set; }
        public long UniBlockID { get; set; }
    }
}