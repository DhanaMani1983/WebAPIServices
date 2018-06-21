using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.DataAccessHandlers;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;

namespace Saga.Gmd.WebApiServices.DAL.Customer
{
    public interface ICustomerDataAccess
    {
        List<MatchedCustomer> GetMatchingCustomers(CustomerMatchParameter p);
        CustomerMatchInfoDetails GetCustomerMatchDetails(int customerIds, SuppressionOptions suppressionOptions);


    }

    public class CustomerMatchDataAccess : ICustomerDataAccess
    {
        public ILog Logger { get; }
        private string MciCrDbConnectionString { get; }
        private readonly ICustomerDetailsDataAccess _customerDetailsDataAccess;
        private readonly bool _logParameterValue;

        private readonly ICustomerMatchDataAccessHandler _customerMatchDataAccessHandler;


        public CustomerMatchDataAccess(ILog logger,
            ICustomerMatchDataAccessHandler customerMatchDataAccessHandler, 
            ICustomerDetailsDataAccess customerDetailsDataAccess)
        {
            Logger = logger;
            _customerDetailsDataAccess = customerDetailsDataAccess;
            _customerMatchDataAccessHandler = customerMatchDataAccessHandler;

            MciCrDbConnectionString = ConfigurationManager.AppSettings["MciCrConnection"];
            _logParameterValue = Convert.ToBoolean(ConfigurationManager.AppSettings["EnableObjectDump"]);

        }

        public List<MatchedCustomer> GetMatchingCustomers(CustomerMatchParameter p)
        {
            // GITCS-3 : Call new injected Handler inplace of the block below 
     
            return _customerMatchDataAccessHandler.MatchCustomers(p);   
        }

        public CustomerMatchInfoDetails GetCustomerMatchDetails(int customerId, SuppressionOptions suppressionOptions)
        {
            CustomerMatchInfoDetails customerMatchInfoDetails = new CustomerMatchInfoDetails();
            KeyValuePair<string, string> keyValuePair = new KeyValuePair<string, string>("PKEY", customerId.ToString());

            CustomerInfoDetails customerInfo = _customerDetailsDataAccess.GetCustomerDetails(keyValuePair, AddressType.Correspondence, suppressionOptions );

            var keys = _customerDetailsDataAccess.GetCustomerAllIndexKeys("PKEY", customerId.ToString(), "All");

            customerMatchInfoDetails.Title = customerInfo.Title;
            customerMatchInfoDetails.FirstName = customerInfo.FirstName;
            customerMatchInfoDetails.Surname = customerInfo.Surname;
            customerMatchInfoDetails.Dob = customerInfo.Dob;
            customerMatchInfoDetails.EmailAddress = customerInfo.Email;
            customerMatchInfoDetails.Homephone = customerInfo.HomePhone;
            customerMatchInfoDetails.MobilePhone = customerInfo.MobilePhone;
            customerMatchInfoDetails.SmsPhone = customerInfo.SmsPhone;
            customerMatchInfoDetails.Address = customerInfo.FullAddress;
            customerMatchInfoDetails.Postcode = ((CorrespondenceAddress)customerInfo.Address).Postcode;
            var firstOrDefault = keys.FirstOrDefault(x => x.SourceKey == "NUME");
            if (firstOrDefault != null)
                customerMatchInfoDetails.NumeroId = firstOrDefault.Keys;
            var customerIndexResult = keys.FirstOrDefault(x => x.SourceKey == "TARS");
            if (customerIndexResult != null)
                customerMatchInfoDetails.TaurusId = customerIndexResult.Keys;
            return customerMatchInfoDetails;
        }



    }
}
