using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.DTO
{
    public class CustomField
    {
        public Guid CustomFieldID { get; set; }
        public int ObjectType { get; set; }
        public int FieldType { get; set; }
        public string Description { get; set; }
        public string DefaultValue { get; set; }
        public string Options { get; set; }
        public int[] AppliesTo { get; set; }
    }

    public class CustomFieldData : CustomField
    {
        public bool IsCompany { get; set; }
        public bool IsContact { get; set; }
    }

}
