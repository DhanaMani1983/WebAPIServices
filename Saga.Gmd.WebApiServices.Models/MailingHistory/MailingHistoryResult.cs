using System;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.MailingHistory
{
    public class MailingHistoryResult
    {
        [JsonProperty(PropertyName = "MailingRef")]
        public string MailingRef { get; set; }
        [JsonProperty(PropertyName = "SelectRefNumber")]
        public string SelectRefNumber { get; set; }
        [JsonProperty(PropertyName = "MailedDate")]
        public DateTime? MailedDate { get; set; }
        [JsonProperty(PropertyName = "MailedSourceCode")]
        public string MailedSourceCode { get; set; }
        [JsonProperty(PropertyName = "LetterCode")]
        public string LetterCode { get; set; }
        [JsonProperty(PropertyName = "Channel")]
        public string Channel { get; set; }
        [JsonProperty(PropertyName = "CompanyCode")]
        public string CompanyCode { get; set; }
        [JsonProperty(PropertyName = "SelectDescription")]
        public string SelectDescription { get; set; }
        [JsonProperty(PropertyName = "ProductCode")]
        public string ProductCode { get; set; }
    }
}

