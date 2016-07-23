﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livemenu.Models;

namespace livemenu.Areas.Projects.Models
{
    public class DynamicTableGridViewModel
    {
        public long? ApplicationID { get; set; }
    }

    public class DynamicTableResponsiveLayoutModel : ResponsiveLayoutModel
    {
        public long? ApplicationID { get; set; }
    }
}
