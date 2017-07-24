using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace LegacyGateway.Model
{
    [Table("daCustomFieldDependencies")]
    public class CustomFieldDependency
    {
        [JsonProperty(PropertyName = "id")]
        public Guid CustomFieldDependencyID { get; set; }
        [JsonProperty(PropertyName = "controlling_custom_field_id")]
        public Guid ControllingFieldID { get; set; }
        [JsonProperty(PropertyName = "dependent_custom_field_id")]
        public Guid DependentFieldID { get; set; }
        [JsonIgnore]
        public string Value { get; set; }
        [JsonProperty(PropertyName = "mappings")]
        public List<Dependency> Dependencies {
            get
            {
                List<Dependency> dependencies = new List<Dependency>();
                XDocument doc = XDocument.Parse(Value);

                foreach (XElement element in doc.Root.Element("Values").Elements("Value"))
                {
                    dependencies.Add(new Dependency(element));
                }

                return dependencies;
            }
        }

    }
    public class Dependency
    {
        public string controlling_value { get; set; }
        public string[] dependent_values { get; set; }
       
        public Dependency(XElement element)
        {
            controlling_value = element.Attribute("Text").Value;
            dependent_values = element.Elements("DependentValue").Attributes("Text").Select(dv => dv.Value).ToArray();
        }
    }
}
