using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using log4net.Appender;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.BLL.Customer
{
    public class MciRequestService : IMciRequestService
    {
        private readonly IMciRequestDataAccess _mciRequestDataAccess;
        private readonly ILog _logger;

        public MciRequestService(IMciRequestDataAccess mciRequestDataAccess, ILog logger)
        {
            this._mciRequestDataAccess = mciRequestDataAccess;
            _logger = logger;
        }

        public List<ClientScopes> GetClientScopes(string clientId)
        { 
            List<ClientScopes> dalScopes = _mciRequestDataAccess.GetClientScopes(clientId); 
            return dalScopes;
        }

        public List<CustomerIndexResult> GetCustomerAllIndexKeys(string sourceKey, string custRef, string targetKey)
        { 
            List<CustomerIndexResult> results = _mciRequestDataAccess.GetCustomerAllIndexKeys(sourceKey, custRef, targetKey); 

            return results;
        }

        public int? GetPersistantKey(NameAndAddress nameAndAddress)
        {
            var pkey =_mciRequestDataAccess.GetPersistantKey(nameAndAddress);
            return pkey;
        }

        public int? GetPersistantKey(string key, string value,string targetKey)
        {
            List<CustomerIndexResult> output = _mciRequestDataAccess.GetCustomerAllIndexKeys(key, value, targetKey);
            return output?.First().CustomerId;
        }

        public long SaveCustomer(Models.Customer.CustomerLoadOrMatch customerLoad)
        {
           return _mciRequestDataAccess.SaveCustomer(customerLoad);
        }

        public long SaveCustomer(CustomerLoadOrMatch customersLoad,out int customerId, out DateTime processedDatetime)
        {
            return _mciRequestDataAccess.SaveCustomer(customersLoad, out customerId, out processedDatetime);
        }
    }
}
