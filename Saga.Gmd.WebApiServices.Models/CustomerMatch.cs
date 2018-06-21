using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.Models
{
    public class CustomerMatch
    {
        public string MatchType { get; set; }
    }

    public class TravelSummary
    {
        public bool Required { get; set; }
    }

    public class CustomerMatchDetails
    {
        public CustomerMatchDetails()
        {
            CustomerMatchInfoDetailes = new List<CustomerMatchInfoDetails>();
        }
        [JsonProperty("CustomerMatches")]
        public List<CustomerMatchInfoDetails> CustomerMatchInfoDetailes { get; set; }

        [JsonIgnore]
        public int MatchCount => this.CustomerMatchInfoDetailes.Count; 
        
    }
}
