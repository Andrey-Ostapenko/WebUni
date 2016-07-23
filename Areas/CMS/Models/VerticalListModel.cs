using System.Collections.Generic;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Configuration;

namespace livemenu.Areas.BWP.Models
{

    public class VerticalListModel:IInternalWidgetModelBase
    {
        public IEnumerable<ElementConfig> Headers { get; set; }
        public IEnumerable<ElementGroup> ElementGroups { get; set; }

        public long MMID { get; set; }
        public string Kind { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public string ID { get; set; }
    }
}