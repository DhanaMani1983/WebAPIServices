using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Saga.Gmd.WebApiServices.Api.Models;
using Saga.Gmd.WebApiServices.DAL.Models.Interface;

namespace Saga.Gmd.WebApiServices.BLL
{
    public class PermissionService : IPermissionService
    {

        private readonly IPermissionDataAccess _dataAccess;

        public PermissionService(IPermissionDataAccess dataAccess)
        {
            _dataAccess = dataAccess; 
        }

        public dynamic GetPermission(NameAndAddress nameAndAddress = null, IEnumerable<int> customerIdList = null)            
        {
           return _dataAccess.GetCustomerPermission(null, customerIdList,null,null);
        }
    }
}
