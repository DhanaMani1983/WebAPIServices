using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using Saga.Gmd.WebApiServices.DAL.Transaction;

namespace Saga.Gmd.WebApiServices.BLL.Transaction
{
    public class TransactionServices : ITransactionServices
    {
        private readonly ITransactionDataAccess _transactionDataAccess;
        private readonly ILog _logger;
        public TransactionServices(ITransactionDataAccess transactionDataAccess, ILog logger)
        {
            _transactionDataAccess = transactionDataAccess;
            _logger = logger;
        }

        public int Save(Models.Transaction.TransactionParamter transaction)
        {
            int output;
            try
            {
                output = _transactionDataAccess.Save(transaction);
            }
            catch (Exception ex)
            {
                _logger.Error("TransactionServices ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return output;
        }
    }
}
