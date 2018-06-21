using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Saga.Gmd.WebApiServices.Api.App_Start;
using Saga.Gmd.WebApiServices.Api.Components;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public partial class CustomerController
    {

        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_RETRIEVE_NAMEADDR")]
        [HttpPost]
        [ActionName("NameAndAddress")]
        [Route("api/v2/customer/NameAndAddress")]
        public IHttpActionResult PostV2(NameAndAddressParameter parameter)
        {
            try
            {
                // Use indexed DI resoloution for V2 strategy 
                IGetRequestProcess nameAndAddress = _strategyProcesses[GetRequestProcessImplementations.NameAndAddressV2];

                parameter.AccessControl =
                    _clientScopeService.VerifyUserHasAccessToGroupCode(parameter.AccessControl,
                        Scope.CUSTOMER_RETRIEVE_NAMEADDR);
                object output = nameAndAddress.Execute(parameter);
                return Ok(output);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"NameAndAddress: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters NameAndAddress:- Name=FirstName={parameter.NameAndAddress.FirstName}, LastName={parameter.NameAndAddress.Surname}, Dob={parameter.NameAndAddress.Dob}, " +
                        $" Address=AddresLine1={ parameter.NameAndAddress.Address.Address1 ?? ""}, AddressLine2={parameter.NameAndAddress.Address.Address2 ?? ""}, AddressLine3={parameter.NameAndAddress.Address.Address3 ?? ""}, " +
                        $" AddressLine4={parameter.NameAndAddress.Address.Address4 ?? ""}, PostCode={parameter.NameAndAddress.Address.Postcode ?? ""}");
                }
                //return Ok(exception);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }



        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_RETRIEVE_NAMEADDR")]
        [HttpPost]
        [ActionName("NameAndAddress")]
        public IHttpActionResult Post(NameAndAddressParameter parameter)
        {
            try
            {
                // Use indexed DI resoloution 
                IGetRequestProcess nameAndAddress = _strategyProcesses[GetRequestProcessImplementations.NameAndAddress];

                //NameAndAddressStrategy nameAndAddress = new NameAndAddressStrategy(parameter, _mailingHistoryService,
                //    _mciRequestService, _clientScopeService, _logger, _permissionsService, _membershipService,
                //    _customerMatchService, _customerDetailsService, _travelSummaryService);
                parameter.AccessControl =
                    _clientScopeService.VerifyUserHasAccessToGroupCode(parameter.AccessControl,
                        Scope.CUSTOMER_RETRIEVE_NAMEADDR);
                object output = nameAndAddress.Execute( parameter );
                return Ok(output);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"PermissionGet: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters PermissionLoad:- Name=FirstName={parameter.NameAndAddress.FirstName}, LastName={parameter.NameAndAddress.Surname}, Dob={parameter.NameAndAddress.Dob}, " +
                        $" Address=AddresLine1={ parameter.NameAndAddress.Address.Address1 ?? ""}, AddressLine2={parameter.NameAndAddress.Address.Address2 ?? ""}, AddressLine3={parameter.NameAndAddress.Address.Address3 ?? ""}, " +
                        $" AddressLine4={parameter.NameAndAddress.Address.Address4 ?? ""}, PostCode={parameter.NameAndAddress.Address.Postcode ?? ""}");
                }
                //return Ok(exception);
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}