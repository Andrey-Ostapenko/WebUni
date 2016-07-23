using System.Linq;

namespace livemenu.Areas.BWP.Models.Catalog
{
    public class CatalogTreeItemModel
    {
        public int Priority { get; set; }
        public long? CatalogItemTypeID { get; set; }

        public string key { get; set; }
        public string title { get; set; }
        public string Code { get; set; }

        public bool folder { get; set; }

        public bool isActive { get; set; }
        public bool isLink { get; set; }
        public bool lazy { get; set; }

        public long? ParentID { get; set; }

        public long? RootID { get; set; }

        public string typeName { get; set; }

        public long ID { get; set; }
        public string ItemName { get; set; }
        public bool CanHaveChildren { get; set; }
        public string typeCode { get; set; }

        public static string MakeKey(long? parentId, long id, bool isLink = false)
        {
            return string.Format("{0}_{1}{2}", parentId ?? 0, id, isLink ? "_link" : string.Empty);
        }
        public static long GetIDFromKey(string key)
        {
            return long.Parse(key.Split('_')[1]);
        }

        public static long? GetParentIDFromKey(string key)
        {
            long res = 0;
            long.TryParse(key.Split('_')[0], out res);
            return res == 0 ? null : (long?)res;
        }
        public static bool IsKeyLink(string Key)
        {
            return (Key ?? string.Empty).Split('_').Count() > 2; //todo
        }
        public bool IsKeyLink()
        {
            return IsKeyLink(key);
        }

        public void RestoreIDAndParentIDFromKey()
        {
            ID = GetIDFromKey(key);
            ParentID = GetParentIDFromKey(key);
        }

        public bool HasShopModelPriceListElement { get; set; }
        public bool IsShopModeEnabled { get; set; }
        public bool IsShopModeInherits { get; set; }
        public bool IsBuyButtonModeEnabled { get; set; }
        public bool IsGrouped { get; set; }
        public bool IsGenericDesign { get; set; }
        public bool IsChain { get; set; }
        public int? TemplateType { get; set; }
        public bool FiltersEnabled { get; set; }
        public bool HasFilters { get; set; }

        public bool DynamicEntityEnabled { get; set; }
        public bool HasDynamicEntity { get; set; }

        public bool LinksEnabled { get; set; }
        public bool HasLinks { get; set; }

        public int? ActionType { get; set; }
        public string ElementName { get; set; }

        public bool AllowDelete { get; set; }
        public bool AllowCreate { get; set; }
        public bool AllowMove { get; set; }
        public bool AllowCopy { get; set; }
        public bool AllowLink { get; set; }
    }
}