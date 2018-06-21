using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class NoAccessToPermission
    {
        [JsonProperty("Permissions")]
        public string Message { get; set; }
    }
}