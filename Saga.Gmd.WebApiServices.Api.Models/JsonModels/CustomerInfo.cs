using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class CustomerInfo
    {
        [JsonProperty("CustomerDetails")]
        public object CustomerDetails { get; set; }
    }
}
