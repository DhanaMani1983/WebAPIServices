using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.DAL.Models
{
    public class Brand
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public List<Division> ListOfDivision { get; set; }
    }
}
