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

   [ModelBinder(typeof(NameAndAddressModelBinder))]
    public class NameAndAddressParameter
    {
        public NameAndAddressParameter()
        {
            NameAndAddress = new NameAndAddress();
            ReturnMe = new ReturnMe(); 
            ResponseRequestedItems = new List<string>();
            AccessControl = new AccessControl();
            SuppressionOptions = new SuppressionOptions { IgnoreSuppression = true };

        }
        public NameAndAddress NameAndAddress { get; set; }
        public ReturnMe ReturnMe { get; set; }
        public SuppressionOptions SuppressionOptions { get; set; }

        [JsonIgnore]
        public List<string> ResponseRequestedItems { get; set; }
        [JsonIgnore]
        public AccessControl AccessControl { get; set; }
    }
}
