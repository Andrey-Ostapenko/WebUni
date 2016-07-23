using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using livemenu.Kernel.Authentication;
using livemenu.Kernel.Configuration;

namespace livemenu.Models.Menu
{
    public class MenuModel
    {
        public IEnumerable<MenuPathConfig> MicroModuleMenuItemsModel { get; set; }

        public AccessMode AccessMode { get; set; }
    }
}