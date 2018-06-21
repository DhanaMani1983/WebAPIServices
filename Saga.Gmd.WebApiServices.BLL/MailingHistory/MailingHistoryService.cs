using System;
using System.Collections.Generic;
using System.Dynamic;
using log4net;
using Saga.Gmd.WebApiServices.DAL.MailingHistory;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Customer;
using Saga.Gmd.WebApiServices.Models.MailingHistory;


namespace Saga.Gmd.WebApiServices.BLL.MailingHistory
{
    public class MailingHistoryService : IMailingHistoryService
    {
        private readonly IMailingHistoryDataAccess _dataAccess;
        public readonly ILog _logger;

        public MailingHistoryService(IMailingHistoryDataAccess dataAccess, ILog logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public dynamic GetMailingHistoryList(Models.MailingHistory.MailingHistory mailingHistory = null, NameAndAddress nameAndAddress = null, IEnumerable<int> customerIdList = null)
        {

            CustomerMatchParameter parameter = new CustomerMatchParameter();
            if (nameAndAddress != null)
            {

                parameter.MatchType = "NAME_AND_ADDRESS";
                parameter.Address1 = nameAndAddress.Address.Address1;
                parameter.Address2 = nameAndAddress.Address.Address2;
                parameter.Address3 = nameAndAddress.Address.Address3;
                parameter.Address4 = nameAndAddress.Address.Address4;
                parameter.Postcode = nameAndAddress.Address.Postcode;
                parameter.Dob = nameAndAddress.Dob;
                parameter.Phone = nameAndAddress.Phone;
                parameter.Email = nameAndAddress.Email;
                parameter.FirstName = nameAndAddress.FirstName;
                parameter.Surname = nameAndAddress.Surname;
                parameter.Title = nameAndAddress.Title;
                if (mailingHistory != null)
                {
                    parameter.FromDate = mailingHistory.FromDate;
                    parameter.ToDate = mailingHistory.ToDate;
                    parameter.Product = mailingHistory.Product;
                }
            }

            List<ExpandoObject> expandoObject = new List<ExpandoObject>();

            try
            {
                List<MatchedCustomer> customerIds = new List<MatchedCustomer>();
                if (customerIdList == null)
                {
                    customerIds = _dataAccess.GetMatchingCustomers(parameter);
                }
                else
                {
                    foreach (var id in customerIdList)
                    {
                        customerIds.Add(new MatchedCustomer { CustomerId = id });
                    }
                }

                List<int> ids = new List<int>();

                foreach (var item in customerIds)
                {
                    ids.Add(item.CustomerId);
                }

                List<MailingHistoryResult> list = new List<MailingHistoryResult>();
                if (mailingHistory != null)
                {
                    
                    list = _dataAccess.GetCustomerMailingHistory(null, ids, mailingHistory.Product,
                        mailingHistory.FromDate, mailingHistory.ToDate, string.Empty); 

                    if (mailingHistory.FieldsTobeReturned.Contains(null) || mailingHistory.FieldsTobeReturned.Count == 0 )
                    {
                        return list;
                    }
                }
                else
                {
                    list = _dataAccess.GetCustomerMailingHistory(null, ids, string.Empty,
                       null, null, null);
                    return list;
                }

                DataShapedObject dataShapedObject = new DataShapedObject();
                foreach (var item in list)
                {
                    if (mailingHistory.FieldsTobeReturned != null &&
                        mailingHistory.FieldsTobeReturned.Count != 0)
                    {
                        expandoObject.Add(dataShapedObject.Create(item, mailingHistory.FieldsTobeReturned));
                    }
                }
            }

            catch (Exception ex)
            {
                _logger.Info("MailingHistoryService.GetMailingHistoryList " + ex.Message, ex);
                throw new Exception(ex.Message);
            }

            return expandoObject;

        }


    }
}