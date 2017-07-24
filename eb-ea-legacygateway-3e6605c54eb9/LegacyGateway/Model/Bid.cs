using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBids")]
    public class Bid
    {
        [JsonProperty(PropertyName = "id")]
        public Guid BidID { get; set; }
        [JsonProperty(PropertyName = "invitation_id")]
        public Guid InvitationID { get; set; }
        [JsonProperty(PropertyName = "qualification")]
        public string BidQualifications { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string BidStatus {
            get{
                switch (Status)
                {
                    case 1:
                        return "Draft";
                    case 2:
                        return "Submitted";
                }
                return "";
            }
        }
        [JsonProperty(PropertyName = "date_submitted")]
        public DateTime? DateSubmitted { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "created_by_id")]
        public Guid CreatedByID { get; set; }
        [JsonProperty(PropertyName = "date_last_modified")]
        public DateTime? LastModifiedDate { get; set; }
        [JsonProperty(PropertyName = "last_modified_by_id")]
        public Guid? LastModifiedBy { get; set; }

        [JsonIgnore]
        public int Status { get; set; }
    }
}
