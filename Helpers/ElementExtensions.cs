using System.Collections.Generic;
using System.Linq;
using livemenu.Kernel.Configuration;

namespace livemenu.Helpers
{
    public static class ElementExtensions
    {
        public static string GetElementLocalizedNameByCode(this List<ElementConfig> elementGroupConfig, string code)
        {
            return elementGroupConfig.First(e => e.Code == code).LocalizedNames.GetStringForCurrentLocale();
        }

        public static ElementConfig GetElementConfigByCode(this List<ElementConfig> elementGroupConfig, string code)
        {
            return elementGroupConfig.First(e => e.Code == code);
        }

    }
}