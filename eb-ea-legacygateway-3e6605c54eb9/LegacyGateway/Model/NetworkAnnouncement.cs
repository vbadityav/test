using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daNetworkAnnouncements")]
    public class NetworkAnnouncement
    {
        [JsonProperty(PropertyName = "id")]
        public Guid AnnouncementID { get; set; }
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "date_posted")]
        public DateTime DatePosted { get; set; }
        [JsonProperty(PropertyName = "date_expired")]
        public DateTime DateExpired { get; set; }
        [JsonProperty(PropertyName = "priority")]
        public int PriorityLevel { get; set; }
    }
}
