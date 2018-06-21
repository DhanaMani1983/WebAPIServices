using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
 

namespace Saga.Gmd.WebApiServices.Models.Transaction
{
    

    public  class TransactionParamter
    { 
        [JsonIgnore]
        public string RequestType { get; set; }
        //[JsonIgnore] 
        // I have removed the JsonIgnore because DataSet gets mapped to Product_Type in Quantum so if DataSet is not assigned 
        // the landed rows cannot be processed and fail with '-Unrecognised Product Type'
        public string DataSet { get; set; }
        public string FeederSystem { get; set; }
        public string SourceSystem { get; set; }
        public string TransactionBrand { get; set; }
        public string SeqNo { get; set; }
        public string MarketingSource { get; set; }
        public TransactionCustomerLoad Customer { get; set; }
        public object Transaction { get; set; }
    }
}
