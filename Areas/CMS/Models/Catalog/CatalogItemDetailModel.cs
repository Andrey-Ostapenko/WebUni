using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace livemenu.Areas.BWP.Models.Catalog
{
    public class CatalogItemDetailModel
    {
        public IEnumerable<CatalogItemMMModel> MMs { get; set; }
        public long ID { get; set; }
    }

    public class CatalogItemMMModel
    {
        public string Code { get; set; }
        public long ID { get; set; }

        public string Name { get; set; }

        public long CatalogItemID { get; set; }
    }
}