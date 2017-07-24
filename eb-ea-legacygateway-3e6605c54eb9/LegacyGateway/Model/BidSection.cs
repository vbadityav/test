using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyGateway.Api;
using System.ComponentModel.DataAnnotations.Schema;
using TableDependency.Enums;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidSections")]
    public class BidSection
    {
        [JsonProperty(PropertyName = "id")]
        public Guid BidSectionID { get; set; }
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string SectionName { get; set; }
        [JsonIgnore]
        public int SectionType { get; set; }
        [JsonProperty(PropertyName = "section_type")]
        public string SectionTypeInfo {
            get
            {
                return SectionType == 1 ? "Base" : "Alternate";
            }
        }
        [JsonIgnore]
        public bool IncludeCostPerSqFt { get; set; }
        [JsonIgnore]
        public bool IncludePercentOfBaseBid { get; set; }
        [JsonIgnore]
        public bool IncludePercentOfSection { get; set; }
        [JsonProperty(PropertyName = "attributes")]
        public List<Attribute<Guid,string, bool>> Attributes {
            get
            {
                return new List<Attribute<Guid,string, bool>>() {
                    new Attribute<Guid, string, bool>(BidSectionID, "include_cost_per_sqft", IncludeCostPerSqFt),
                    new Attribute<Guid, string, bool>(BidSectionID, "include_percent_of_base_bid", IncludePercentOfBaseBid),
                    new Attribute<Guid, string, bool>(BidSectionID, "include_percent_of_section", IncludePercentOfSection)
                };
            }
        }
        [JsonProperty(PropertyName = "display_order")]
        public int? DisplayOrder { get; set; }
    }
}
