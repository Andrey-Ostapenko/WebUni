using System.Collections.Generic;
using livemenu.Kernel.Configuration;
using livemenu.Kernel.MicroModules.ElementGroups;

namespace livemenu.Areas.BWP.Models
{


    public class VerticalListEditModel : ElementGroupEditModel, IInternalWidgetModelBase
    {
    //    public List<ElementConfig> ElementGroupConfig { get; set; }
    //    public long ElementGroupID { get; set; }
        public long MicroModuleID { get; set; }
   //     public List<ElementModel> Elements { get; set; }
       
        public string Kind { get; set; }
        public string Code { get; set; }
        public string State { get; set; }
        public string ID { get; set; }
        public string UIID { get; set; }
    }

    public class ElementGroupEditModel
    {
        public ElementGroupContainerConfig ElementGroupContainerConfig { get; set; }
        //public List<ElementConfig> ElementGroupConfig { get; set; }
        public long ElementGroupID { get; set; }
        public List<ElementModel> Elements { get; set; }
    }
}