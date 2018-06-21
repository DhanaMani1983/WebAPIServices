using System.Collections.Generic;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.BLL.Customer
{

    public interface ICustomerDetailsService
    {
        CustomerInfoDetails GetCustomerDetail(KeyValuePair<string, string> keyValuePair, AddressType addressType, SuppressionOptions suppressionOptions);
        // CustomerInfoDetails GetCustomerDetails(int customerId,AddressType addressType);


    }
    public class CustomerDetailsService : ICustomerDetailsService
    {
        private readonly ICustomerDetailsDataAccess _customerDetailsDataAccess;
        private readonly ILog _logger;
        public CustomerDetailsService(ICustomerDetailsDataAccess customerDetailsDataAccess, ILog logger)
        {
            _customerDetailsDataAccess = customerDetailsDataAccess;
            _logger = logger;
        }

        public CustomerInfoDetails GetCustomerDetail(KeyValuePair<string, string> keyValuePair,
            AddressType addressType,
            SuppressionOptions suppressionOptions)
        {
            var customerDetails = _customerDetailsDataAccess.GetCustomerDetails(keyValuePair, addressType, suppressionOptions);
            return customerDetails;
        }

        /*
        public CustomerInfoDetails  GetCustomerDetails(int customerId, 
            AddressType addressType)
        {
            var customerInfoDetails = _customerDetailsDataAccess.GetCustomerDetails(customerId, addressType);
            return customerInfoDetails;
        } 
        */
             
    }
}
