using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class NoAccessToCusotmerKeys
    {
        [JsonProperty("CustomerKeys")]
        public string Message { get; set; }
    }
}