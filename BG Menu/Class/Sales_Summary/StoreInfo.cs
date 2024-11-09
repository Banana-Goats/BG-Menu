namespace BG_Menu.Class.Sales_Summary
{
    public class StoreInfo
    {
        public string StoreName { get; set; }
        public string[] StoreNames { get; set; }
        public string[] WarehouseNames { get; set; }
        public string ExcludeItemGroup { get; set; }
        public string IncludeItemGroup { get; set; }
        public string StartWeek { get; set; }
        public string EndWeek { get; set; }
        public string[] IncludeTargetOnlyStoreNames { get; set; }
    }
}