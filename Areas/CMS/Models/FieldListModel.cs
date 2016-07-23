using System.Collections.Generic;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.MicroModules.ElementGroups;

namespace livemenu.Areas.BWP.Models
{
    public class FieldListModel : ElementGroupEditModel, IInternalWidgetModelBase
    {
     //   public GeneratedFieldsMicroModuleConfig Config { get; set; }
        //public List<ElementModel> Elements { get; set; }

        //
        //public long ElementGroupID { get; set; }
   //     public FieldListMicroModuleConfig FieldListMicroModuleConfig { get; set; }


        public long MicroModuleID { get; set; }
        public string Kind { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public string ID { get; set; }
    }


   
}