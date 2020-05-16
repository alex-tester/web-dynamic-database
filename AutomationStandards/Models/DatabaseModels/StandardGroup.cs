using System;
using System.Collections.Generic;

namespace AutomationStandards.Models.DatabaseModels
{
    public partial class StandardGroup
    {
        public StandardGroup()
        {
            Standard = new HashSet<Standard>();
        }

        public int StandardGroupID { get; set; }
        public string StandardGroupName { get; set; }

        public virtual ICollection<Standard> Standard { get; set; }
    }
}
