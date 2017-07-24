using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyGateway.Api;
using TableDependency.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace LegacyGateway.Model
{
    [Table("daAccountPortals")]
    public class AccountPortal
    {
        public Guid AccountID { get; set; }
        public Guid PortalID { get; set; }
    }
}
