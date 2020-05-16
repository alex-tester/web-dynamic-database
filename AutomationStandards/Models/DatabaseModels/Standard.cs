using System;
using System.Collections.Generic;

namespace AutomationStandards.Models.DatabaseModels
{
    public partial class Standard
    {
        public Standard()
        {
            StandardConfig = new HashSet<StandardConfig>();
        }

        public int StandardID { get; set; }
        public int StandardGroupID { get; set; }
        public string DBTableName { get; set; }
        public string StandardName { get; set; }
        public string StandardDefinition { get; set; }
        public string ManageRoles { get; set; }
        public int VersionConfig { get; set; }
        public int VersionValue { get; set; }
        public string Tags { get; set; }
        public bool NotifiyOwner { get; set; }
        public string OwnerEmail { get; set; }
        public string ViewerRoles { get; set; }
        public int UsageCount { get; set; }

        public virtual StandardGroup StandardGroup { get; set; }
        public virtual ICollection<StandardConfig> StandardConfig { get; set; }
    }
}
