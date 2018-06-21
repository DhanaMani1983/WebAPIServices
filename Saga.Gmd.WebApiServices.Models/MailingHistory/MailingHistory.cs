using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Saga.Gmd.WebApiServices.Models.MailingHistory
{
    public class MailingHistory
    {

        public bool? Required { get; set; }
        public string Product { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public List<string> FieldsTobeReturned { get; set; }  
        public MailingHistory()
        {
            FieldsTobeReturned = new List<string>();
            
        } 
        [JsonIgnore]
        public string MatchType { get; set; }
    }
}
