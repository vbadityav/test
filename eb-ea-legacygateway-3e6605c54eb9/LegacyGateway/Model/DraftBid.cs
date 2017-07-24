using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daDraftBids")]
    public class DraftBid
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DraftBidID { get; set; }
        [JsonProperty(PropertyName = "qualification")]
        public string BidQualifications { get; set; }
        [JsonProperty(PropertyName = "invitation_id")]
        public Guid InvitationID { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string BidStatus { get { return "Draft"; } }
        [JsonProperty(PropertyName = "date_submitted")]
        public DateTime? DateSubmitted { get; set; }
    }
}
