using System;
using System.Collections.Generic;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{
    public class MailingHistory
    {
        public MailingHistory()
        {
            FieldsTobeReturned = new List<string>(); 
        }
        public List<string> FieldsTobeReturned { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? FromDate { get; set; }
        public string Product { get; set; }

      
    } 
    
}


