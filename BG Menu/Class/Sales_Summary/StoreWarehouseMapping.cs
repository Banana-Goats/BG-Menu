using System.Collections.Generic;

namespace BG_Menu.Class.Sales_Summary
{
    public static class StoreWarehouseMapping
    {
        public static List<StoreInfo> GetUKStoreMapping()
        {
            return new List<StoreInfo>
            {
                new StoreInfo { StoreName = "Birkenhead", WarehouseNames = new[] { "Ableworld Birkenhead", "Birkenhead Off-Line" } },
                new StoreInfo { StoreName = "Burton", WarehouseNames = new[] { "Ableworld Burton", "Burton Off-Line" } },
                new StoreInfo { StoreName = "Chester", WarehouseNames = new[] { "Ableworld Chester", "Chester Off-Line" } },
                new StoreInfo { StoreName = "Congleton", WarehouseNames = new[] { "Ableworld Congleton", "Congleton Off-Line" } },
                new StoreInfo { StoreName = "Cheltenham", WarehouseNames = new[] { "Ableworld Cheltenham", "Cheltenham Off-Line" } },
                new StoreInfo { StoreName = "Crewe", WarehouseNames = new[] { "Ableworld Crewe", "Crewe Off-Line" } },
                new StoreInfo { StoreName = "Darlington", WarehouseNames = new[] { "Ableworld Darlington", "Darlington Offline" } },
                new StoreInfo { StoreName = "Gloucester", WarehouseNames = new[] { "Ableworld Gloucester", "Gloucester Offline" } },
                new StoreInfo { StoreName = "Hanley", WarehouseNames = new[] { "Ableworld Hanley", "Hanley Off-Line" } },
                new StoreInfo { StoreName = "Lincoln", WarehouseNames = new[] { "Ableworld Lincoln", "Lincoln Offline" } },
                new StoreInfo { StoreName = "Llandudno", WarehouseNames = new[] { "Ableworld Llandudno", "Llandudno Off-Line" } },
                new StoreInfo { StoreName = "Nantwich", WarehouseNames = new[] { "Ableworld Nantwich", "Nantwich Off-Line" } },
                new StoreInfo { StoreName = "Newark", WarehouseNames = new[] { "Ableworld Newark", "Newark Offline" } },
                new StoreInfo { StoreName = "Newport", WarehouseNames = new[] { "Ableworld Newport", "Newport Off-Line" } },
                new StoreInfo { StoreName = "Northwich", WarehouseNames = new[] { "Ableworld Northwich", "Northwich Off-Line" } },
                new StoreInfo { StoreName = "Oswestry", WarehouseNames = new[] { "Ableworld Oswestry", "Oswestry Off-Line" } },
                new StoreInfo { StoreName = "Queensferry", WarehouseNames = new[] { "Ableworld Queensferry", "Queensferry Off-line" } },
                new StoreInfo { StoreName = "Reading", WarehouseNames = new[] { "Ableworld Reading", "Reading Offline" } },
                new StoreInfo { StoreName = "Rhyl", WarehouseNames = new[] { "Ableworld Rhyl", "Rhyl Off-Line" } },
                new StoreInfo { StoreName = "Runcorn", WarehouseNames = new[] { "Ableworld Runcorn", "Runcorn Offline" } },
                new StoreInfo { StoreName = "Shrewsbury", WarehouseNames = new[] { "Ableworld Shrewsbury", "Shrewsbury Off-Line" } },
                new StoreInfo { StoreName = "Stafford", WarehouseNames = new[] { "Ableworld Stafford", "Stafford Off-Line" } },
                new StoreInfo { StoreName = "Stockport", WarehouseNames = new[] { "Ableworld Stockport", "Stockport Off-Line" } },
                new StoreInfo { StoreName = "Stockton", WarehouseNames = new[] { "Ableworld Stockton", "Stockton Offline" } },
                new StoreInfo { StoreName = "Thatcham", WarehouseNames = new[] { "Ableworld Thatcham", "Thatcham Off-Line" } },
                new StoreInfo { StoreName = "Wrexham", WarehouseNames = new[] { "Ableworld Wrexham", "Wrexham Off-Line" } },

                // Engineering
                new StoreInfo { StoreName = "Engineering", WarehouseNames = new[]
                {   "Ableworld Birkenhead Engineering  W/shop",
                    "Ableworld Birkenhead Engineering  W/shop Off-line",
                    "Ableworld Burton Engineering  W/shop",
                    "Ableworld Burton Engineering  W/shop Off-line",
                    "Ableworld Crewe Engineering W/shop",
                    "Ableworld Crewe Engineering W/shop Off-Line",
                    "Ableworld Darlington Engineering W/shop",
                    "Ableworld Darlington Engineering W/shop Offline",
                    "Ableworld Gloucester Engineering W/Shop",
                    "Ableworld Gloucester Engineering W/Shop Offline",
                    "Ableworld Hanley Engineering  W/shop",
                    "Ableworld Hanley Engineering  W/shop Off-line",
                    "Ableworld Lincoln Engineering W/shop",
                    "Ableworld Lincoln Engineering W/shop Off-line",
                    "Ableworld Llandudno Engineering W/shop",
                    "Ableworld Llandudno Engineering W/shop Off-Line",
                    "Ableworld Newport Engineering W/shop",
                    "Ableworld Newport Engineering W/shop Off-line",
                    "Ableworld Newark Engineering W/shop",
                    "Ableworld Newark Engineering W/shop Off-Line",
                    "Ableworld Queensferry Engineering W/shop",
                    "Ableworld Queensferry Engineering W/shop Off-line",
                    "Ableworld Reading Engineering  W/shop",
                    "Ableworld Reading Engineering  W/shop Offline",
                    "Ableworld Rhyl Engineering  W/shop",
                    "Ableworld Rhyl Engineering  W/shop Off-line",
                    "Ableworld Runcorn Engineering W/shop",
                    "Ableworld Runcorn Engineering W/shop Off-line",
                    "Ableworld Shrewsbury Engineering  W/shop",
                    "Ableworld Shrewsbury Engineering  W/shop Off-line",
                    "Ableworld Stafford Engineering  W/shop",
                    "Ableworld Stafford Engineering  W/shop Off-line",
                    "Ableworld Stapeley Engineering  W/shop",
                    "Ableworld Stapeley Engineering  W/shop Off-line",
                    "Ableworld Stockport Engineering  W/shop",
                    "Ableworld Stockport Engineering  W/shop Off-line",
                    "Ableworld Stockton Engineering W/shop",
                    "Ableworld Stockton Engineering W/shop Offline",
                    "Ableworld Thatcham Engineering  W/shop",
                    "Ableworld Thatcham Engineering  W/shop Offline"
                } },

                // Stairlifts
                new StoreInfo { StoreName = "Stairlifts", WarehouseNames = new[]
                {
                    "Ableworld Stairlfts Shrewsbury",
                    "Ableworld Stairlfts Shrewsbury Off-Line",
                    "Ableworld Stairlift Hub",
                    "Ableworld Stairlift Hub Off-Line",
                    "Ableworld Stairlifts Birkenhead",
                    "Ableworld Stairlifts Birkenhead Off-Line",
                    "Ableworld Stairlifts Burton",
                    "Ableworld Stairlifts Burton Off-Line",
                    "Ableworld Stairlifts Darlington",
                    "Ableworld Stairlifts Darlington Offline",
                    "Ableworld Stairlifts Gloucester",
                    "Ableworld Stairlifts Gloucester Offline",
                    "Ableworld Stairlifts Hanley offline",
                    "Ableworld Stairlifts Hanley/Fenton",
                    "Ableworld Stairlifts Lincoln",
                    "Ableworld Stairlifts Lincoln Offline",
                    "Ableworld Stairlifts Queensferry",
                    "Ableworld Stairlifts Queensferry Offline",
                    "Ableworld Stairlifts Reading",
                    "Ableworld Stairlifts Reading Offline",
                    "Ableworld Stairlifts Rhyl",
                    "Ableworld Stairlifts Rhyl Off-Line",
                    "Ableworld Stairlifts Runcorn",
                    "Ableworld Stairlifts Runcorn Offline",
                    "Ableworld Stairlifts Stafford",
                    "Ableworld Stairlifts Stafford Offline",
                    "Ableworld Stairlifts Stockport",
                    "Ableworld Stairlifts Stockport Off-Line",
                    "Ableworld Stairlifts Thatcham",
                    "Ableworld Stairlifts Thatcham Offline",
                    "Ableworld Stairlifts Stapeley Off-Line",
                    "Ableworld Stairlifts Stapeley",
                    "Ableworld Stairlifts Stockton Offline",
                    "Ableworld Stairlifts Stockton",
                    "MNE Stairlifts Off-Line",
                    "Cheltenham Stairlifts",
                    "Cheltenham Stairlifts Off-Line",
                } },

                //Specialist
                new StoreInfo { StoreName = "Specialist", WarehouseNames = new[]
                {   "Ableworld Specialist Crewe",
                    "Ableworld Specialist Crewe Off-Line",
                    "Ableworld Specialist Hanley",
                    "Ableworld Specialist Hanley Off-Line",
                    "Ableworld Specialist Shrewsbury",
                    "Ableworld Specialist Shrewsbury Off-Line",
                    "Ableworld Specialist Stafford",
                    "Ableworld Specialist Stafford Off-Line",
                    "Ableworld Specialist Nantwich",
                    "Ableworld Specialist Nantwich Off-Line"
                } },
            };
        }

