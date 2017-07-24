using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    public class BidDetailCustomFieldValue
    {
        [JsonProperty(PropertyName = "bid_id")]
        public Guid BidID { get; set; }
        [JsonProperty(PropertyName = "section_item_value_id")]
        public Guid BidDetailsID { get; set; }
        [JsonProperty(PropertyName = "section_item_custom_field_id")]
        public int CustomFieldID { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
