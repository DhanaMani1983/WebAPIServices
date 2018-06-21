using System;
using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.BLL.Customer
{
   public  interface IMciRequestService
   {
       List<ClientScopes> GetClientScopes(string clientId);
       List<CustomerIndexResult> GetCustomerAllIndexKeys(string sourceKey, string custRef, string targetKey);
        int? GetPersistantKey(NameAndAddress nameAndAddress);
       int? GetPersistantKey(string key, string value, string targetKey);
       long SaveCustomer( Models.Customer.CustomerLoadOrMatch customersLoad);
        long SaveCustomer(Models.Customer.CustomerLoadOrMatch customersLoad, out int customerId,out DateTime processedDatetime);
    }
}
