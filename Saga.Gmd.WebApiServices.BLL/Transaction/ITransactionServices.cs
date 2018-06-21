using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.BLL.Transaction
{
    public interface ITransactionServices
    {
        int Save(Models.Transaction.TransactionParamter transaction);
    }
}
