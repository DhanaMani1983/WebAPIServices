

namespace Saga.Gmd.WebApiServices.DAL.Transaction
{
    public interface ITransactionDataAccess
    {
        int Save(Models.Transaction.TransactionParamter transaction);
    }
}