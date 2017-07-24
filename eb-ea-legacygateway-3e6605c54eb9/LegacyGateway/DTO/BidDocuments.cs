using LegacyGateway.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.DTO
{
    public class BidDocumentsWrapper
    {
        [JsonProperty(PropertyName = "bid_documents")]
        public List<BidDocuments> BidDocuments { get; set; }
    }
    public class BidDocuments
    {
        [JsonProperty(PropertyName = "bid_package_id")]
        public Guid BidPackageID { get; set; }
        [JsonProperty(PropertyName = "documents")]
        public IEnumerable<Document> Documents { get; set; }
        [JsonProperty(PropertyName = "file_paths")]
        public IEnumerable<FilePath> FilePaths { get; set; }
    }

    public class BidDocument : Document
    {
        public Guid BidPackageID { get; set; }
    }

    public class BidFilePath: FilePath
    {
        public Guid BidPackageID { get; set; }
    }

}
