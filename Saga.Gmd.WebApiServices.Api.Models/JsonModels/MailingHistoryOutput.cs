using System.Collections.Generic;
using Newtonsoft.Json; 
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.Api.Models.JsonModels
{
    public class MailingHistoryOutput
    {
        public MailingHistoryOutput()
        {
            MailingHistoryList = new List<MailingHistoryResult>();
        }
        [JsonProperty("MailingHistory")]
        public dynamic MailingHistoryList { get; set; }
    }
}