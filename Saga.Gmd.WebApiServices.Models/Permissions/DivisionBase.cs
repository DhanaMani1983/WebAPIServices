using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Permissions
{
    public class DivisionBase
    {
        public string What { get; set; } 
		public string Value { get; set;}
        public int Level { get; set; }
    }
}
