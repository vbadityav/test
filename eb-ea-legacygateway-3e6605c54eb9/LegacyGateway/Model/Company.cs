using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyGateway.Model
{
    [Table("daCompanies")]
    public class Company
    {
        [JsonProperty(PropertyName = "id")]
        public Guid CompanyID { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public Guid AccountID { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string CompanyName { get; set; }
        [JsonProperty(PropertyName = "is_active")]
        public bool IsActive { get; set; }
        [JsonProperty(PropertyName = "is_deleted")]
        public bool IsDeleted { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "website_url")]
        public string URL { get; set; }
        [JsonProperty(PropertyName = "construction_code_display")]
        public string ConstructionCodedisplay { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public DateTime DateCreated { get; set; }
        [JsonProperty(PropertyName = "created_by_id")]
        public Guid CreatedById { get; set; }
        [JsonProperty(PropertyName = "company_external_id")]
        public string CompanyExternalId { get; set; }
        [JsonProperty(PropertyName = "csi_codes")]
        public string CSICodes { get; set; }
        [JsonProperty(PropertyName = "number")]
        public string CompanyNumber { get; set; }
        [JsonProperty(PropertyName = "is_prequalified")]
        public bool IsPreQualified { get; set; }
        [JsonProperty(PropertyName = "do_not_use")]
        public bool DoNotUse { get; set; }

        public Address address
        {
            get
            {
                return new Address()
                {
                    Address1 = Address1,
                    Address2 = Address2,
                    City = City,
                    State = State,
                    Zip = Zip,
                    Country = Country,
                    County = County
                };
            }
        }

        [JsonIgnore]
        public string Address1 { get; set; }
        [JsonIgnore]
        public string Address2 { get; set; }
        [JsonIgnore]
        public string City { get; set; }
        [JsonIgnore]
        public string State { get; set; }
        [JsonIgnore]
        public string Zip { get; set; }
        [JsonIgnore]
        public string Country { get; set; }
        [JsonIgnore]
        public string County { get; set; }
        
        public Classification classfication {
            get
            {
                return new Classification()
                {
                    WBE = WBE,
                    MBE = MBE,
                    DBE = DBE,
                    VBE = VBE,
                    SBE = SBE,
                    Other = Other
                };
            }
        }

        [JsonIgnore]
        public bool WBE { get; set; }
        [JsonIgnore]
        public bool MBE { get; set; }
        [JsonIgnore]
        public bool DBE { get; set; }
        [JsonIgnore]
        public bool VBE { get; set; }
        [JsonIgnore]
        public bool SBE { get; set; }
        [JsonIgnore]
        public bool Other { get; set; }

        [JsonProperty(PropertyName = "custom_fields")]
        public List<CustomField> CustomFields { get; set; }
    }

    public class Address
    {
        [JsonProperty(PropertyName = "line_1")]
        public string Address1 { get; set; }
        [JsonProperty(PropertyName = "line_2")]
        public string Address2 { get; set; }
        [JsonProperty(PropertyName = "city")]
        public string City { get; set; }
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }
        [JsonProperty(PropertyName = "zip")]
        public string Zip { get; set; }
        [JsonProperty(PropertyName = "country")]
        public string Country { get; set; }
        [JsonProperty(PropertyName = "county")]
        public string County { get; set; }
    }

    public class Classification
    {
        [JsonProperty(PropertyName = "wbe")]
        public bool WBE { get; set; }
        [JsonProperty(PropertyName = "mbe")]
        public bool MBE { get; set; }
        [JsonProperty(PropertyName = "dbe")]
        public bool DBE { get; set; }
        [JsonProperty(PropertyName = "vbe")]
        public bool VBE { get; set; }
        [JsonProperty(PropertyName = "sbe")]
        public bool SBE { get; set; }
        [JsonProperty(PropertyName = "other")]
        public bool Other { get; set; }
    }
}
