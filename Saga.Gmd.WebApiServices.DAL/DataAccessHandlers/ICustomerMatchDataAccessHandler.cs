using Saga.Gmd.WebApiServices.Models.Customer;
using System.Collections.Generic;

namespace Saga.Gmd.WebApiServices.DAL.DataAccessHandlers
{
    public interface ICustomerMatchDataAccessHandler
    {
        List<MatchedCustomer> MatchCustomers(CustomerMatchParameter customerMatchParameter);
    }
}
