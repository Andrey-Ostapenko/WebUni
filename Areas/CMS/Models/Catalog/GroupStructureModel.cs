using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace livemenu.Areas.CMS.Models.Catalog
{
    public class GroupStructureItemModel
    {
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public bool IsCollapse { get; set; }
    }
    public class GroupStructureModel
    {
        public List<GroupStructureItemModel>  Items { get; set; }
    }
}
