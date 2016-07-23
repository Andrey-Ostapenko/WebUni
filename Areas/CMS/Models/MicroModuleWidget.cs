using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using livemenu.Common;
using livemenu.Kernel.MicroModules;

namespace livemenu.Areas.BWP.Models
{
    public class MicroModuleWidget : WidgetModelBase
    {
        private readonly IMicroModuleBase _microModule;

        public MicroModuleWidget(IMicroModuleBase microModule, WidgetSettings settings = null)
        {
            if (settings != null)
                this.Settings = settings;
            _microModule = microModule;
            this.Title = _microModule.Title;
            this.Url = _microModule.Url;
            this.Kind = _microModule.Kind;
            this.Code = _microModule.MM.Code;
            this.MenuItems = _microModule.Operations != null ? _microModule.Operations.Select(x => new WidgetMenuItemModel() { Name = x.Name, Url = x.Url }) : null;
        }
    }
}