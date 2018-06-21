using System;
using log4net;
using Saga.Gmd.WebApiServices.BLL.Transaction;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Api.Components.Transaction
{
    public class TransactionProcess
    {
        private readonly  WebApiServices.Models.Transaction.TransactionParamter _transaction;
        private readonly ITransactionServices _transactionServices;
        private readonly ILog _logger;

        public TransactionProcess(WebApiServices.Models.Transaction.TransactionParamter transaction,
            ITransactionServices transactionServices, ILog logger)
        {
            _transaction = transaction;
            _transactionServices = transactionServices;
            _logger = logger;
        }

        public object Process()
        {
            int output = 0;
            try
            {
                output = _transactionServices.Save(_transaction);
            }
            catch (Exception ex)
            {
                _logger.Error("TransactionProcess: " + "ErrorTag: " + ErrorTagProvider.ErrorTag + " -- " + ex.Message,
                    ex);
                throw new Exception(ex.Message);
            }
            return "Transaction loaded successfuly.";
        }
    }
}