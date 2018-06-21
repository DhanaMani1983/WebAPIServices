using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Permissions
{
    public class Channel
    {
        public Channel()
        {
            Type = string.Empty;
            Value = string.Empty;
            Divisions = new List<Division>();
        }

        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName="Value")]
        public string Value { get; set; }

        [JsonProperty(PropertyName = "Divisions")]
        public List<Division> Divisions { get; set; } 
    }
}
