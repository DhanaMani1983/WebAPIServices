using System.Collections.Generic;

namespace Saga.Gmd.WebApiServices.Models.Security
{
    public interface IAccessControl
    {
        IList<CustomerKeyAccess> CustomerKeyAccess { get; set; }
        IList<ModuleAccess> ModuleAccess { get; set; }
    }
}