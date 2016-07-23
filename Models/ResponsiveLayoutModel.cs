using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace livemenu.Models
{
    public class ResponsiveLayoutModel
    {
        public string CustomID { get; set; }
        public string Name { get; set; }
        public bool IsIFrame { get; set; }

        public string InstructionLink { get; set; }

        public bool OnlyInstruction { get; set; }
        public bool WithoutInstruction { get; set; }
    }
}
