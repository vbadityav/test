using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Utilities
{
    public class ApiResponse
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
    }
}
