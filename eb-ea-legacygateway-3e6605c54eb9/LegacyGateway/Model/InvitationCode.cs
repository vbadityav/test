using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daInvitationCodes")]
    public class InvitationCode
    {
        [JsonProperty(PropertyName = "id")]
        public Guid InvitationCodeID { get; set; }
        [JsonProperty(PropertyName = "code_id")]
        public Guid ConstructionCodeID { get; set; }
        [JsonProperty(PropertyName = "invitation_id")]
        public Guid InvitationID { get; set; }
        [JsonIgnore]
        public int BiddingStatus { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string Status {
            get
            {
                switch (BiddingStatus)
                {
                    case 0:
                        return "Pending";
                    case 1:
                        return "WillBid";
                    case 2:
                        return "WillNotBid";
                    case 3:
                        return "Awarded";
                }
                return null;
            }
        }
        [JsonProperty(PropertyName = "awarded")]
        public bool isAwarded { get; set; }
        [JsonProperty(PropertyName = "revoked")]
        public bool isRevoked { get; set; }
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid? BidPackageID { get; set; }
    }
}
