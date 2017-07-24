using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daCustomFieldValues")]
    public class CustomFieldValue
    {
        [JsonProperty(PropertyName="id")]
        public Guid CustomFieldValueID { get; set; }
        [JsonProperty(PropertyName="custom_field_id")]
        public Guid CustomFieldID { get; set; }
        [JsonProperty(PropertyName="entity_id")]
        public Guid EntityID { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value {
            get {
                switch (FieldType)
                {
                    case 1:
                    case 3:
                    case 7:
                        return vc_value;
                    case 2:
                        return text_value;
                    case 4:
                        return i_value.HasValue ? i_value.ToString() : null;
                    case 6:
                        return dt_value.HasValue ? dt_value.Value.ToString("MM.dd.yyyy") : null;
                }
                return "";
            }
        }

        [JsonProperty(PropertyName = "values")]
        public string[] Values
        {
            get
            {
                if (FieldType == 18 && !String.IsNullOrWhiteSpace(vc_value) && !String.IsNullOrWhiteSpace(vc_value.Trim('|')))
                {
                    return vc_value.Trim('|').Split('|');
                }
                return null;
            }
        }

        [JsonProperty(PropertyName = "file_id")]
        public Guid? FileID
        {
            get
            {
                return FieldType == 9 ? u_value : null;
            }
            set
            {
                u_value = value;
            }
        }

        [JsonIgnore]
        public int? i_value { get; set; }
        [JsonIgnore]
        public string vc_value { get; set; }
        [JsonIgnore]
        public string text_value { get; set; }
        [JsonIgnore]
        public DateTime? dt_value { get; set; }
        [JsonIgnore]
        public Guid? u_value { get; set; }
        [JsonIgnore]
        public int FieldType { get; internal set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}
