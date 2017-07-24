using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidDetails")]
    public class BidDetail
    {
        [JsonProperty(PropertyName = "id")]
        public Guid BidDetailsID { get; set; }
        [JsonProperty(PropertyName = "bid_id")]
        public Guid BidID { get; set; }
        [JsonProperty(PropertyName = "section_item_id")]
        public Guid BidItemID { get; set; }
        [JsonProperty(PropertyName = "total_cost")]
        public decimal? AdjustedTotalCost { get; set; }
        [JsonProperty(PropertyName = "unit_cost")]
        public decimal? AdjustedUnitCost { get; set; }
        [JsonProperty(PropertyName = "gpo_pricing_included")]
        public bool? GPOPricingIncluded { get; set; }
        [JsonIgnore]
        public string CustomField1Value { get; set; }
        [JsonIgnore]
        public string CustomField2Value { get; set; }
        [JsonIgnore]
        public string CustomField3Value { get; set; }
        [JsonProperty(PropertyName = "section_item_custom_field_values")]
        public List<BidDetailCustomFieldValue> CustomFields {
            get
            {
                List<BidDetailCustomFieldValue> list = new List<BidDetailCustomFieldValue>();
                if (!String.IsNullOrWhiteSpace(CustomField1Value))
                {
                    list.Add(new BidDetailCustomFieldValue() { BidDetailsID = BidDetailsID, BidID = BidID, CustomFieldID = 1, Value = CustomField1Value });
                }
                if (!String.IsNullOrWhiteSpace(CustomField2Value))
                {
                    list.Add(new BidDetailCustomFieldValue() { BidDetailsID = BidDetailsID, BidID = BidID, CustomFieldID = 2, Value = CustomField2Value });
                }
                if (!String.IsNullOrWhiteSpace(CustomField3Value))
                {
                    list.Add(new BidDetailCustomFieldValue() { BidDetailsID = BidDetailsID, BidID = BidID, CustomFieldID = 3, Value = CustomField3Value });
                }
                if(list.Count > 0)
                {
                    return list;
                }
                return null;
            }
        }
    }
}
