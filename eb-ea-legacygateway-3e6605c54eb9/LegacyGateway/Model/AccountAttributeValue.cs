using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.Model
{
    [Table("daAccountAttributeValues")]
    public class AccountAttributeValue
    {
        public Guid AttributeValueID { get; set; }
        public Guid AccountID { get; set; }
        public Guid AttributeID { get; set; }
        public int? i_Value { get; set; }
        public string vc_Value { get; set; }
        public DateTime? dt_Value { get; set; }
        public Guid? uid_Value { get; set; }
    }
}
