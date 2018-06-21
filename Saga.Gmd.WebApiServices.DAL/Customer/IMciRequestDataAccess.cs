using System;
using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{
    public interface IMciRequestDataAccess
    {
        List<CustomerIndexResult> GetCustomerAllIndexKeys(string sourceKey, string custRef, string targetKey);
        List<ClientScopes> GetClientScopes(string clientId);
        int? GetPersistantKey(NameAndAddress nameAndAddress);
        long SaveCustomer(CustomerLoadOrMatch customerLoad);

        long SaveCustomer(Models.Customer.CustomerLoadOrMatch customerLoad, out int customerId,
            out DateTime processedDatetime);


    }
}
