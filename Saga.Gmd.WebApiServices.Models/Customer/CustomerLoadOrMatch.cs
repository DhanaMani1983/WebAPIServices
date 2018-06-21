using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Saga.Gmd.WebApiServices.Models.Security;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
 
    public class CustomerLoadOrMatch
    {
        public CustomerLoadOrMatch()
        {
            Customer = new CustomerLoad();
            AccessControl = new AccessControl();
        }
        public CustomerLoad Customer { get; set; }

        [JsonIgnore]
        public AccessControl AccessControl { get; set; }
    }
}
