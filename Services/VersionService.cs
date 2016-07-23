using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livemenu.Kernel.Configuration;

namespace livemenu.Services
{
    public class VersionService : IVersionService
    {
        public string GetVersion()
        {
           return System.Reflection.Assembly.GetExecutingAssembly()
                                             .GetName()
                                             .Version
                                             .ToString();
        }
    }
}
