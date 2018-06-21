using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Permissions;
using Saga.Gmd.WebApiServices.Api.Components.Permission;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models.Gdpr;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public partial class CustomerController
    {
        [HttpPost]
        [ActionName("CustomerPermissionsLoad")]
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_PERMISSIONS_LOAD")]
        public IHttpActionResult Permissions([FromBody] PermissionsCustomerLoadModel permissions)
        {
            try
            {
                var processPermission = new ProcessPermission(null, _permissionsService, _mciRequestService, _logger, null);
                var message = processPermission.AddOrUpdateCustomerPermission(permissions);
                return Ok(message);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"PermissionsLoad: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters MembershipLoad:- Name=FirstName={permissions.CustomerNameAndAddress.FirstName}, LastName={permissions.CustomerNameAndAddress.Surname}, Dob={permissions.CustomerNameAndAddress.Dob}, " +
                        $" Address=AddresLine1={ permissions.CustomerNameAndAddress.Address.Address1 ?? ""}, AddressLine2={permissions.CustomerNameAndAddress.Address.Address2 ?? ""}, AddressLine3={permissions.CustomerNameAndAddress.Address.Address3 ?? ""}, " +
                        $" AddressLine4={permissions.CustomerNameAndAddress.Address.Address4 ?? ""}, PostCode={permissions.CustomerNameAndAddress.Address.Postcode ?? ""}");
                }
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}