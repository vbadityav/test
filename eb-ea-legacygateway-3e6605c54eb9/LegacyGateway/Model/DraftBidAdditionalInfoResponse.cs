using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daDraftBidAdditionalInfoResponses")]
    public class DraftBidAdditionalInfoResponse
    {
        [JsonProperty(PropertyName = "id")]
        public Guid DraftAdditionalInfoResponseID { get; set; }
        [JsonProperty(PropertyName = "additional_information_id")]
        public Guid AdditionalInformationID { get; set; }
        [JsonProperty(PropertyName = "bid_id")]
        public Guid BidID { get; set; }
        [JsonProperty(PropertyName = "answer")]
        public string Response { get; set; }
    }
}
