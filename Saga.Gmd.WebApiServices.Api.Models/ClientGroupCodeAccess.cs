using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Api.Models
{
    public class ClientGroupCodeAccess
    {
        //public GroupCode GroupCode { get; set; }
        //public GroupCode CusomerKeys { get; set; }
        public AccessControl AccessControl { get; set; }
        public bool HasAccess { get; set; }
    }
}