using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using livemenu.Common;

namespace livemenu.Models.Widgets
{
    public class WidgetsViewModel
    {
        public WidgetsViewModel()
        {
            IsPartial = false;
        }
        public IEnumerable<IWidgetModel> Widgets { get; set; }

        public bool IsPartial { get; set; }
    }
}