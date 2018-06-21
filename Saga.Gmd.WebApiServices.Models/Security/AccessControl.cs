using System.Collections.Generic;

namespace Saga.Gmd.WebApiServices.Models.Security
{
    public class AccessControl  
    {
        public AccessControl()
        {
            CustomerKeyAccess = new List<CustomerKeyAccess>();
            ModuleAccess = new List<ModuleAccess>();
        }
        public List<CustomerKeyAccess> CustomerKeyAccess { get; set; } 
        public List<ModuleAccess> ModuleAccess { get; set; }
    }
}