        public static List<StoreInfo> GetFranchiseStoreMapping()
        {
            return new List<StoreInfo>
            {
                // MGB
                new StoreInfo { StoreName = "Blackpool", WarehouseNames = new[] { "Ableworld Blackpool", "Blackpool Offline" } },
                new StoreInfo { StoreName = "Hyde", WarehouseNames = new[] { "Ableworld Hyde", "Hyde Offline" } },
                new StoreInfo { StoreName = "Salford", WarehouseNames = new[] { "Ableworld Salford", "Salford Offline" } },
                new StoreInfo { StoreName = "Southport", WarehouseNames = new[] { "Ableworld Southport", "Southport Offline" } },
                new StoreInfo { StoreName = "St Helens", WarehouseNames = new[] { "Ableworld St Helens", "St Helens Offline" } },
                new StoreInfo { StoreName = "Wavertree", WarehouseNames = new[] { "Ableworld Wavertree", "Wavertree Offline" } },
                new StoreInfo { StoreName = "Wigan", WarehouseNames = new[] { "Ableworld Wigan", "Wigan Offline" } },


                // JSCD
                new StoreInfo { StoreName = "Bridgend", WarehouseNames = new[] { "Bridgend", "Bridgend Off-Line" } },
                new StoreInfo { StoreName = "Cardiff", WarehouseNames = new[] { "Cardiff", "Cardiff Off-Line" } },
                new StoreInfo { StoreName = "Newport Wales", WarehouseNames = new[] { "Newport Wales", "Newport Wales Off-Line" } },

                // SJLK
                new StoreInfo { StoreName = "Paisley", WarehouseNames = new[] { "Paisley", "Paisley Off-Line" }, ExcludeItemGroup = "Stairlifts" },

                // AMD
                new StoreInfo { StoreName = "Broxburn", WarehouseNames = new[] { "Broxburn", "Broxburn Off-Line" }, ExcludeItemGroup = "Stairlifts" },

                // SML
                new StoreInfo { StoreName = "Christchurch", WarehouseNames = new[] { "Christchurch", "Christchurch Off-Line" }, ExcludeItemGroup = "Stairlifts" },
                new StoreInfo { StoreName = "Southampton", WarehouseNames = new[] { "Southampton", "Southampton Off-Line" }, ExcludeItemGroup = "Stairlifts" },

                // GRMR
                new StoreInfo { StoreName = "Colchester", WarehouseNames = new[] { "Colchester", "Colchester Off-Line" }, ExcludeItemGroup = "Stairlifts" },

                // AWG
                new StoreInfo { StoreName = "Leeds", WarehouseNames = new[] { "Leeds", "Leeds - Offline" }, ExcludeItemGroup = "Stairlifts" },

                new StoreInfo { StoreName = "DropShip", WarehouseNames = new[] { "Maeko Dropship Warehouse AWUK", "Ableworld Head Office", "Head Office Off-Line" } },
            };
        }

