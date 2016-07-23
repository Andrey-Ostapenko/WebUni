namespace livemenu.Models.Rights.Catalogs
{
    public class RightsCatalogModel
    {
        public long RightSubjectID { get; set; }
        public long CatalogItemID { get; set; }

        public bool IsRoot { get; set; }
    }
}