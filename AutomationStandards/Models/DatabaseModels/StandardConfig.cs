using System;
using System.Collections.Generic;

namespace AutomationStandards.Models.DatabaseModels
{
    public partial class StandardConfig
    {
        public int StandardConfigID { get; set; }
        public int StandardID { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public int DataTypeID { get; set; }
        public int SortOrder { get; set; }
        public int VersionNumber { get; set; }
        public bool UseToolTip { get; set; }
        public string ToolTip { get; set; }
        public bool UseStandardData { get; set; }
        public bool AllowMultiSelect { get; set; }
        public int StandardLUID { get; set; }
        public string StandardLUValue { get; set; }
        public bool StandardUseFilter { get; set; }
        public string StandardFilterSQL { get; set; }

        public virtual StandardDataType DataType { get; set; }
        public virtual Standard Standard { get; set; }
    }
}
