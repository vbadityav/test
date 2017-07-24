using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daPortalTypes")]
    public class PortalType
    {
        [JsonIgnore]
        public int id
        {
            get
            {
                return PortalTypeID;
            }
            set
            {
                PortalTypeID = value;
            }
        }
        [JsonProperty(PropertyName="id")]
        public int PortalTypeID { get; set; }
        [JsonProperty(PropertyName="app_name")]
        public string DBName_App { get; set; }
    }
}
