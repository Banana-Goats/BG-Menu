using System;

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

    public class StoreSales
    {
        public string Store { get; set; }
        public int Week { get; set; }
        public decimal Sales { get; set; }
    }

    public class StoreTarget
    {
        public string Store { get; set; }
        public int Week { get; set; }
        public decimal Target { get; set; }
    }
}
