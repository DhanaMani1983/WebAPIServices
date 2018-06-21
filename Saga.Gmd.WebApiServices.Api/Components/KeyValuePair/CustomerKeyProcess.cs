using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using log4net;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.ReturnMe;

namespace Saga.Gmd.WebApiServices.Api.Components.KeyValuePair
{
    public interface ICustomerKeyProcess
    {
        /// <summary>
        /// Outcome of Process() call
        /// </summary>
        bool CustomerFound { get; }

        object Process(KeyValueParameter keyValueParameter);
        object ProcessForNameAndAddress(NameAndAddressParameter nameAndAddressParameter, KeyValueParameter keyValueParameter = null);
    }

    public class CustomerKeyProcess : ICustomerKeyProcess
    {

        private readonly IMciRequestService _mciRequestService;
        private readonly IClientScopeService _clientScopeService;
        private readonly ILog _logger;

        /// <summary>
        /// Outcome of Process() call
        /// </summary>
        public bool CustomerFound { get; private set; }

       
        public CustomerKeyProcess(
            IMciRequestService mciRequestService,
            IClientScopeService clientScopeService, 
            ILog logger 
            )
        {
            _mciRequestService = mciRequestService;
            _clientScopeService = clientScopeService;
            _logger = logger;

            CustomerFound = true;
        }

        public object Process(KeyValueParameter keyValueParameter)
        { 
            var customerIndexList = new List<CustomerIndexResult>();

            string messageSet = string.Empty;
            bool setHasPartialResult = false;
            CustomerKeys customerKeys = new CustomerKeys();
            List<string> emptyList = new List<string>();
            
            if (keyValueParameter.ReturnMe.CustomerKeys.SequenceEqual(emptyList))
            {
                NoAccessToCusotmerKeys message = new NoAccessToCusotmerKeys();
                message.Message = null;
                return message;
            }
            if (keyValueParameter.AccessControl.CustomerKeyAccess.Count > 0 && keyValueParameter.AccessControl.CustomerKeyAccess.Find(x => x.HasAccess) == null)
            {
                NoAccessToCusotmerKeys message = new NoAccessToCusotmerKeys();
                message.Message = "You dont' have access CustomerKeys data.";
                return message;
            }

            try
            {
                foreach (var code in keyValueParameter.AccessControl.CustomerKeyAccess)
                {

                    if (code.HasAccess)
                    {
                        var output = _mciRequestService.GetCustomerAllIndexKeys(keyValueParameter.KeyValue.Key,
                            keyValueParameter.KeyValue.Value, code.Key.ToString());

                        if ((output != null) && output.Count > 0)
                        {
                            if ((output as List<CustomerIndexResult>)[0].CustomerFound == false)
                            {
                                CustomerFound = false;
                                return HttpStatusCode.NotFound;
                            }
                        }

                        if (customerIndexList.Count > 0)
                        {
                            customerIndexList.AddRange(output);
                        }
                        else
                        {
                            customerIndexList = output;
                        }
                    }
                    else
                    {
                        setHasPartialResult = customerIndexList.Count > 0;
                        messageSet = string.Format(ErrorMessages.DontHavePermission, code.Key);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("CustomerKeyProcess: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }
            List<string> invalidValues = new List<string>();
            foreach (var key in keyValueParameter.ReturnMe.CustomerKeys)
            {
                 
                if (keyValueParameter.AccessControl.CustomerKeyAccess.Exists(x => x.HasAccess == false && x.Key.ToString() == key))
                {
                    invalidValues.Add(key);
                }


                GroupCode code;
                var isValidCode = Enum.TryParse(key, true, out code);

                if (!isValidCode)
                {
                    invalidValues.Add(key);
                }
            }

            

            if (invalidValues.Count > 0)
            {
                if (string.IsNullOrEmpty(messageSet))
                {
                    messageSet =
                        $"Provided CustomerKey request has invalid value/s '{string.Join(",", invalidValues.ToArray())}'";
                }
                setHasPartialResult = true;
            }


            customerKeys.Keys.Message = messageSet;
            customerKeys.Keys.HasPartialResult = setHasPartialResult;
            customerKeys.Keys.KeysList = customerIndexList;

            return customerKeys;
        }

        public object ProcessForNameAndAddress(NameAndAddressParameter nameAndAddressParameter, KeyValueParameter keyValueParameter = null)
        {
            List<string> emptyList = new List<string>();

            if (nameAndAddressParameter.ReturnMe.CustomerKeys.SequenceEqual(emptyList))
            {
                NoAccessToCusotmerKeys message = new NoAccessToCusotmerKeys();
                message.Message = null;
                return message;
            }
            else if (nameAndAddressParameter.AccessControl.CustomerKeyAccess.Find(x => x.HasAccess) == null)
            {
                NoAccessToCusotmerKeys message = new NoAccessToCusotmerKeys();
                message.Message = "You dont' have access CustomerKeys data.";
                return message;
            }

            if (nameAndAddressParameter == null)
                throw new ArgumentException("NameAndAddress cannot be null."); 
            
            object customerIndexList = new object();
            CustomerKeys keys = new CustomerKeys();

            try
            {
                var pkey = _mciRequestService.GetPersistantKey(nameAndAddressParameter.NameAndAddress);

                if (pkey == null || !pkey.HasValue)
                {
                    keys.Keys.HasPartialResult = false;
                    keys.Keys = null;
                    customerIndexList = keys;
                }
                else
                {
                    if (keyValueParameter == null)
                    {
                        ReturnMeForKeyValuePair returnme = new ReturnMeForKeyValuePair();
                        returnme.CustomerKeys = nameAndAddressParameter.ReturnMe.CustomerKeys;
                        returnme.MailingHistory = nameAndAddressParameter.ReturnMe.MailingHistory;
                        returnme.Membership = nameAndAddressParameter.ReturnMe.Membership;
                        returnme.Permissions = nameAndAddressParameter.ReturnMe.Permissions;
                        keyValueParameter = new KeyValueParameter {AccessControl = nameAndAddressParameter.AccessControl, ReturnMe = returnme };
                    }
                    keyValueParameter.KeyValue.Key = "PKEY";
                    keyValueParameter.KeyValue.Value = pkey.Value.ToString();

                    // AE : Feb 18
                    // Call to New self instance ! Replace with direct call to target ley value Process() method
                    // var customerKeyProcess = new CustomerKeyProcess(keyValueParameter, _mciRequestService,
                    //     _clientScopeService, _logger);
                    // customerIndexList = customerKeyProcess.Process();
                    customerIndexList = Process(keyValueParameter);

                }

            }
            catch (Exception ex)
            {
                _logger.Error("NameAndAddressStrategy Execute (CustomerKeys) " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message, ex);
                throw new Exception(ex.Message);
            }

            return customerIndexList;
        }

    }
}