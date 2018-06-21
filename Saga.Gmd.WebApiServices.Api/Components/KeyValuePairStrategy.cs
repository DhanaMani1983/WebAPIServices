using System;
using System.Net;
using log4net;
using Saga.Gmd.WebApiServices.Api.Components.KeyValuePair;
using Saga.Gmd.WebApiServices.Api.Components.Membership;
using Saga.Gmd.WebApiServices.Api.Components.NameAndAddress;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Components.StrategyParts;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;
using Autofac.Features.Indexed;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public class KeyValuePairStrategy : IGetRequestProcess
    {

        private KeyValueParameter _keyValue;

        private readonly NameAndAddressParameter _nameAndAddress;

        private readonly IMailingHistoryService _mailingHistoryService;
        private readonly IMciRequestService _mciRequestService;
        private readonly IClientScopeService _clientScopeService;
        private readonly IPermissionService _permissionService;
        private readonly ILog _logger;
        private readonly IMembershipService _membershipService;
        private readonly ICustomerDetailsService _customerDetailsService;
        private readonly ITravelSummaryService _travelSummaryService;
        private readonly ICustomerMatchService _customerMatchService;
        private readonly IKeyValueStrategyReaderPart _membershipFlagsKeyValueReader;
        private readonly ICustomerDetailsProcess _customerDetailsProcess;
        private readonly ICustomerKeyProcess _customerKeyProcess;

        protected readonly IIndex<NameAndAddressStrategyReaderImplementations, INameAndAddressStrategyReaderPart>
            _nameAndAddressReaders;

        protected INameAndAddressStrategyReaderPart _membershipOptionsReader;


        public KeyValuePairStrategy( 
                                     IMailingHistoryService mailingHistoryService,
                                     IMciRequestService mciRequestService,
                                     IClientScopeService clientScopeService,
                                     ILog logger, 
                                     IPermissionService permissionsService,
                                     IMembershipService membershipService, 
                                     ICustomerDetailsService customerDetailsService, 
                                     ITravelSummaryService travelSummaryService, 
                                     ICustomerMatchService customerMatchService,
                                     IKeyValueStrategyReaderPart membershipFlagsKeyValueReader,
                                     ICustomerDetailsProcess customerDetailsProcess,
                                     ICustomerKeyProcess customerKeyProcess,
                                     IIndex<NameAndAddressStrategyReaderImplementations, INameAndAddressStrategyReaderPart> nameAndAddressReaders
            )
        {
            _mailingHistoryService = mailingHistoryService;
            _mciRequestService = mciRequestService;
            _clientScopeService = clientScopeService;
            _permissionService = permissionsService;
            _membershipService = membershipService;
            _customerDetailsService = customerDetailsService;
            _travelSummaryService = travelSummaryService;
            _customerMatchService = customerMatchService;
            _logger = logger;
            _membershipFlagsKeyValueReader = membershipFlagsKeyValueReader;
            _customerDetailsProcess = customerDetailsProcess;
            _customerKeyProcess = customerKeyProcess;
            _nameAndAddressReaders = nameAndAddressReaders;

            _membershipOptionsReader = _nameAndAddressReaders[NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPart];
            // Set local ref to V1 reader 

        }

        public object[] Execute(NameAndAddressParameter nameAndAddress)
        {
            throw new NotSupportedException("Name and address calls not supported on KeyValuePairStrategy");
        }


        /// <summary>
        /// keyValue now an instance parameter to allow for Autofac DI instantiation of strategy classes...
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public object[] Execute( KeyValueParameter keyValue)
        {

            _keyValue = keyValue;
            int processedObject = 0;

            object[] output = new object[_keyValue.ResponseRequestedItems.Count];

            bool customerFound = true;
            try
            {
                if (_keyValue.ReturnMe.CustomerKeys != null)
                {  
                    // AE : Feb 18 - Used injected key Process instance from DI container
                    // var customerKeyProcess = new CustomerKeyProcess(_keyValue, _mciRequestService,
                    //     _clientScopeService, _logger);
                    output[processedObject] = _customerKeyProcess.Process(_keyValue);

                    customerFound = _customerKeyProcess.CustomerFound;
                    processedObject++;

                }

                if (customerFound)
                {
                    if (_keyValue.ReturnMe.MailingHistory != null)
                    {
                        MailingHistoryProcess mailingHistoryProcess = new MailingHistoryProcess(
                            _nameAndAddress, _mailingHistoryService, _mciRequestService, _clientScopeService,
                            _logger, _keyValue);
                        output[processedObject] = mailingHistoryProcess.ProcessForKeyValue();
                        processedObject++;
                    }


                    // CITCS-17 - Request includes an aggregated view of Membership Permissions
                    if ( _keyValue.ReturnMe.MembershipFlags.Required  )
                    {
                        // IKeyValueReadStrategyPart mebmershipFlagsReader = new KeyValueMembershipFlagsReadStrategyPart( 
                        //    _keyValue, _permissionService, _mciRequestService, _membershipService, _logger 
                        //     );
                        output[processedObject] = _membershipFlagsKeyValueReader.Process(_keyValue );
                        processedObject++;

                    }

                    // As per Sam Sheperd 19/09/2017 - We should not process any permissions data request in /Customer
                    // so I have commented this out and asked Jay to remove from the documentation

                    //if (_keyValue.ReturnMe.Permissions != null && _keyValue.ReturnMe.Permissions.Required)
                    //{
                    //    var permissionProcess = new ProcessPermission(_keyValue, _permissionService,
                    //        _mciRequestService, _logger, _nameAndAddress);
                    //    output[_processedObject] = permissionProcess.ProcessPermissionFromKeyValue();
                    //    _processedObject++;
                    //}

                    if (_keyValue.ReturnMe.Membership != null && _keyValue.ReturnMe.Membership.Required)
                    {
                        var membershipProcess = new MembershipProcess(_keyValue, _membershipService,
                            _mciRequestService, _logger);

                        var memberObj = membershipProcess.ProcessMembershipWithKeyValue();

                        if (!membershipProcess.CustomerFound)
                        {
                            output[processedObject] = HttpStatusCode.NotFound; 
                        }
                        else
                        {
                            output[processedObject] = memberObj;
                        }
                        processedObject++;
                        if (memberObj != null && (memberObj.GetType() != typeof(NoAccessToMembership)))
                        {
                            //need to resize the array to accommodate the membershipstatus, membershipCancelReason and MembershipStatusReason values
                            //that are retrieved from GMD to populate the frontend e.g AFE
                            Array.Resize(ref output, output.Length + 1);
                            // output[processedObject] = membershipProcess.GetMembershipOptionsData();

                            // GITCS-1 : Retire above membership process method 
                            // _membershipOptionsReader = 
                            //    _nameAndAddressReaders[NameAndAddressStrategyReaderImplementations.MembershipOptionsReaderPart];
                            output[processedObject] = _membershipOptionsReader.Process(_nameAndAddress, null);

                            processedObject++;
                        }
                    }

                    if (_keyValue.ReturnMe.CustomerDetails != null && _keyValue.ReturnMe.CustomerDetails.Required)
                    {
                        CustomerInfo info = new CustomerInfo();

                        // var customerDetails = new CustomerDetailsProcess(_keyValue, _customerDetailsService, _logger);
                        info.CustomerDetails = _customerDetailsProcess.Process(_keyValue);
                        // AE : 15 Feb 18 : Call process interface from DI container

                        if (info.CustomerDetails == null)
                            output[processedObject] = HttpStatusCode.NotFound;
                        else
                            output[processedObject] = info;
                        processedObject++;
                    }

                    if (_keyValue.ReturnMe.TravelSummary != null && _keyValue.ReturnMe.TravelSummary.Required)
                    {
                        var travelSummary = new CustomerMatchProcess(_nameAndAddress, _customerMatchService, _travelSummaryService, _logger);
                        output[processedObject] = travelSummary.TravelSummaryProcess(int.Parse(_keyValue.KeyValue.Value));
                        processedObject++;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("KeyValuePairStrategy: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }

            return output;
        }
    }
}