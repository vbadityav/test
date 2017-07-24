using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidAdditionalInfoItems")]
    public class BidAdditionalInfoItem
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AdditionalInformationID { get; set; }
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
        [JsonProperty(PropertyName = "required")]
        public bool isRequired { get; set; }
        [JsonProperty(PropertyName = "is_bidder_entry")]
        public bool isBidderEntry { get; set; }
        [JsonProperty(PropertyName = "display_order")]
        public int DisplayOrder { get; set; }
    }
}
