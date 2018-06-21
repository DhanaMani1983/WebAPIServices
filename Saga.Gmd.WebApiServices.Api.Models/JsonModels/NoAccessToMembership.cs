using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class NoAccessToMembership
    {
        [JsonProperty("Membership")]
        public string Message { get; set; }
    }
}