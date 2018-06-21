using Saga.Gmd.WebApiServices.Models;
using Saga.Gmd.WebApiServices.Models.Permissions;
using Saga.Gmd.WebApiServices.Models.ReturnMe;

namespace Saga.Gmd.WebApiServices.BLL.Permissions
{
    public interface IPermissionService
    {
        dynamic GetPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress,
            KeyValue keyValue);

        dynamic GetPermission(ReturnMePermissions parameter, NameAndAddress nameAndAddress, int customerId);

        string PostPermission(Models.Gdpr.PermissionsCustomerLoadModel permission);

        int GetCustomerKeyForPermissionId(long permissionsId);
    }
}
