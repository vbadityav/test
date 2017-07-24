using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.DTO
{
    public class ebPublicBiddingField
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "custom_field_id")]
        public Guid? CustomFieldID {
            get
            {
                Guid returnValue = Guid.Empty;
                string id = System.Text.RegularExpressions.Regex.Replace(Value, "^(?:Company_)|(?:Contact_)", "");
                if (GroupName.EndsWith("Custom Fields") && Guid.TryParse(id, out returnValue))
                {
                    return returnValue;
                }
                return null;
            }
        }
        [JsonProperty(PropertyName = "display_name")]
        public string DisplayName { get; set; }
        [JsonProperty(PropertyName = "display_order")]
        public int DisplayOrder { get; set; }
        [JsonProperty(PropertyName = "is_required")]
        public bool IsRequired { get; set; }

        [JsonIgnore]
        public string GroupName { get; set; }
        [JsonIgnore]
        public bool IsAvailable { get; set; }
        [JsonIgnore]
        public bool IsDefault { get; set; }

        public bool ShouldSerializeValue()
        {
            return !GroupName.EndsWith("Custom Fields");
        }

        public bool ShouldSerializeCustomFieldID()
        {
            return GroupName.EndsWith("Custom Fields");
        }

    }

}
