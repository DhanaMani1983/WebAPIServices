using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saga.Gmd.WebApiServices.Models.Customer
{
    public class CustomerKeyMove
    {
        public string OriginalSourceKey { get; set; }
        public string OriginalSourceId { get; set; }
        public string NewSourceKey { get; set; }
        public string NewSourceId { get; set; }
        public string ToMoveSourceKey { get; set; }
        public string ToMoveSourceId { get; set; }
    }
}
