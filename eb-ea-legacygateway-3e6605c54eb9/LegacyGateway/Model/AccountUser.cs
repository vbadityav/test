using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.Model
{
    [Table("daAccountUsers")]
    public class AccountUser
    {
        [JsonProperty(PropertyName = "account_id")]
        public Guid AccountID { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserID { get; set; }
    }
}
