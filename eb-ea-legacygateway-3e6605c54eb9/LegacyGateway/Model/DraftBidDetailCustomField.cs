using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    public class DraftBidDetailCustomFieldValue
    {
        [JsonProperty(PropertyName = "section_item_value_id")]
        public Guid DraftBidDetailsID { get; set; }
        [JsonProperty(PropertyName = "section_item_custom_field_id")]
        public int CustomFieldID { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
    }
}
