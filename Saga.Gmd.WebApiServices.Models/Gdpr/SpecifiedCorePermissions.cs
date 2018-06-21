using System.Collections.Generic;
using Saga.Gmd.WebApiServices.Common;

namespace Saga.Gmd.WebApiServices.Models.Gdpr
{
    public class SpecifiedCorePermissions
    {
        public List<PermissionCategoryDisplayValue> Get(CorePermission corePermission)
        {
            List<PermissionCategoryDisplayValue> permissions = new List<PermissionCategoryDisplayValue>();
            switch (corePermission)
            {
                case CorePermission.Core:
                    permissions.Add(PermissionCategoryDisplayValue.Insurance);
                    permissions.Add(PermissionCategoryDisplayValue.Travel);
                    permissions.Add(PermissionCategoryDisplayValue.Money);
                    break;
                case CorePermission.CoreAndHealth:
                    permissions.Add(PermissionCategoryDisplayValue.Insurance);
                    permissions.Add(PermissionCategoryDisplayValue.Travel);
                    permissions.Add(PermissionCategoryDisplayValue.Money);
                    permissions.Add(PermissionCategoryDisplayValue.Health);
                    break;
                case CorePermission.CoreAndMagazine:
                    permissions.Add(PermissionCategoryDisplayValue.Insurance);
                    permissions.Add(PermissionCategoryDisplayValue.Travel);
                    permissions.Add(PermissionCategoryDisplayValue.Money);
                    permissions.Add(PermissionCategoryDisplayValue.Magazine);
                    break;
                case CorePermission.CoreAndMembership:
                    permissions.Add(PermissionCategoryDisplayValue.Insurance);
                    permissions.Add(PermissionCategoryDisplayValue.Travel);
                    permissions.Add(PermissionCategoryDisplayValue.Money);
                    permissions.Add(PermissionCategoryDisplayValue.Membership);
                    break;
                case CorePermission.CoreAndRetirementVillage:
                    permissions.Add(PermissionCategoryDisplayValue.Insurance);
                    permissions.Add(PermissionCategoryDisplayValue.Travel);
                    permissions.Add(PermissionCategoryDisplayValue.Money);
                    permissions.Add(PermissionCategoryDisplayValue.RetirementVillages);
                    break;
                case CorePermission.Membership:
                    permissions.Add(PermissionCategoryDisplayValue.Membership);
                    break;
            }

            return permissions;
        }
    }


   


}
