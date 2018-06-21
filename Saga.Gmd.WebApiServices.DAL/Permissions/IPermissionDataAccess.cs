using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.ReturnMe;

namespace Saga.Gmd.WebApiServices.DAL.Permissions
{
    public interface IPermissionDataAccess
    {
        //Permission GetCustomerPermission(int customerIds);

        dynamic GetCustomerPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress,
            KeyValue keyValue);

        dynamic GetCustomerPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress,
            int customerId);
        
        string PostCustomerPermission(Models.Gdpr.PermissionsCustomerLoadModel permission);

        int GetCustomerIdFromPermission(long permissionsId);
    }
}
