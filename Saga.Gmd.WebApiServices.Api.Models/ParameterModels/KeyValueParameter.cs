using System.Collections.Generic;
using System.Web.Http.ModelBinding;
using FluentValidation.Attributes;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.ReturnMe;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{

    [Validator(typeof(KeyValueParameterValidator))]
    [ModelBinder(typeof(KeyValueModelBinder))]
    public class KeyValueParameter
    {
        public KeyValueParameter()
        {
            KeyValue = new KeyValue(); 
            ReturnMe = new ReturnMeForKeyValuePair();
            ResponseRequestedItems = new List<string>();
            AccessControl = new AccessControl();
            SuppressionOptions = new SuppressionOptions { IgnoreSuppression = true };

        }
        public KeyValue KeyValue { get; set; }
        public ReturnMeForKeyValuePair ReturnMe { get; set; }
        public SuppressionOptions SuppressionOptions { get; set; }

        [JsonIgnore]
        public string Version { get; set; }

        [JsonIgnore]
        public List<string> ResponseRequestedItems { get; set; }
        [JsonIgnore]
        public AccessControl AccessControl { get; set; }


    }
}
