using System;
using System.Collections.Generic;

namespace AutomationStandards.Models.DatabaseModels
{
    public partial class StandardDataType
    {
        public StandardDataType()
        {
            StandardConfig = new HashSet<StandardConfig>();
        }

        public int DataTypeID { get; set; }
        public string DataTypeName { get; set; }
        public string SQLDataType { get; set; }

        public virtual ICollection<StandardConfig> StandardConfig { get; set; }
    }
}
