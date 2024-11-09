public static class StoreLocationManager
{
    // Dictionary to store machine name, location, and store type
    public static Dictionary<string, (string Location, string StoreType)> StoreLocations = new Dictionary<string, (string Location, string StoreType)>
        {
            // UK 
            { "ABL0031", ("Birkenhead - Till 1", "UK Store") },
            { "ABL0088", ("Birkenhead - Till 2", "UK Store") },
            { "ABL0032", ("Burton - Till 1", "UK Store") },
            { "ABL0087", ("Burton - Till 2", "UK Store") },
            { "ABL0089", ("Cheltenham - Till 1", "UK Store") },
            { "ABLL0110", ("Cheltenham - Queuebuster 1", "UK Store") },
            { "ABL0033", ("Chester - Till 1", "UK Store") },
            { "ABL0034", ("Congleton - Till 1", "UK Store") },
            { "ABL0035", ("Crewe - Till 1", "UK Store") },
            { "ABL0074", ("Darlington - Till 1", "UK Store") },
            { "ABL0066", ("Gloucester - Till 1", "UK Store") },
            { "ABL0036", ("Hanley - Till 1", "UK Store") },
            { "ABL0071", ("Hanley - Till 2", "UK Store") },
            { "ABL0077", ("Lincoln - Till 1", "UK Store") },
            { "ABL0044", ("Llandudno - Till 1", "UK Store") },
            { "ABL0037", ("Nantwich - Till 1", "UK Store") },
            { "ABL0078", ("Newark - Till 1", "UK Store") },
            { "ABL0038", ("Newport - Till 1", "UK Store") },
            { "ABL0039", ("Northwich - Till 1", "UK Store") },
            { "ABL0064", ("Oswestry - Till 1", "UK Store") },
            { "ABL0053", ("Queensferry - Till 1", "UK Store") },
            { "ABL0065", ("Reading - Till 1", "UK Store") },
            { "ABL0045", ("Rhyl - Till 1", "UK Store") },
            { "ABL0068", ("Runcorn - Till 1", "UK Store") },
            { "ABL0041", ("Shrewsbury - Till 1", "UK Store") },
            { "ABL0070", ("Shrewsbury - Till 2", "UK Store") },
            { "ABL0042", ("Stafford - Till 1", "UK Store") },
            { "ABL0067", ("Stafford - Till 2", "UK Store") },
            { "ABL0043", ("Stockport - Till 1", "UK Store") },
            { "ABL0075", ("Stockton - Till 1", "UK Store") },
            { "ABL0082", ("Thatcham - Till 1", "UK Store") },
            { "ABL0046", ("Wrexham - Till 1", "UK Store") },
            { "ABL0079", ("Wrexham - Till 2", "UK Store") },        
            

            // Franchise
            { "ABLF0001", ("Paisley - Till 1", "Franchise Store") },
            { "ABLF0002", ("Paisley - Workshop", "Franchise Store") },

            { "ABLF0003", ("Broxburn - Till 1", "Franchise Store") },

            { "ABLF0004", ("Southampton - Till 1", "Franchise Store") },
            { "ABLF0005", ("Southampton - Till 2", "Franchise Store") },
            { "ABLF0006", ("Southampton - Workshop", "Franchise Store") },
            { "ABLF0007", ("Christchurch - Till 1", "Franchise Store") },

            { "ABLF0008", ("Bridgend - Till 1", "Franchise Store") },
            { "ABLF0009", ("Bridgend - Workshop", "Franchise Store") },
            { "ABLF0010", ("Cardiff - Till 1", "Franchise Store") },
            { "ABLF0020", ("Newport Wales - Till 1", "Franchise Store") },

            { "ABLF0011", ("Colchester - Till 1", "Franchise Store") },

            { "ABLF0012", ("St Helens - Till 1", "Franchise Store") },
            { "ABLF0013", ("St Helens - Workshop", "Franchise Store") },
            { "ABLF0014", ("Wavertree - Till 1", "Franchise Store") },
            { "ABLF0015", ("Hyde - Till 1", "Franchise Store") },
            { "ABLF0016", ("Blackpool - Till 1", "Franchise Store") },
            { "ABLF0017", ("Wigan - Till 1", "Franchise Store") },
            { "ABLF0018", ("Southport - Till 1", "Franchise Store") },
            { "ABLF0019", ("Salford - Till 1", "Franchise Store") },
            
            { "ABLF0021", ("Leeds - Till 1", "Franchise Store") },

        };

    public static string GetLocationByMachineName(string machineName)
    {
        if (StoreLocations.TryGetValue(machineName, out var storeInfo))
        {
            return storeInfo.Location;
        }
        return "Unknown Location"; // Default value if machine name is not found
    }

    public static string GetStoreTypeByMachineName(string machineName)
    {
        if (StoreLocations.TryGetValue(machineName, out var storeInfo))
        {
            return storeInfo.StoreType;
        }
        return "Unknown Store Type"; // Default value if machine name is not found
    }        
}
