using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.DAL.GmdToAef;
using Saga.Gmd.WebApiServices.Models;

namespace Saga.Gmd.WebApiServices.BLL
{
    public interface IGmdToAfeService
    {
        bool Save(int customerId, bool isAfeUpdated);
        bool Update(int rowId, bool isAfeUpdated);
        List<GmdToAefModel> GetUnProcessedCustomers();
    }

    public class GmdToAfeService : IGmdToAfeService
    {
        private readonly IGmdToAfeDataAccess _dataAccess;
        public GmdToAfeService(IGmdToAfeDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public bool Save(int customerId, bool isAfeUpdated)
        {
            return _dataAccess.Save(customerId, isAfeUpdated);
        }

        public bool Update(int rowId, bool isAfeUpdated)
        {
            return _dataAccess.Update(rowId, isAfeUpdated);
        }

        public List<GmdToAefModel> GetUnProcessedCustomers()
        {
            return _dataAccess.GetUnProcessedCustomers();
        }
    }
}
