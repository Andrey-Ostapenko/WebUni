using System.Collections.Generic;
using livemenu.Common;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.MicroModules;

namespace livemenu.Areas.BWP.Models
{
    public class FieldListMicroModule : MicroModuleBase<FieldListMicroModuleConfig>
    {
        public FieldListMicroModule()
        {
            this.Operations = new List<MicroModuleOperation>()
                {
                };
        }

        public override IMicroModuleBaseT<FieldListMicroModuleConfig> SetSettings(MicroModule mm, FieldListMicroModuleConfig config)
        {
            base.SetSettings(mm, config);

            // по хорошему, сюда надо запихнуть сам ммодуль, но это строка поедет на клиент и будет вызываться оттуда, 
            // а контроллер может быть один на несколько ммодулей, поэтому передаем код, по которому внутри уже нужно будет прогрузить сам ммодуль
            Url = MVC.CMS.FieldListMicroModule.Index(mm.Code, string.Empty);
            return this;

        }

        protected override void InitWidgetSettings()
        {
            base.InitWidgetSettings();

            this.WidgetSettings.AvailableMenuItems = new List<StandartWidgetMenuItem>() { StandartWidgetMenuItem.Save  };
        }

        public static string Type
        {
            get { return "FieldList"; }
        }
    }
}