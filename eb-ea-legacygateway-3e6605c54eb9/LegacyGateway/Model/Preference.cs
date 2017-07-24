using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;
using LegacyGateway.DTO;

namespace LegacyGateway.Model
{
    [Table("daPreferences")]
    public class Preference
    {
        [JsonProperty(PropertyName = "id")]
        public Guid PreferenceID { get; set; }
        [JsonProperty(PropertyName = "account_id")]
        public Guid? AccountID { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public Guid? UserID { get; set; }
        [JsonProperty(PropertyName = "project_id")]
        public Guid? PortalID { get; set; }
        [JsonProperty(PropertyName = "preference_type")]
        public int Type { get; set; }
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "entity_id")]
        public Guid? EntityID { get; set; }
    }

    // Type = 12
    public class SecurityQuestions
    {
        [JsonProperty(PropertyName = "question")]
        public string Question { get; set; }
        [JsonProperty(PropertyName = "answer")]
        public string Answer { get; set; }
    }
    //Type 12 Raw
    public class ebUserResponses
    {
        public string Question1 { get; set; }
        public string Question2 { get; set; }
        public string Question3 { get; set; }
        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
    }

    public class PublicBiddingFieldWrapper
    {
        public IEnumerable<ebPublicBiddingField> company_fields { get; set; }
        public IEnumerable<ebPublicBiddingField> company_custom_fields { get; set; }
        public IEnumerable<ebPublicBiddingField> contact_fields { get; set; }
        public IEnumerable<ebPublicBiddingField> contact_custom_fields { get; set; }

        public PublicBiddingFieldWrapper()
        {

        }

        public PublicBiddingFieldWrapper(string value)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ebPublicBiddingField[]));
            ebPublicBiddingField[] fields;
            using (StringReader sr = new StringReader(value))
            {
                fields = (ebPublicBiddingField[])serializer.Deserialize(sr);
            }

            var group = from f in fields
                        group f by f.GroupName into g
                        select g;

            company_fields = group.FirstOrDefault(g => g.Key == "Company Fields");
            company_custom_fields = group.FirstOrDefault(g => g.Key == "Company Custom Fields");
            contact_fields = group.FirstOrDefault(g => g.Key == "Contact Fields");
            contact_custom_fields = group.FirstOrDefault(g => g.Key == "Contact Custom Fields");
        }
    }
}
