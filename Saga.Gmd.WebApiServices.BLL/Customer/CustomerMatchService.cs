using System.Collections.Generic;
using log4net;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.BLL.Customer
{

    public interface ICustomerMatchService
    {
        // List<CustomerMatchInfoDetails> GetMatchingCustomers(NameAndAddress nameAndAddress, string matchType);
        List<CustomerMatchInfoDetails> GetMatchingCustomers(NameAndAddress nameAndAddress, SuppressionOptions suppressionOptions, string matchType);

    }
    public class CustomerMatchService : ICustomerMatchService
    {
        private readonly ICustomerDataAccess _customerDataAccess;
        private readonly ILog _logger;

        public CustomerMatchService(ICustomerDataAccess customerDataAccess, ILog logger)
        {
            _customerDataAccess = customerDataAccess;
            _logger = logger;
        }

        public List<CustomerMatchInfoDetails> GetMatchingCustomers(NameAndAddress nameAndAddress, SuppressionOptions suppressionOptions, string matchType)
        {
            CustomerMatchParameter customerMatchParameter = new CustomerMatchParameter();


            customerMatchParameter.MatchType = matchType;
            customerMatchParameter.Address1 = nameAndAddress.Address.Address1;
            customerMatchParameter.Address2 = nameAndAddress.Address.Address2;
            customerMatchParameter.Address3 = nameAndAddress.Address.Address3;
            customerMatchParameter.Address4 = nameAndAddress.Address.Address4;
            customerMatchParameter.Postcode = nameAndAddress.Address.Postcode;
            customerMatchParameter.Dob = nameAndAddress.Dob;
            customerMatchParameter.Phone = nameAndAddress.Phone;
            customerMatchParameter.Email = nameAndAddress.Email;
            customerMatchParameter.FirstName = nameAndAddress.FirstName;
            customerMatchParameter.Surname = nameAndAddress.Surname;
            customerMatchParameter.Title = nameAndAddress.Title;

            List<MatchedCustomer> matchedCustomers =  _customerDataAccess.GetMatchingCustomers(customerMatchParameter);

            List<CustomerMatchInfoDetails> customerMatchDetails = new List<CustomerMatchInfoDetails>();
            foreach (var c in matchedCustomers)
            {
                customerMatchDetails.Add(_customerDataAccess.GetCustomerMatchDetails(c.CustomerId, suppressionOptions));
            }

            return customerMatchDetails;
        }
    }
}
