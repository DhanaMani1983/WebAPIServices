using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Permissions
{
    public class Permission
    {
        public Permission()
        {
            Detail = new List<PermissionDetail>();
        }

        [JsonProperty(PropertyName = "Permissions")]
        public List<PermissionDetail> Detail { get;set;} 
    }
}
