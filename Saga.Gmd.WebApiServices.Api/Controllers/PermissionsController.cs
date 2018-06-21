
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Components;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;
using Saga.Gmd.WebApiServices.BLL.Transaction;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;
using System.Web.Http.Description;
using Autofac.Features.Indexed;
using AutoMapper;
using Saga.Gmd.WebApiServices.Api.App_Start;
using Saga.Gmd.WebApiServices.Api.Components.Permission;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.ReturnMe;
using MatchType = Saga.Gmd.WebApiServices.Common.MatchType;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public class PermissionsController : ApiController
    {
        private readonly IMailingHistoryService _mailingHistoryService;
        private readonly IMciRequestService _mciRequestService;
        private readonly IClientScopeService _clientScopeService;
        private readonly ILog _logger;
        private readonly IPermissionService _permissionsService;
        private readonly IMembershipService _membershipService;
        private readonly ICustomerDetailsService _customerDetailsService;
        private readonly ITransactionServices _transactionServices;
        private readonly ICustomerMatchService _customerMatchService;
        private readonly ITravelSummaryService _travelSummaryService;
        private readonly bool _logParameterValue;
        private readonly IIndex<GetRequestProcessImplementations, IGetRequestProcess> _strategyProcesses;

        public PermissionsController(
            IMailingHistoryService mailingHistoryService,
            IMciRequestService mciRequestService,
            IClientScopeService clientScopeService,
            ILog logger,
            IPermissionService permissionService,
            IMembershipService membershipService,
            ICustomerDetailsService customerDetailsService,
            ITransactionServices transactionServices,
            ICustomerMatchService customerMatchService, 
            ITravelSummaryService travelSummaryService,
            IIndex<GetRequestProcessImplementations, IGetRequestProcess> strategyProcesses
            )
        {
            _mailingHistoryService = mailingHistoryService;
            _mciRequestService = mciRequestService;
            _clientScopeService = clientScopeService;
            _logger = logger;
            _permissionsService = permissionService;
            _membershipService = membershipService;
            _customerDetailsService = customerDetailsService;
            _transactionServices = transactionServices;
            _customerMatchService = customerMatchService;
            _travelSummaryService = travelSummaryService;
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);
            _strategyProcesses = strategyProcesses;

        }

        /// <summary>
        /// Response to HTTP GET with information matching the supplied identifier
        /// </summary>
        /// <param name="getParameters"></param>
        /// <returns></returns>
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_PERMISSIONS_RETRIEVE_KVPAIR")]
        [HttpGet]
        [ActionName("KeyValuePair")]
        public IHttpActionResult Get(KeyValueParameter getParameters)
        {
            try
            {
                // Use indexed DI resoloution  to get the required strategy 
                IGetRequestProcess strategy = _strategyProcesses[GetRequestProcessImplementations.PermissionsKeyValuePair];

                //PermissionsKeyValuePairStrategy strategy = new PermissionsKeyValuePairStrategy(getParameters, _mailingHistoryService,
                //    _mciRequestService, _clientScopeService,
                //    _logger, _permissionsService,
                //    _membershipService, _customerDetailsService,
                //    _travelSummaryService, _customerMatchService);

                //getParameters.AccessControl = _clientScopeService.VerifyUserHasAccessToGroupCode(getParameters.AccessControl, Scope.CUSTOMER_PERMISSIONS_RETRIEVE_KVPAIR);

                object output = strategy.Execute(getParameters);

                return Ok(output);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"PermissionGetKeyValuePair: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters PermissionLoad:- Name=Key={getParameters.KeyValue.Key}, Value={getParameters.KeyValue.Value}");
                }
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }



        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_PERMISSIONS_RETRIEVE_NAMEADDR")]
        [HttpPost]
        [ActionName("NameAndAddress")]
        public IHttpActionResult Post(NameAndAddressParameter parameter)
        {
            try
            {

                // Use indexed DI resoloution to get the required strategy 
                IGetRequestProcess nameAndAddress = _strategyProcesses[GetRequestProcessImplementations.NameAndAddress];

                //NameAndAddressStrategy nameAndAddress = new NameAndAddressStrategy(parameter, _mailingHistoryService,
                //    _mciRequestService, _clientScopeService, _logger, _permissionsService, _membershipService,
                //    _customerMatchService, _customerDetailsService, _travelSummaryService);
                parameter.AccessControl = _clientScopeService.VerifyUserHasAccessToGroupCode(parameter.AccessControl,
                    Scope.CUSTOMER_PERMISSIONS_RETRIEVE_NAMEADDR);
                object output = nameAndAddress.Execute(parameter);
                return Ok(output);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"PermissionGetNameAndAddress: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters PermissionLoad:- Name=FirstName={parameter.NameAndAddress.FirstName}, LastName={parameter.NameAndAddress.Surname}, Dob={parameter.NameAndAddress.Dob}, " +
                        $" Address=AddresLine1={ parameter.NameAndAddress.Address.Address1 ?? ""}, AddressLine2={parameter.NameAndAddress.Address.Address2 ?? ""}, AddressLine3={parameter.NameAndAddress.Address.Address3 ?? ""}, " +
                        $" AddressLine4={parameter.NameAndAddress.Address.Address4 ?? ""}, PostCode={parameter.NameAndAddress.Address.Postcode ?? ""}");

                }
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

        [HttpPost]
        [ActionName("CustomerPermissionsLoad")]
        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_PERMISSIONS_LOAD")]
        public IHttpActionResult Post(PermissionsCustomerLoad permissions)
        {
            try
            {


                var mappedPermissions = Mapper.Map<WebApiServices.Models.Gdpr.PermissionsCustomerLoadModel>(permissions);
                var processPermission =
                    new ProcessPermission(null, _permissionsService, _mciRequestService, _logger, null);
                var message = processPermission.AddOrUpdateCustomerPermission(mappedPermissions);

                //Post show return summary instead of message if the load is successfully
                if (string.Compare(DatabaseMessage.PermissionsCreated, message,
                        StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    if ((!string.IsNullOrWhiteSpace(permissions.CustomerNameAndAddress.FirstName)) &&
                        (!string.IsNullOrWhiteSpace(permissions.CustomerNameAndAddress.Surname)) &&
                        (permissions.CustomerNameAndAddress.Address != null))
                    {
                        NameAndAddressParameter parameter = new NameAndAddressParameter() { NameAndAddress = permissions.CustomerNameAndAddress, ResponseRequestedItems = new List<string>() { ReturnMeTypeConstant.Permissions.ToString() } };

                        parameter.ReturnMe = new ReturnMe()
                        {
                            Permissions = new ReturnMePermissions
                            {
                                Required = true,
                                Journey = permissions.JourneyType,
                                ResponseParameter = GdprResponseType.Summary.ToString(),
                                PermissionParameter = "All",
                                MatchType = Saga.Gmd.WebApiServices.Common.MatchType.POSTCODE_AND_NAME.ToString()

                            }
                        };
                        parameter.NameAndAddress.MatchType = Saga.Gmd.WebApiServices.Common.MatchType.POSTCODE_AND_NAME
                            .ToString();

                        // Use indexed DI resoloution to get the required strategy 
                        IGetRequestProcess nameAndAddress = _strategyProcesses[GetRequestProcessImplementations.NameAndAddress];
                        
                        //NameAndAddressStrategy nameAndAddress = new NameAndAddressStrategy(parameter,
                        //    _mailingHistoryService, _mciRequestService, _clientScopeService, _logger, _permissionsService,
                        //    _membershipService, _customerMatchService, _customerDetailsService, _travelSummaryService);
                        parameter.AccessControl =
                            _clientScopeService.VerifyUserHasAccessToGroupCode(parameter.AccessControl,
                                Scope.CUSTOMER_PERMISSIONS_RETRIEVE_NAMEADDR);
                        object output = nameAndAddress.Execute(parameter);
                        return Ok(output);
                    }
                    else
                    {
                        KeyValueParameter parameter = new KeyValueParameter()
                        {
                            ResponseRequestedItems = new List<string>() { ReturnMeTypeConstant.Permissions.ToString() },
                            KeyValue = new KeyValue() { Key = "CPCK", Value = permissions.PermissionsId.ToString() }
                        };

                        parameter.ReturnMe = new ReturnMeForKeyValuePair()
                        {
                            Permissions = new ReturnMePermissions
                            {
                                Required = true,
                                Journey = permissions.JourneyType,
                                ResponseParameter = GdprResponseType.Summary.ToString(),
                                PermissionParameter = "All"
                            }
                        };

                        // Use indexed DI resoloution  to get the required strategy 
                        IGetRequestProcess strategy = _strategyProcesses[GetRequestProcessImplementations.PermissionsKeyValuePair];

                        //PermissionsKeyValuePairStrategy strategy = new PermissionsKeyValuePairStrategy(parameter, _mailingHistoryService,
                        //    _mciRequestService, _clientScopeService,
                        //    _logger, _permissionsService,
                        //    _membershipService, _customerDetailsService,
                        //    _travelSummaryService, _customerMatchService);
                        object output = strategy.Execute(parameter);
                        return Ok(output);
                    }

                }
                return Ok(message);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"PermissionGetNameAndAddress: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters PermissionLoad:- Name=FirstName={permissions.CustomerNameAndAddress.FirstName}, LastName={permissions.CustomerNameAndAddress.Surname}, Dob={permissions.CustomerNameAndAddress.Dob}, " +
                        $" Address=AddresLine1={ permissions.CustomerNameAndAddress.Address.Address1 ?? ""}, AddressLine2={permissions.CustomerNameAndAddress.Address.Address2 ?? ""}, AddressLine3={permissions.CustomerNameAndAddress.Address.Address3 ?? ""}, " +
                        $" AddressLine4={permissions.CustomerNameAndAddress.Address.Address4 ?? ""}, PostCode={permissions.CustomerNameAndAddress.Address.Postcode ?? ""}");

                }
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }
    }
}
