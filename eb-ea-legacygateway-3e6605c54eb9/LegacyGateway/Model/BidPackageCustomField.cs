using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    public class BidPackageCustomField
    {
        [JsonProperty(PropertyName = "id")]
        public int ID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "display_order")]
        public int DisplayOrder { get; set; }

        public BidPackageCustomField() { }

        public BidPackageCustomField(string name, int displayOrder)
        {
            ID = displayOrder;
            Name = name;
            DisplayOrder = displayOrder;
        }
    }
}
