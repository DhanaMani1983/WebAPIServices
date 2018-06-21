using System;
using log4net;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using Saga.Gmd.WebApiServices.Api.Models.JsonModels;
using Saga.Gmd.WebApiServices.Api.Models.ParameterModels;
using Saga.Gmd.WebApiServices.BLL.Customer;
using Saga.Gmd.WebApiServices.BLL.Permissions;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.Api.Components.Permission
{
    public class ProcessPermission
    {
        private readonly KeyValueParameter _keyValueParameter;
        private readonly IPermissionService _permissionService;
        private readonly ILog _logger;
        private readonly NameAndAddressParameter _nameAndAddress;
        private readonly IMciRequestService _mciRequestService;
        private readonly bool _logParameterValue;

        public ProcessPermission(KeyValueParameter keyValueParameter, IPermissionService permissionService, IMciRequestService mciRequestService, ILog logger, NameAndAddressParameter nameAndAddress)
        {
            _permissionService = permissionService;
            _logger = logger;
            _nameAndAddress = nameAndAddress;
            _mciRequestService = mciRequestService;
            _keyValueParameter = keyValueParameter;
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);
        }

        //Constructor used for Post request as will not require paramter object  
        public ProcessPermission(IPermissionService permissionService, IMciRequestService mciRequestService, ILog logger) : this(null, permissionService, mciRequestService, logger, null)
        {

        }

        /// <summary>
        /// PermissionsKeyValuePairStrategy.Execute() worker method.. 
        /// </summary>
        /// <returns></returns>
        public object ProcessPermissionFromKeyValue()
        {
            object listOfPermission = new object();
            CustomerKeys keys = new CustomerKeys();

            try
            {

                int pkey = string.Compare(_keyValueParameter.KeyValue.Key, "CPCK", StringComparison.CurrentCultureIgnoreCase) == 0 ? GetCustomerIdFromPermissionId(Convert.ToInt64(_keyValueParameter.KeyValue.Value)) : GetCustomerIdFromKeyValue();


                if (pkey == 0)
                {
                    keys.Keys.HasPartialResult = false;
                    keys.Keys.Message = "No Pkey found for the customer.";
                }
                else
                {
                    try
                    {
                        listOfPermission = _permissionService.GetPermission(_keyValueParameter.ReturnMe.Permissions, _nameAndAddress.NameAndAddress, _keyValueParameter.KeyValue);

                        return listOfPermission;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("ProcessPermission Name and Value :" + ex.Message, ex);
                        if (_logParameterValue)
                        {
                            _logger.Error($"Parameters ProcessPermissionFromKeyValue:- Key={_keyValueParameter.KeyValue.Key}, Value={_keyValueParameter.KeyValue.Value}");
                        }
                        throw new Exception(ex.Message);
                    }

                }

            }
            catch (Exception ex)
            {
                _logger.Error("ProcessPermission Key and Value Execute :" + ex.Message, ex);
                if (_logParameterValue)
                {
                    _logger.Error($"Parameters ProcessPermissionFromKeyValue:- Key={_keyValueParameter.KeyValue.Key}, Value={_keyValueParameter.KeyValue.Value}");
                }

                throw new Exception(ex.Message);
            }

            return listOfPermission;
        }

        private int GetCustomerIdFromKeyValue()
        {
            var customerKey = _mciRequestService.GetCustomerAllIndexKeys(_keyValueParameter.KeyValue.Key,
                _keyValueParameter.KeyValue.Value, SourceKey.PKEY.ToString());
            List<int> customerIds = new List<int>();
            foreach (var key in customerKey)
            {
                customerIds.Add(key.CustomerId);
            }

            return customerIds.First();
        }


        private int GetCustomerIdFromPermissionId(long permissionsId)
        {
            return _permissionService.GetCustomerKeyForPermissionId(permissionsId);
        }

        public object ProcessPermissionForNameAndAddress()
        {
            try
            {
                object listOfPermission = _permissionService.GetPermission(_nameAndAddress.ReturnMe.Permissions, _nameAndAddress.NameAndAddress, null);

                return listOfPermission;
            }
            catch (Exception ex)
            {
                _logger.Error("Process Permission Name and Value :" + ex.Message, ex);

                if (_logParameterValue)
                {
                    _logger.Error(
                        $"Parameters ProcessPermissionForNameAndAddress:- Name=FirstName={_nameAndAddress.NameAndAddress.FirstName}, LastName={_nameAndAddress.NameAndAddress.Surname}, Dob={_nameAndAddress.NameAndAddress.Dob}, " +
                        $"Address=AddresLine1={_nameAndAddress.NameAndAddress.Address.Address1}, AddressLine2={_nameAndAddress.NameAndAddress.Address.Address2}, AddressLine3={_nameAndAddress.NameAndAddress.Address.Address3}, " +
                        $"AddressLine4={_nameAndAddress.NameAndAddress.Address.Address4}, PostCode={_nameAndAddress.NameAndAddress.Address.Postcode}");
                }

                throw new Exception(ex.Message);
            }
        }

        public string AddOrUpdateCustomerPermission(WebApiServices.Models.Gdpr.PermissionsCustomerLoadModel permission)
        {
            try
            {
                if (permission == null || permission?.PermissionCategory.Count == 0)
                    throw new ArgumentNullException(nameof(permission), "Permissions object cannot be null for Post operation.");
                return _permissionService.PostPermission(permission);
            }
            catch (Exception ex)
            {
                _logger.Error("ProcessPermission Post Permission Data:" + ex.Message, ex);
                throw new Exception(ex.Message);
            }
        }
    }
}