using System;
using log4net;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.MailingHistory;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Api.Components.Permission;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Membership;
using Saga.Gmd.WebApiServices.BLL.Permissions;

namespace Saga.Gmd.WebApiServices.Api.Components
{
    public class PermissionsKeyValuePairStrategy : IGetRequestProcess
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

        public PermissionsKeyValuePairStrategy(
            IMailingHistoryService mailingHistoryService,
                                     IMciRequestService mciRequestService,
                                     IClientScopeService clientScopeService,
                                     ILog logger, IPermissionService permissionsService,
                                     IMembershipService membershipService,
                                     ICustomerDetailsService customerDetailsService,
                                     ITravelSummaryService travelSummaryService,
                                     ICustomerMatchService customerMatchService)
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
        }

        public object[] Execute(NameAndAddressParameter nameAndAddress)
        {
            throw new NotSupportedException("Name and address calls not supported on PermissionsKeyValuePairStrategy");
        }


        public object[] Execute(KeyValueParameter keyValue)
        {
            int processedObject = 0;

            _keyValue = keyValue;

            object[] output = new object[_keyValue.ResponseRequestedItems.Count];
            try
            {
                //if (_keyValue.ReturnMe.CustomerKeys != null)
                //{
                //    var customerKeyProcess = new CustomerKeyProcess(_keyValue, _mciRequestService,
                //        _clientScopeService, _logger);
                //    output[_processedObject] = customerKeyProcess.Process();
                //    _processedObject++;
                //}

                //if (_keyValue.ReturnMe.MailingHistory != null)
                //{
                //    MailingHistoryProcess mailingHistoryProcess = new MailingHistoryProcess(
                //        _nameAndAddress, _mailingHistoryService, _mciRequestService, _clientScopeService,
                //        _logger, _keyValue);
                //    output[_processedObject] = mailingHistoryProcess.ProcessForKeyValue();
                //    _processedObject++;
                //}

                if (_keyValue.ReturnMe.Permissions != null && _keyValue.ReturnMe.Permissions.Required)
                {
                    // we make some temporary parameters to pass so that we can create an address to construct 
                    // the ProcessPermission() object

                    //KeyValueParameter tempKVP = new KeyValueParameter();
                    //tempKVP.KeyValue = _keyValue.KeyValue;
                    //tempKVP.AccessControl = new WebApiServices.Models.Security.AccessControl();
                    //tempKVP.AccessControl.CustomerKeyAccess.Add(new WebApiServices.Models.Security.CustomerKeyAccess() { HasAccess = true, Key = WebApiServices.Models.GroupCode.GCUST });
                    //tempKVP.AccessControl.ModuleAccess.Add(new WebApiServices.Models.Security.ModuleAccess() { HasAccess = true, Key = WebApiServices.Models.GroupCode.GCUST });
                    //tempKVP.ReturnMe.CustomerDetails.AddressType = AddressType.Correspondence;


                    //var customerDetails = new CustomerDetailsProcess(tempKVP, _customerDetailsService, _logger);
                    //WebApiServices.Models.Customer.CustomerInfoDetails details = customerDetails.Process() as WebApiServices.Models.Customer.CustomerInfoDetails;

                    NameAndAddressParameter tempNad = new NameAndAddressParameter();

                    tempNad.AccessControl = _keyValue.AccessControl;
                    tempNad.ResponseRequestedItems = _keyValue.ResponseRequestedItems;
                    tempNad.ReturnMe = _keyValue.ReturnMe;
                    tempNad.NameAndAddress = new WebApiServices.Models.NameAndAddress();



                    //if (details != null)
                    //{
                    //tempNad.NameAndAddress.FirstName = details.FirstName;
                    //tempNad.NameAndAddress.Surname = details.Surname;
                    //tempNad.NameAndAddress.Title = details.Title;
                    //tempNad.NameAndAddress.Dob = details.Dob;

                    //if (details.Address.GetType() == typeof(WebApiServices.Models.Customer.CorrespondenceAddress))
                    //{
                    //    WebApiServices.Models.Customer.CorrespondenceAddress a = details.Address as WebApiServices.Models.Customer.CorrespondenceAddress;

                    //    tempNad.NameAndAddress.Address.Address1 = a.Address1;
                    //    tempNad.NameAndAddress.Address.Address2 = a.Address2;
                    //    tempNad.NameAndAddress.Address.Address3 = a.Address3;
                    //    tempNad.NameAndAddress.Address.Address4 = a.Address4;
                    //    tempNad.NameAndAddress.Address.Postcode = a.Postcode;

                    //}
                    //else if(details.Address.GetType() == typeof(WebApiServices.Models.Customer.TransactionalAddress))
                    //{
                    //    WebApiServices.Models.Customer.TransactionalAddress a = details.Address as WebApiServices.Models.Customer.TransactionalAddress;

                    //    tempNad.NameAndAddress.Address = new Address()
                    //    {
                    //        Address1 = a.HouseNumber + (string.IsNullOrEmpty(a.HouseNumber) == false ? " " : "") + a.Street,
                    //        Address2 = a.Street1,
                    //        Address3 = a.City,
                    //        Address4 = a.County,
                    //        Postcode = a.Postcode
                    //    };

                    //}

                    //var permissionProcess = new ProcessPermission(_keyValue, _permissionService, _mciRequestService, _logger, _nameAndAddress);
                    var permissionProcess = new ProcessPermission(_keyValue, _permissionService, _mciRequestService, _logger, tempNad);
                    output[processedObject] = permissionProcess.ProcessPermissionFromKeyValue();
                    processedObject++;
                    // }
                }

                //if (_keyValue.ReturnMe.Membership != null && _keyValue.ReturnMe.Membership.Required)
                //{
                //    var membershipProcess = new MembershipProcess(_keyValue, _membershipService,
                //        _mciRequestService, _logger);
                //    var memberObj = membershipProcess.ProcessMembershipWithKeyValue();
                //    output[_processedObject] = memberObj;
                //    _processedObject++;
                //    if (memberObj != null && (memberObj.GetType() != typeof(NoAccessToMembership)))
                //    {
                //        //need to resize the array to accommodate the membershipstatus, membershipCancelReason and MembershipStatusReason values
                //        //that are retrieved from GMD to populate the frontend e.g AFE
                //        Array.Resize(ref output, output.Length + 1);
                //        output[_processedObject] = membershipProcess.GetMembershipOptionsData();
                //        _processedObject++;
                //    }
                //}

                //if (_keyValue.ReturnMe.CustomerDetails != null && _keyValue.ReturnMe.CustomerDetails.Required)
                //{
                //    CustomerInfo info = new CustomerInfo();
                //    var customerDetails = new CustomerDetailsProcess(_keyValue, _customerDetailsService,_logger);
                //    info.CustomerDetails = customerDetails.Process();
                //    output[_processedObject] = info;
                //    _processedObject++;
                //}

                //if (_keyValue.ReturnMe.TravelSummary != null && _keyValue.ReturnMe.TravelSummary.Required)
                //{
                //    var travelSummary = new CustomerMatchProcess(_nameAndAddress, _customerMatchService, _travelSummaryService, _logger);
                //    output[_processedObject] = travelSummary.TravelSummaryProcess(int.Parse(_keyValue.KeyValue.Value));
                //    _processedObject++;
                //}
            }
            catch (Exception ex)
            {
                _logger.Error($"{this.GetType().Name}: ErrorTag: {ErrorTagProvider.ErrorTag} -- {ex.Message}", ex);
                throw new Exception(ex.Message);
            }

            return output;
        }
    }
}