using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models.Customer;
using log4net;
using Saga.Gmd.WebApiServices.Common;
using System.Net;

namespace Saga.Gmd.WebApiServices.BLL.Customer
{

    public interface ICustomerKeyMoveService
    {
        dynamic MoveCustomerKey(CustomerKeyMove customerKeyMove);
    }
    public class CustomerKeyMoveService:ICustomerKeyMoveService
    {

        private readonly ICustomerKeyMoveDataAccess _customerKeyMoveDataAccess;
        private readonly ILog _logger;
        public CustomerKeyMoveService(ICustomerKeyMoveDataAccess customerKeyMoveDataAccess, ILog logger)
        {
            _customerKeyMoveDataAccess = customerKeyMoveDataAccess;
            _logger = logger;
        }

        public dynamic MoveCustomerKey(CustomerKeyMove customerKeyMove)
        {
            string output;
            try
            {
                if (_customerKeyMoveDataAccess.MoveCustomerKey(customerKeyMove))
                {
                    output = "Key Successfully Moved";
                }
                else
                {
                    output = "Failed to move key for unknown reason";
                }

            }
            catch (Exception ex)
            {
                _logger.Error("MoveCustomerKey ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);

                if (ex.GetType() == typeof(DataException))
                {
                    return new object[]{ HttpStatusCode.NotFound,ex.Message};
                    throw new Exception(string.Format(ex.Message, ErrorTagProvider.ErrorTagDatabase));
                }

                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return output;
        }
    }
}
