using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.Model
{
    [Table("daCompanyConstructionCodes")]
    class CompanyConstructionCode
    {
        [JsonProperty(PropertyName = "company_id")]
        public Guid CompanyID { get; set; }
        [JsonProperty(PropertyName = "construction_code_id")]
        public Guid ConstructionCodeID { get; set; }
        [JsonProperty(PropertyName = "date_assigned")]
        public DateTime DateAssigned { get; set; }
    }
}