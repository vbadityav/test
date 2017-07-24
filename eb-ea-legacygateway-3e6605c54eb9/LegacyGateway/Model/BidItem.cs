using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidItems")]
    public class BidItem
    {
        [JsonProperty(PropertyName = "id")]
        public Guid BidItemID { get; set; }
        [JsonProperty(PropertyName = "section_id")]
        public Guid BidSectionID { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string ItemNumber { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "details")]
        public string LongDescription { get; set; }
        [JsonProperty(PropertyName = "spec_reference")]
        public string SpecReference { get; set; }
        [JsonProperty(PropertyName = "part_number")]
        public string PartNumber { get; set; }
        [JsonProperty(PropertyName = "quantity")]
        public decimal? Quantity { get; set; }
        [JsonProperty(PropertyName = "unit_of_measure")]
        public string UnitOfMeasure { get; set; }
        [JsonProperty(PropertyName = "unit_cost")]
        public decimal? EstimatedUnitCost { get; set; }
        [JsonProperty(PropertyName = "total_cost")]
        public decimal? EstimatedTotalCost { get; set; }
        [JsonProperty(PropertyName = "display_order")]
        public int DisplayOrder { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "created_by_id")]
        public Guid CreatedByID { get; set; }
        [JsonProperty(PropertyName = "last_modified_by_id")]
        public Guid LastModifiedByID { get; set; }
        [JsonProperty(PropertyName = "date_last_modified")]
        public DateTime LastModifiedDate { get; set; }
        [JsonProperty(PropertyName = "hide_from_bidder")]
        public bool HideFromBidder { get; set; }

        [JsonProperty(PropertyName = "construction_code_id")]
        public Guid ConstructionCodeID { get; set; }
        [JsonProperty(PropertyName = "budget_line_item_id")]
        public Guid BudgetLineItemID { get; set; }

        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
    }
}
