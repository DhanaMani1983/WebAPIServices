using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Permissions
{
    public class Division : DivisionBase
    {
        public Division()
        {
            SubDivisions = new List<DivisionBase>(); 
        }
        public List<DivisionBase> SubDivisions {get;set;} 
    }
}