        public static List<StoreInfo> GetCompanyMapping()
        {
            return new List<StoreInfo>
            {
                new StoreInfo { StoreName = "AWG", WarehouseNames = new[] { "Leeds", "Leeds - Offline", "Maeko Dropship Warehouse - AWG Ltd" } },
                new StoreInfo { StoreName = "AMD", WarehouseNames = new[] { "Broxburn", "Broxburn Off-Line", "Maeko dropship warehouse - AMD" } },
                new StoreInfo { StoreName = "GRMR", WarehouseNames = new[] { "Colchester", "Colchester Off-Line", "Colchester Stairlifts", "Colchester Stairlifts Off-Line", "Maeko dropship warehouse - GRMR" } },
                new StoreInfo { StoreName = "JSCD", WarehouseNames = new[] { "Cardiff", "Bridgend", "Newport Wales", "Cardiff Off-Line", "Bridgend Off-Line", "JSCD Stairlifts", "Newport Wales Off-Line", "Maeko Dropship Warehouse - JSCD", "JSCD Stairlifts - Offline" } },
                new StoreInfo { StoreName = "Mobility GB", WarehouseNames = new[] { "Ableworld Wavertree", "Ableworld Wigan", "Ableworld Southport", "Ableworld St Helens", "Ableworld Salford", "Ableworld Blackpool", "Ableworld Hyde", "Hyde Offline", "Southport Offline", "MGB Stairlifts", "MGB Engineering", "Maeko Dropship Warehouse - MGB", "Wigan Offline", "Blackpool Offline", "St Helens Offline", "Wavertree Offline" } },
                new StoreInfo { StoreName = "SJLK", WarehouseNames = new[] { "Paisley", "Paisley Off-Line", "Maeko Dropship Warehouse - SJLK" } },
                new StoreInfo { StoreName = "SML", WarehouseNames = new[] { "SML Stairlifts", "SML Stairlifts - Offline", "Christchurch", "Christchurch Off-Line", "Southampton", "Southampton Off-Line", "Maeko dropship warehouse - SML" } },
            };
        }

