using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.ModelBinding;
using FluentValidation.Attributes;
using Newtonsoft.Json.Serialization;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.ReturnMe;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Api.Models.ModelBinder;
using Saga.Gmd.WebApiServices.Api.Models.Validators;
using Saga.Gmd.WebApiServices.Models.Gdpr;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models.ParameterModels
{
    [Validator(typeof(PermissionParameterValidator))]
    public class PermissionParameter
    {
        public string Title { get; set; }
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Product { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
        public string  Brand { get; set; }

        public PermissionsCustomerLoad Permissions { get; set; }

        [JsonProperty("ReturnMe")]
        public ReturnMePermissions ReturnMe { get; set; }

        [JsonIgnore]
        public AccessControl AccessControl { get; set; }
    }
}
