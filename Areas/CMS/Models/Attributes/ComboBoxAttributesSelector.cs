using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using livemenu.Common.Entities.Entities;

namespace livemenu.Areas.BWP.Models.Attributes
{
    public class AttributesEditModel
    {
        public string AttributesGroupCode { get; set; }
        public List<BWPAttribute> Attributes { get; set; }
    }
    public class ComboBoxAttributesSelector
    {
        public string uID { get; set; }
        public List<BWPAttribute> AttributeList { get; set; }

        public long? Value { get; set; }
        public string AgCode { get; set; }
    }
}