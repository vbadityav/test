using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LegacyGateway.Model
{
    public class Attribute<T, K, V>
    {
        [JsonProperty(PropertyName = "section_id")]
        public T SectionID { get; set; }
        [JsonProperty(PropertyName = "key")]
        public K Key { get; set; }
        [JsonProperty(PropertyName = "value")]
        public V Value { get; set; }

        public Attribute(T section_id, K key, V value)
        {
            this.SectionID = section_id;
            this.Key = key;
            this.Value = value;
        }
    }
}
