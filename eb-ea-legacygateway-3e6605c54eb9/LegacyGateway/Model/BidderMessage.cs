using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidderMessages")]
    public class BidderMessage
    {
        //TODO: Figure out correct contact user fields; Should response be its own thing?
        [JsonIgnore]
        public Guid MessageID { get; set; }
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "invitation_id")]
        public Guid? InvitationID { get; set; }
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
        [JsonIgnore]
        public string Body { get; set; }
        [JsonIgnore]
        public DateTime? DateSent { get; set; }
        [JsonIgnore]
        public Guid? SenderUserID { get; set; }
        [JsonIgnore]
        public bool isQuestion { get; set; }
        [JsonProperty(PropertyName = "question_number")]
        public int QuestionNum { get; set; }
        [JsonProperty(PropertyName = "send_response_to_all_bidders")]
        public bool SendToAllBidders { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }


        [JsonProperty(PropertyName = "question_id")]
        public Guid? QuestionID { get; set; }
        [JsonProperty(PropertyName = "response_id")]
        public Guid? ResponseID { get; set; }
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
        [JsonProperty(PropertyName = "response")]
        public string Response { get; set; }
        [JsonProperty(PropertyName = "date_question_sent")]
        public DateTime? QuestionSent { get; set; }
        [JsonProperty(PropertyName = "date_response_sent")]
        public DateTime? ResponseSent { get; set; }
        [JsonProperty(PropertyName = "contact")]
        public Contact Contact { get; set; }
        //[JsonProperty(PropertyName = "contact_first_name")]
        //public string ContactFirstName { get { return Contact.FirstName; } }
        //[JsonProperty(PropertyName = "contact_last_name")]
        //public string ContactLastName { get { return Contact.LastName; } }
        //[JsonProperty(PropertyName = "company")]
        //public string ContactCompany { get; set; }
    }
}
