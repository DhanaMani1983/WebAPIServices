using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.DAL.Models.Interface
{
    public interface IPermissionDataAccess
    {
        List<MatchedCustomer> GetMatchingCustomers(CustomerMatchParameter p);

        List<CustomerPermission> GetCustomerPermission(string systemCode, IEnumerable<int> customerIds, string product,string message);
    }
}
