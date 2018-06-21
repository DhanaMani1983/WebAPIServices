using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class MailingHistory
    {
        [JsonProperty("MailingHistory")]
        public object MailingHistoryList { get; set; }
    }
}