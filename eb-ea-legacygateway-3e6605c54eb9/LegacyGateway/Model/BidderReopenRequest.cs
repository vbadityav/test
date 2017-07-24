using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.Model
{
    [Table("daBidderReopenRequests")]
    public class BidderReopenRequest
    {
        [JsonProperty(PropertyName = "id")]
        public Guid BidderReopenRequestID { get; set; }
        [JsonProperty(PropertyName = "project_id")]
        public Guid PortalID { get; set; }
        [JsonProperty(PropertyName = "invitation_id")]
        public Guid InvitationID { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public Guid UserID { get; set; }
        [JsonProperty(PropertyName = "status")]
        public string StatusName
        {
            get
            {
                switch (Status)
                {
                    case 1:
                        return "Pending";
                    case 2:
                        return "Approved";
                    case 3:
                        return "Rejected";
                }
                return "Pending";
            }
        }
        [JsonIgnore]
        public int Status { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string MessageIn { get; set; }
        [JsonProperty(PropertyName = "response")]
        public string MessageOut { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "date_completed")]
        public DateTime? DateCompleted { get; set; }
        [JsonProperty(PropertyName = "completed_by_user_id")]
        public Guid? CompletedByUserID { get; set; }
    }
}
