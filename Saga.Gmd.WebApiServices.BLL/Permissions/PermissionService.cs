
using System;
using System.Runtime.Remoting.Messaging;
using Saga.Gmd.WebApiServices.BLL.Permissions;
using Saga.Gmd.WebApiServices.DAL.Permissions;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Permissions;
using Saga.Gmd.WebApiServices.Models.ReturnMe;

namespace Saga.Gmd.WebApiServices.BLL.Permissions
{
    public class PermissionService : IPermissionService
    {
        private readonly IPermissionDataAccess _dataAccess;

        public PermissionService(IPermissionDataAccess dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public dynamic GetPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress, KeyValue keyValue)
        {
            return _dataAccess.GetCustomerPermission(parameter, nameAndAddress, keyValue);
        }

        public dynamic GetPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress, int customerId)
        {
            return _dataAccess.GetCustomerPermission(parameter, nameAndAddress, customerId );
        }


        public string PostPermission(Models.Gdpr.PermissionsCustomerLoadModel permission)
        {
            return _dataAccess.PostCustomerPermission(permission);
        }

        public int GetCustomerKeyForPermissionId(long permissionsId)
        {
            return _dataAccess.GetCustomerIdFromPermission(permissionsId);
        }
    }
}
