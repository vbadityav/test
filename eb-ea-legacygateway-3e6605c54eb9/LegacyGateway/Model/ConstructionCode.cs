using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daConstructionCodes")]
    public class ConstructCode
    {
        [JsonProperty(PropertyName = "id")]
        public Guid ConstructionCodeID { get; set; }
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public Guid? ParentConstructionCodeID { get; set; }
        [JsonProperty(PropertyName = "parent_code")]
        public string ParentCode { get; set; }
        [JsonProperty(PropertyName = "parent_description")]
        public string ParentDescription { get; set; }
    }
}
