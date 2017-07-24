using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.Model
{
    public class AccountAttribute
    {
        public Guid AttributeID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int DataType { get; set; }
    }
}
