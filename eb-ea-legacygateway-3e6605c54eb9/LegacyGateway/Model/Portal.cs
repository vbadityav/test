using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daPortals")]
    public class Portal
    {
        [JsonProperty(PropertyName="id")]
        public Guid PortalID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "file_store_path")]
        public string FileStorePath { get; set; }
        [JsonIgnore]
        public Guid AccountID { get; set; }
        [JsonProperty(PropertyName = "account")]
        public BidPackageAccountData Account { get; set; }
    }
}
