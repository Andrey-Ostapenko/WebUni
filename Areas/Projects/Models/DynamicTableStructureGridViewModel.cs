using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livemenu.Common.Entities.Entities;
using livemenu.Models;

namespace livemenu.Areas.Projects.Models
{
    public class DynamicTableStructureGridViewModel
    {
        public DynamicTable Table { get; set; }
        public long? ApplicationID { get; set; }
    }
}
