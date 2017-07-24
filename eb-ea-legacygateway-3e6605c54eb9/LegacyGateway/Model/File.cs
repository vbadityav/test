using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daFiles")]
    public class File
    {
        [JsonProperty(PropertyName = "id")]
        public Guid FileID { get; set; }
        [JsonProperty(PropertyName = "object_id")]
        public Guid ObjectID { get; set; }
        [JsonProperty(PropertyName = "file_name")]
        public string FileName { get; set; }
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public Guid ParentID { get; internal set; }

        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "invitation_id")]
        public Guid? InvitationID { get; set; }
        [JsonProperty(PropertyName = "bid_id")]
        public Guid? BidID { get; set; }
    }
}