        public static List<StoreInfo> GetAggregatedStoreMapping()
        {
            return new List<StoreInfo>
            {
             new StoreInfo { StoreName = "UK", StoreNames = new[] { "Birkenhead", "Burton", "Chester", "Congleton", "Cheltenham", "Crewe",
                 "Darlington", "Gloucester", "Hanley", "Lincoln", "Llandudno", "Nantwich", "Newark", "Newport", "Northwich",
                 "Oswestry", "Queensferry", "Reading", "Rhyl", "Runcorn", "Shrewsbury","Stafford", "Stockport", "Stockton", "Thatcham", "Wrexham", "Stairlifts", "Specialist", "DropShip" }, IncludeTargetOnlyStoreNames = new[] { "Engineering" } },


             new StoreInfo { StoreName = "Franchise Total", StoreNames = new[] { "AWG", "AMD", "GRMR", "JSCD", "Mobility GB", "SJLK", "SML"} },
             new StoreInfo { StoreName = "Company Total", StoreNames = new[] { "UK", "Franchise Total"} },


             new StoreInfo { StoreName = "North A", StoreNames = new[] { "AMD", "Darlington", "Hyde", "Lincoln", "Newark", "Northwich", "SJLK", "Queensferry", "Southport", "St Helens", "Stockton" } },
             new StoreInfo { StoreName = "North B", StoreNames = new[] { "Birkenhead", "Blackpool", "Chester", "AWG", "Llandudno", "Rhyl", "Runcorn", "Salford", "Stockport", "Wavertree", "Wigan" } },

             new StoreInfo { StoreName = "South A", StoreNames = new[] { "Burton", "SML", "GRMR", "Congleton", "Crewe", "Hanley", "Shrewsbury", "Stafford" } },
             new StoreInfo { StoreName = "South B", StoreNames = new[] { "Gloucester", "Nantwich", "Newport", "JSCD", "Oswestry", "Reading", "Thatcham", "Wrexham" } },

             new StoreInfo { StoreName = "North Region", StoreNames = new[] { "AMD", "Darlington", "Lincoln", "Newark", "Northwich", "SJLK", "Queensferry", "Stockton",
                                                                              "Birkenhead", "Chester", "AWG", "Llandudno", "Rhyl", "Runcorn", "Stockport", "Mobility GB" } },

             new StoreInfo { StoreName = "South Region", StoreNames = new[] { "South A", "South B" } },

            };
        }
    }
}