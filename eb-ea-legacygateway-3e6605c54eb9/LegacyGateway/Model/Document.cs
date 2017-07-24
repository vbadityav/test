using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    public class Document
    {
        [JsonProperty(PropertyName="id")]
        public Guid ID { get; set; }
        [JsonProperty(PropertyName = "object_id")]
        public Guid? ObjectID { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public Guid? ParentID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "version")]
        public int? Version { get; set; }
        [JsonProperty(PropertyName = "document_type")]
        public string DocumentType { get; set; }
    }
}
