using log4net;
using Saga.Gmd.WebApiServices.DAL.Customer;
using Saga.Gmd.WebApiServices.Models.Customer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.BLL.Customer
{
    public class MciSourceKeyService : IMciSourceKeyService
    {        
        private static ILog _logger = LogManager.GetLogger(typeof(MciSourceKeyService));

        public MciSourceKeyService()
        {
        }

        public List<string> GetSourceKey()
        {
            List<string> sourcekeys = new List<string>();
            try
            {
                MciSourceDataAccess _mciSourceDataAccess = new MciSourceDataAccess();
                sourcekeys = _mciSourceDataAccess.GetAllSourceKeys();
            }
            catch (SqlException ex)
            {
                _logger.Error("MciSourceKeyService - GetSourceKey:" + "ErrorTag: " + ErrorTagProvider.ErrorTagDatabase + " -- " + ex.Message, ex);
                throw new Exception(string.Format(DatabaseMessage.DatabaseException, ErrorTagProvider.ErrorTagDatabase));
            }
            return sourcekeys;
        }
    }
}
