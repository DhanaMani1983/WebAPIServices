using System;
using Autofac.Features.Indexed;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using log4net;
using Saga.Gmd.WebApiServices.Api.Components;
using Saga.SSO.ClientCredentials.Endpoint.ApiFilters;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;
using Saga.Gmd.WebApiServices.BLL.Transaction;
using Saga.Gmd.WebApiServices.Api.WebClients;
using System.Configuration;
using AutoMapper;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.Models.Membership;

namespace Saga.Gmd.WebApiServices.Api.Controllers
{
    public partial class CustomerController : ApiController
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
        private readonly IGmdToAfeService _gmdToAfeService;
        private readonly IAfeWebApiClient _afeWebClient;
        private readonly bool _logParameterValue;
        private readonly IIndex<GetRequestProcessImplementations, IGetRequestProcess> _strategyProcesses;
        private readonly IIndex<StrategyWriterPartImplementations, IStrategyWriterPart<MembershipDataInput, int, int, MembershipDetails>> _membershipDetailsWriterParts;
        private readonly IIndex<StrategyWriterPartImplementations, IStrategyWriterPart<MembershipDataInput, int, int, string>> _membershipWriterParts;
        private readonly IMapper _mapper;


        public CustomerController(
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
            IGmdToAfeService gmdToAfeService,
		    IAfeWebApiClient afeWebClient,	
            IIndex<GetRequestProcessImplementations, IGetRequestProcess> strategyProcesses,
            IIndex<StrategyWriterPartImplementations, IStrategyWriterPart<MembershipDataInput, int, int, MembershipDetails>> membershipDetailsWriterParts,
            IIndex<StrategyWriterPartImplementations, IStrategyWriterPart<MembershipDataInput, int, int, string>> membershipWriterParts,
            IMapper mapper
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
            _gmdToAfeService = gmdToAfeService;
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);
            _strategyProcesses = strategyProcesses;
            _membershipWriterParts = membershipWriterParts;
            _membershipDetailsWriterParts = membershipDetailsWriterParts;
            _afeWebClient = afeWebClient;
            _mapper = mapper;
        }


        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_RETRIEVE_KVPAIR")]
        [HttpGet]
        [ActionName("KeyValuePair")]
        [Route("api/v2/customer/KeyValuePair")]
        // [Route("api/v4/customer/KeyValuePair")]
        public IHttpActionResult GetV2(KeyValueParameter getParameters)
        {

            try
            {
                // Get the correct strategy via indexed DI resoloution 
                IGetRequestProcess keyValuePair = _strategyProcesses[GetRequestProcessImplementations.KeyValuePairV2];

                getParameters.AccessControl =
                    _clientScopeService.VerifyUserHasAccessToGroupCode(getParameters.AccessControl,
                        Scope.CUSTOMER_RETRIEVE_KVPAIR);
                object output = keyValuePair.Execute(getParameters);

                return Ok(output);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"CustomerGetKeyValuePairv2: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters CustomerGetKeyValuePairv2:- Name=Key={getParameters.KeyValue.Key}, Value={getParameters.KeyValue.Value}");
                }

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }



        [ResponseType(typeof(ResponsePackage))]
        [AuthenticateIdentity()]
        [AuthorizeIdentity(Role = "CUSTOMER_RETRIEVE_KVPAIR")]
        [HttpGet]
        [ActionName("KeyValuePair")]
        public IHttpActionResult Get(KeyValueParameter getParameters)
        {

            try
            {

                // Use indexed DI resoloution 
                IGetRequestProcess keyValuePair = _strategyProcesses[GetRequestProcessImplementations.KeyValuePair];

                //KeyValuePairStrategy keyValuePair = new KeyValuePairStrategy(getParameters, _mailingHistoryService,
                //    _mciRequestService, _clientScopeService, _logger, _permissionsService, _membershipService,
                //    _customerDetailsService, _travelSummaryService, _customerMatchService,
                //    _membershipFlagsKeyValueReader
                //    );
                getParameters.AccessControl =
                    _clientScopeService.VerifyUserHasAccessToGroupCode(getParameters.AccessControl,
                        Scope.CUSTOMER_RETRIEVE_KVPAIR);
                object output = keyValuePair.Execute( getParameters );

                return Ok(output);
            }
            catch (Exception exception)
            {
                if (_logParameterValue)
                {
                    _logger.Error($"MembershipGetKeyValuePair: ErrorTag: {ErrorTagProvider.ErrorTagDatabase} --  {exception.Message}");

                    _logger.Error(
                        $"Parameters PermissionLoad:- Name=Key={getParameters.KeyValue.Key}, Value={getParameters.KeyValue.Value}");
                }

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, exception.Message));
            }
        }

    }
}
