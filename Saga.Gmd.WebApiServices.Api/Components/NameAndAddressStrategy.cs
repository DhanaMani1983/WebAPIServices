using System;
using System.Runtime.Remoting.Messaging;
using Autofac.Features.Indexed;
using log4net;
using Saga.Gmd.WebApiServices.Api.Components.KeyValuePair;
using Saga.Gmd.WebApiServices.Api.Components.Membership;
using Saga.Gmd.WebApiServices.Api.Components.NameAndAddress;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Components.Permission;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public class NameAndAddressStrategy : IGetRequestProcess
    {

        private readonly KeyValueParameter _keyValue;

        private NameAndAddressParameter _nameAndAddress;

        private readonly IMailingHistoryService _mailingHistoryService;
        private readonly IMciRequestService _mciRequestService;
        private readonly IClientScopeService _clientScopeService;
        private readonly ILog _logger;
        public readonly IPermissionService PermissionService;
        private readonly IMembershipService _membershipService;
        private readonly ICustomerMatchService _customerMatchService;
        private readonly ICustomerDetailsService _customerDetailsService;
        private readonly ITravelSummaryService _travelSummaryService;
        private readonly ICustomerKeyProcess _customerKeyProcess;

        protected readonly IIndex<NameAndAddressStrategyReaderImplementations, INameAndAddressStrategyReaderPart>
            _nameAndAddressReaders;

        protected INameAndAddressStrategyReaderPart _membershipOptionsReader;


        /// <summary>
        /// nameAndAddress now an instance parameter on Execute()
        /// </summary>
        public NameAndAddressStrategy(
            IMailingHistoryService mailingHistoryService,
            IMciRequestService mciRequestService, IClientScopeService clientScopeService,
            ILog logger,
            IPermissionService permissionService,
            IMembershipService membershipService,
            ICustomerMatchService customerMatchService,
            ICustomerDetailsService customerDetailsService, 
            ITravelSummaryService travelSummaryService,
            ICustomerKeyProcess customerKeyProcess,
            IIndex<NameAndAddressStrategyReaderImplementations, INameAndAddressStrategyReaderPart> nameAndAddressReaders
            )
        {
            _mailingHistoryService = mailingHistoryService;
            _mciRequestService = mciRequestService;
            _clientScopeService = clientScopeService;
            _logger = logger;
            PermissionService = permissionService;
            _membershipService = membershipService;
            _customerMatchService = customerMatchService;
            _customerDetailsService = customerDetailsService;
            _travelSummaryService = travelSummaryService;
            _customerKeyProcess = customerKeyProcess;
            _nameAndAddressReaders = nameAndAddressReaders;

            _membershipOptionsReader = _nameAndAddressReaders[NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPart];
        }


        public object[] Execute(KeyValueParameter keyValue)
        {
            throw new NotSupportedException("Key value calls not supported on NameAndAddressStrategy");
        }


        public object[] Execute(NameAndAddressParameter nameAndAddress)
        {
            int processedObject = 0;

            _nameAndAddress = nameAndAddress;

            object[] output = new object[_nameAndAddress.ResponseRequestedItems.Count];
            try
            {
                foreach (var item in _nameAndAddress.ResponseRequestedItems)
                {
                    switch (item)
                    {
                        case ReturnMeTypeConstant.CustomerKeys:
                            //CustomerKeyProcess customerKeyProcess = new CustomerKeyProcess(_keyValue,
                            //    _mciRequestService, _clientScopeService, _logger, _nameAndAddress);
                            //output[processedObject] = customerKeyProcess.ProcessForNameAndAddress( _nameAndAddress, _keyValue );
                            //
                            output[processedObject] = _customerKeyProcess.ProcessForNameAndAddress(_nameAndAddress, _keyValue);
                            // AE Feb 18 : Use process from DI container 

                            processedObject++;
                            break;

                        case ReturnMeTypeConstant.MailingHistory:
                            MailingHistoryProcess mailingHistoryProcess = new MailingHistoryProcess(_nameAndAddress,
                                _mailingHistoryService, _mciRequestService, _clientScopeService, _logger, _keyValue);
                            output[processedObject] = mailingHistoryProcess.Process();
                            processedObject++;
                            break;
                        case ReturnMeTypeConstant.Permissions:
                            var processPermission = new ProcessPermission(_keyValue, PermissionService,
                                _mciRequestService, _logger, _nameAndAddress);
                            output[processedObject] = processPermission.ProcessPermissionForNameAndAddress();
                            processedObject++;
                            break;
                        case ReturnMeTypeConstant.Membership:

                            // GITCS-1 : Refactor to NameAndAddressStrategyMembershipReaderParts, in place of MembershipProcess class
                            //
                            var membershipDetailReader =
                                _nameAndAddressReaders[
                                    NameAndAddressStrategyReaderImplementations.MembershipDetailsReaderPart];
                            var memberObj = membershipDetailReader.Process(_nameAndAddress,
                                _keyValue?.KeyValue.IntValue   );
                            /*
                            var membershipProcess = new MembershipProcess(_keyValue, _membershipService,
                                _mciRequestService, _logger, _nameAndAddress);
                            var memberObj = membershipProcess.GetMembershipFromNameAndAddress();
                            */

                            output[processedObject] = memberObj;
                            processedObject++;

                            if (memberObj != null && (memberObj.GetType() != typeof(NoAccessToMembership)))
                            {
                                //need to resize the array to accommodate the membershipstatus, membershipCancelReason and MembershipStatusReason values
                                //that are retrieved from GMD to populate the frontend e.g AFE 
                                Array.Resize(ref output, output.Length + 1);
                                // output[processedObject] = membershipProcess.GetMembershipOptionsData();

                                //var membershipOptionsReader =
                                //    _nameAndAddressReaders[
                                //        NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPart ];
                                output[processedObject] = _membershipOptionsReader.Process(_nameAndAddress, null);

                                processedObject++;
                            }

                            break;
                        case ReturnMeTypeConstant.CustomerMatch:
                            var customerMatch = new CustomerMatchProcess(_nameAndAddress, _customerMatchService,_travelSummaryService, _logger);
                            var matchedCustomerDetails = customerMatch.Process();
                            output[processedObject] = matchedCustomerDetails;
                            processedObject++;
                            break;
                        case ReturnMeTypeConstant.TravelSummary:
                            var travelSummary = new CustomerMatchProcess(_nameAndAddress, _customerMatchService, _travelSummaryService, _logger); 
                            output[processedObject] = travelSummary.TravelSummaryProcess(0, _nameAndAddress.NameAndAddress);
                            processedObject++;
                            break;
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.Error(
                    "NameAndAddressStrategy: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);

            }
            return output;
        }
    }
}