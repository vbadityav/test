using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daObjects")]
    public class ebObject
    {
        [JsonProperty(PropertyName="id")]
        public Guid ObjectID { get; set; }
        [JsonIgnore]
        public int ObjectType { get; set; }
        [JsonIgnore]
        public DateTime DateCreated { get; set; }
        [JsonIgnore]
        public Guid Owner { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public Guid Parent { get; set; }
        [JsonIgnore]
        public Guid Portal { get; set; }
        [JsonIgnore]
        public bool IsDeleted { get; set; }
        [JsonProperty(PropertyName = "files")]
        public List<File> Files { get; set; }
    }
}
