using System.Collections.Generic;

namespace BG_Menu.Class.Functions
{
    /// <summary>
    /// Represents a supplier with its associated properties.
    /// </summary>
    public class Supplier
    {
        public int SupplierID { get; set; } // Primary Key
        public string Name { get; set; } // Supplier Name (e.g., "Amazon")
        public string GLAccount { get; set; }
        public string GLName { get; set; } // GL Account Name
        public string VATCode { get; set; } // VAT Code (e.g., "I1", "I2")
        public string DescriptionPattern { get; set; } // Pattern for Description if needed
        public string FilenamePattern { get; set; } // Pattern for Filename
        public List<string> DetectionKeywords { get; set; } // List of Detection Keywords
        public List<string> TotalExtractionRegexes { get; set; }

        public Supplier()
        {
            DetectionKeywords = new List<string>();
        }
    }
}
