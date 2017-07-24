using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daFilePaths")]
    public class FilePath
    {
        [JsonProperty(PropertyName = "file_id")]
        public Guid FileID { get; set; }
        [JsonIgnore]
        public int PathTypeID { get; set; }
        [JsonProperty(PropertyName = "path_type")]
        public string PathType {
            get
            {
                switch (PathTypeID)
                {
                    case 1:
                        return "Native";
                    case 2:
                        return "Pdf";
                    case 3:
                        return "JDoc";
                    case 4:
                        return "PigeonHole";
                    case 5:
                        return "Zip";
                }
                return String.Empty;
            }
        }
        [JsonProperty(PropertyName = "name")]
        public string PathName { get; set; }
        [JsonProperty(PropertyName = "size")]
        public long FileSize { get; set; }
    }
}