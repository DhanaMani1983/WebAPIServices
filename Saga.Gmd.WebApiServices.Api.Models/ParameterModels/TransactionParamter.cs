using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.Transaction;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{

    /*
     * Note: if any one have doubt why this duplicate model, reason is I wanted to apply model binder as attribute and keep the model Saga.Gmd.WebApiServices.
     * Models project free from http.web comonent reference and also don't want to apply model binding to the paramter in Action paramter  itself.
     */
    [ModelBinder(typeof(TransactionModelbinder))]
    [Validator(typeof(TransactCustomerValidator))]
    public class TransactionParamter
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
