using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daCustomFields")]
    public class CustomField
    {
        [JsonProperty(PropertyName="id")]
        public Guid CustomFieldID { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public Guid? AccountID { get; set; }
        [JsonProperty(PropertyName="name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName="description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "field_type")]
        public string FieldTypeString {
            get
            {
                switch (FieldType)
                {
                    case 1:
                        return "String";
                    case 2:
                        return "Text";
                    case 3:
                        return "DropDownList";
                    case 4:
                        return "Integer";
                    case 6:
                        return "Date";
                    case 7:
                        return "Decimal";
                    case 9:
                        return "MultiplePickList";
                    case 18:
                        return "File";
                }
                return "String";
            }
        }
        [JsonIgnore]
        public int FieldType { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonIgnore]
        public int ObjectType { get; set; }
        [JsonProperty(PropertyName = "display_order")]
        public int DisplayOrder { get; set; }
        [JsonIgnore]
        public string DefaultValue { get; set; }
        public string default_value
        {
            get
            {
                if(FieldType == 3 && DefaultValue != null)
                {
                    string value = DefaultValue.Trim('|');

                    return value == string.Empty ? null : value;
                }
                return DefaultValue;
            }
        }
        [JsonProperty(PropertyName = "default_values")]
        public string[] MPLDefaultValue {
            get
            {
                if(FieldType == 9 && DefaultValue != null && DefaultValue.Trim('|') != String.Empty)
                {
                    return DefaultValue.Trim('|').Split('|');
                }
                return null;
            }
        }
        [JsonProperty(PropertyName = "required")]
        public bool Required { get; set; }
        [JsonProperty(PropertyName = "min_length")]
        public int? MinLength { get; set; }
        [JsonProperty(PropertyName = "max_length")]
        public int? MaxLength { get; set; }
        [JsonIgnore]
        public bool? MustBeUnique { get; set; }
        [JsonProperty(PropertyName = "unique")]
        public bool? must_be_unique
        {
            get
            {
                return FieldType == 1 ? MustBeUnique : null;
            }
        }
        [JsonProperty(PropertyName = "number_visible_rows")]
        public int? NumVisibleRows { get; set; }
        [JsonProperty(PropertyName = "decimal_places")]
        public int? DecimalPlaces { get; set; }
        [JsonIgnore]
        public string Options { get; set; }
        [JsonProperty(PropertyName = "options")]
        public string[] option_array
        {
            get
            {
                if (String.IsNullOrEmpty(Options) || FieldType == 18)
                {
                    return null;
                }
                return Options.Split('|');
            }
        }

        public string default_path
        {
            get
            {
                return FieldType == 18 ? Options : null;
            }
        }
        [JsonIgnore]
        public DateTime LastModifiedDate { get; set; }
        public bool ShouldSerializeDecimalPlaces()
        {
            return FieldType == 7;
        }

        public bool ShouldSerializedefault_value()
        {
            return FieldType != 9 && FieldType != 18;
        }

    }
}
