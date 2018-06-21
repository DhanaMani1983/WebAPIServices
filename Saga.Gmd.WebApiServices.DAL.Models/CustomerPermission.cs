using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.DAL.Models
{
    
    public class CustomerPermission
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public Brand Brand { get; set; }
    }
    
}
