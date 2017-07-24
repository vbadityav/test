using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daInvitations")]
    public class Invitation
    {
        [JsonProperty(PropertyName = "id")]
        public Guid InvitationID { get; set; }
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "key")]
        public string ITBKey { get; set; }
        [JsonIgnore]
        public int ResponseStatus { get; set; }
        [JsonProperty(PropertyName = "response_status")]
        public string Status { get { return ResponseStatus == 0 ? "No" : "Yes"; } }
        [JsonIgnore]
        public Guid ContactID { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public Contact Contact { get; set; }
    }
}
