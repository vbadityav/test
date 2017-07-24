using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    [Table("daContacts")]
    public class Contact
    {
        [JsonProperty(PropertyName = "id")]
        public Guid? ContactID { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string EncryptedPassword { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public Guid? UserID { get; set; }
        [JsonProperty(PropertyName = "first_name")]
        public string FirstName { get; set; }
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
        [JsonProperty(PropertyName = "company")]
        public string CompanyName { get; set; }
        [JsonProperty(PropertyName = "company_id")]
        public Guid CompanyID { get; set; }
        
        [JsonProperty(PropertyName = "contact_type")]
        public string ContactType { get; set; }
        [JsonProperty(PropertyName = "is_active")]
        public string IsActive { get; set; }
        [JsonProperty(PropertyName = "is_deleted")]
        public string IsDeleted { get; set; }
        [JsonProperty(PropertyName = "phone")]
        public string Phone { get; set; }
        [JsonProperty(PropertyName = "fax")]
        public string Fax { get; set; }
        [JsonProperty(PropertyName = "mobile")]
        public string Mobile { get; set; }
        [JsonProperty(PropertyName = "other_phone")]
        public string OtherPhone { get; set; }
        [JsonProperty(PropertyName = "pager")]
        public string Pager { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "department")]
        public string Department { get; set; }
        [JsonProperty(PropertyName = "name_prefix")]
        public string NamePrefix { get; set; }
        [JsonProperty(PropertyName = "date_created")]
        public string DateCreated { get; set; }
        [JsonProperty(PropertyName = "created_by_id")]
        public string CreatedById { get; set; }
        [JsonProperty(PropertyName = "contact_external_id")]
        public string ContactExternalid { get; set; }
        [JsonProperty(PropertyName = "do_not_use")]
        public string DoNotUse { get; set; }

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
                    Country = Country
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

        [JsonProperty(PropertyName = "custom_fields")]
        public List<CustomField> CustomFields { get; set; }

    }
}
