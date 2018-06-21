using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models
{
    public class GmdToAefModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public bool IsAfeNotified { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
