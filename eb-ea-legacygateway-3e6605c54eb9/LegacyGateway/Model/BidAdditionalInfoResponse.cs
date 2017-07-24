using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidAdditionalInfoResponses")]
    public class BidAdditionalInfoResponse
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AdditionalInfoResponseID { get; set; }
        [JsonProperty(PropertyName = "additional_information_id")]
        public Guid AdditionalInformationID { get; set; }
        [JsonProperty(PropertyName = "bid_id")]
        public Guid BidID { get; set; }
        [JsonProperty(PropertyName = "answer")]
        public string Response { get; set; }
    }
}
