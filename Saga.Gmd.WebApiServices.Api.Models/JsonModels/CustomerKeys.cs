using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class Keys
    {
       
        public object KeysList { get; set; }
        public bool? HasPartialResult { get; set; }
        public string Message { get; set; }
    }

    public class CustomerKeys
    {
        public CustomerKeys()
        {
            Keys = new Keys();
        }
        [JsonProperty("CustomerKeys")]
        public Keys Keys { get; set; }
    }
}