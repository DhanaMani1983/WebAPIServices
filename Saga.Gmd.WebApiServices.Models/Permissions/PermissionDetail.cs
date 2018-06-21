using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Permissions
{
    public class PermissionDetail
    {
        public PermissionDetail()
        {
            Channel = new List<Channel>();
            Brand = string.Empty;
            Consented = false;
            ConsentDate = null;
        }
        public string Brand { get; set; }

        public bool Consented { get; set; }
        
        public int PermissionKey { get; set; }

        public DateTime? ConsentDate { get; set; }

        [JsonProperty(PropertyName ="Channel")]
        public List<Channel> Channel { get; set; }
        
    }
}
