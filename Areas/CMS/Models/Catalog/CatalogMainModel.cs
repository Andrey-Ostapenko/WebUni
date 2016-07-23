using System;
using System.Collections.Generic;
using System.Web;

namespace livemenu.Areas.BWP.Models.Catalog
{
    public class CatalogMainModel
    {
        public long? RootID { get; set; }
        public long CatalogItemID { get; set; }
        public string CatalogItemCode { get; set; }


        public bool CanHaveChildren { get; set; }
    }
}