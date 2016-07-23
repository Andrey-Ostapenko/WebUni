using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livemenu.Common.Kernel.Settings;

namespace livemenu.Models.Account
{
    public class AccountWebUniLicenseModel
    {
        public bool HasLicense { get; set; }
        public WebUniLicense WebUniLicense { get; set; }
        public string TypeName { get; set; }
        public string TariffName { get; set; }

        public string PeriodName { get; set; }
        public DateTime EndDate { get; set; }
    }
}
