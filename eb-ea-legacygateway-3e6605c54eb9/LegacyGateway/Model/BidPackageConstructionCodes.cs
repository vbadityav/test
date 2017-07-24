using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidPackageConstructionCodes")]
    public class BidPackageConstructionCode
    {
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonIgnore]
        public Guid ConstructionCodeID { get; set; }
        [JsonProperty(PropertyName = "code")]
        public ConstructCode Code { get; set; }
    }
}
