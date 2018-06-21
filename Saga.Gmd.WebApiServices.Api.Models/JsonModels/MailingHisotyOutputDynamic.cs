using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class MailingHisotyOutputDynamic
    {
        public MailingHisotyOutputDynamic()
        {
            MailingHistoryExpando = new List<ExpandoObject>();
        }
        [JsonProperty("MailingHistory")]
        public List<ExpandoObject> MailingHistoryExpando { get; set; }
    }

    public class NoAccessToMailingHistory
    {
        [JsonProperty("MailingHistory")]
        public string Message { get; set; }
    }

    public class NoAccessToCustomerDetails
    {
        [JsonProperty("CustomerDetails")]
        public string Message { get; set; }
    }
}