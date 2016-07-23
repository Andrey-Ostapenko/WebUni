using System.Collections.Generic;
using livemenu.Common;
using livemenu.Common.Entities.Entities;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.MicroModules;

namespace livemenu.Areas.BWP.Models
{
    public class VerticalListMicroModule : MicroModuleBase<VerticalListMicroModuleConfig>
    {
        public VerticalListMicroModule()
        {
            this.Operations = new List<MicroModuleOperation>()
                {
                  //  {new MicroModuleOperation(){Name = "Добавить", Url = MVC.BeautifulLinksMicroModule.Add()}},
                    //{new MicroModuleOperation(){Name = "Удалить", Url = MVC.BeautifulLinksMicroModule.Remove()}}
                };

        
        }

        public IEnumerable<ElementConfig> GetMasterCaptions()
        {
            return  this.config.GetMasterCaptions();
        }

        protected override void InitWidgetSettings()
        {
            base.InitWidgetSettings();

            this.WidgetSettings.AvailableMenuItems = new List<StandartWidgetMenuItem>(){ };
        }

        public override IMicroModuleBaseT<VerticalListMicroModuleConfig> SetSettings(MicroModule mm, VerticalListMicroModuleConfig config)
        {
            base.SetSettings(mm, config);

            // по хорошему, сюда надо запихнуть сам ммодуль, но это строка поедет на клиент и будет вызываться оттуда, 
            // а контроллер может быть один на несколько ммодулей, поэтому передаем код, по которому внутри уже нужно будет прогрузить сам ммодуль
            Url = MVC.CMS.VerticalListMicroModule.Index(mm.Code, string.Empty);
            return this;

        }
      
        public static string Type
        {
            get { return "VerticalList"; }
        }
    }
}