using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daBidAddenda")]
    public class BidAddenda
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AddendumID { get; set; }
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string AddendumNumber { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "date_documentation_available")]
        public DateTime DocumentationDateTime { get; set; }
        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }
    }
}
