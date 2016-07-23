using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using livemenu.Common.Entities;

namespace livemenu.Areas.CMS.Models
{
    public enum CatalogItemCodes
    {
        Site = 1,
        Templates,
        Elements,
        Projects,
        Actions,
        Vacancies,
        News,
        Goods = 11
    }

    public static class CatalogItemTypeCodeHelpers
    {
        public static Dictionary<CatalogItemTypeValue, CatalogItemCodes> CatalogItemTypeCodeMap = new Dictionary<CatalogItemTypeValue, CatalogItemCodes>()
        {
            {CatalogItemTypeValue.Projects, CatalogItemCodes.Projects },
            {CatalogItemTypeValue.Layouts, CatalogItemCodes.Templates },
            {CatalogItemTypeValue.Elements, CatalogItemCodes.Elements },
        };
    }

    public static class CatalogItemCodesExtensions
    {
        public static long ToLong(this CatalogItemCodes code)
        {
            return (long)code;
        }
    }

    
}
