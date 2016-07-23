using System.Collections.Generic;
using livemenu.Kernel.Rights;

namespace livemenu.Models.Rights
{
    public class SimpleRightsModel
    {
        public IEnumerable<SimpleRightItem> SimpleRightItems { get; set; } 
    }
}