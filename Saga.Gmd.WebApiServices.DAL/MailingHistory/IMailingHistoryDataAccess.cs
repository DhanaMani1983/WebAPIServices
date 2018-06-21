using System;
using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.MailingHistory;

namespace Saga.Gmd.WebApiServices.DAL.MailingHistory
{
    public interface IMailingHistoryDataAccess
    {
         List<MatchedCustomer> GetMatchingCustomers(CustomerMatchParameter p);

        List<MailingHistoryResult> GetCustomerMailingHistory(string systemCode, IEnumerable<int> customerIds, string message,
             DateTime? fromDate, DateTime? toDate,
             string product = null);

       
    }
}
