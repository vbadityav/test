using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daFolders")]
    public class Folder
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? FolderID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string FolderName { get; set; }
        [JsonProperty(PropertyName = "parent_id")]
        public Guid? ParentID { get; set; }
    }
}
