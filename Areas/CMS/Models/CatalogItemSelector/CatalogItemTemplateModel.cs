using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livemenu.Common.Entities;

namespace livemenu.Areas.CMS.Models.CatalogItemSelector
{
    public class CatalogItemTemplateItemModel
    {
        public long ID { get; set; }    
        public bool IsSelected { get; set; }
        public string Name { get; set; }
    }

    public class CatalogItemTemplateModel
    {
        public CatalogItemTypeValue Type { get; set; }
        public long ID { get; set; }
        public List<CatalogItemTemplateItemModel> Items { get; set; }
    }
}
